using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetJar.Api.Data;
using BudgetJar.Api.Dtos;
using BudgetJar.Api.Extensions;
using BudgetJar.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetJar.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly BudgetContext _db;

        public TransactionsController(BudgetContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAll([FromQuery] int? year, [FromQuery] int? month)
        {
            var userId = User.GetUserId();
            var query = _db.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.UserId == userId);

            if (year.HasValue)
            {
                query = query.Where(t => t.Date.Year == year.Value);
            }

            if (month.HasValue)
            {
                query = query.Where(t => t.Date.Month == month.Value);
            }

            var transactions = await query
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .ToListAsync();

            return transactions.Select(t => t.ToDto()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> Get(int id)
        {
            var transaction = await FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return transaction.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(SaveTransactionRequest request)
        {
            if (!await OwnsCategoryAsync(request.CategoryId))
            {
                ModelState.AddModelError(nameof(request.CategoryId), "Unknown category.");
                return ValidationProblem(ModelState);
            }

            var transaction = new Transaction
            {
                Amount = request.Amount,
                Date = request.Date.Value.Date,
                Description = request.Description?.Trim(),
                CategoryId = request.CategoryId
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
            await _db.Entry(transaction).Reference(t => t.Category).LoadAsync();

            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaveTransactionRequest request)
        {
            var transaction = await FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            if (!await OwnsCategoryAsync(request.CategoryId))
            {
                ModelState.AddModelError(nameof(request.CategoryId), "Unknown category.");
                return ValidationProblem(ModelState);
            }

            transaction.Amount = request.Amount;
            transaction.Date = request.Date.Value.Date;
            transaction.Description = request.Description?.Trim();
            transaction.CategoryId = request.CategoryId;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private Task<Transaction> FindAsync(int id)
        {
            var userId = User.GetUserId();
            return _db.Transactions
                .Include(t => t.Category)
                .SingleOrDefaultAsync(t => t.Id == id && t.Category.UserId == userId);
        }

        private Task<bool> OwnsCategoryAsync(int categoryId)
        {
            var userId = User.GetUserId();
            return _db.Categories.AnyAsync(c => c.Id == categoryId && c.UserId == userId);
        }
    }
}
