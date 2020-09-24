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
    public class CategoriesController : ControllerBase
    {
        private readonly BudgetContext _db;

        public CategoriesController(BudgetContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll()
        {
            var userId = User.GetUserId();
            var categories = await _db.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Type)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return categories.Select(c => c.ToDto()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var category = await FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return category.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(SaveCategoryRequest request)
        {
            var name = request.Name.Trim();
            if (await NameTakenAsync(name))
            {
                return Conflict(new { message = $"You already have a category called \"{name}\"." });
            }

            var category = new Category
            {
                UserId = User.GetUserId(),
                Name = name,
                Type = request.Type,
                Color = request.Color
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = category.Id }, category.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaveCategoryRequest request)
        {
            var category = await FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var name = request.Name.Trim();
            if (await NameTakenAsync(name, id))
            {
                return Conflict(new { message = $"You already have a category called \"{name}\"." });
            }

            category.Name = name;
            category.Type = request.Type;
            category.Color = request.Color;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (await _db.Transactions.AnyAsync(t => t.CategoryId == id))
            {
                return Conflict(new { message = "This category still has transactions. Move or delete them first." });
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private Task<Category> FindAsync(int id)
        {
            var userId = User.GetUserId();
            return _db.Categories.SingleOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        private Task<bool> NameTakenAsync(string name, int? exceptId = null)
        {
            var userId = User.GetUserId();
            return _db.Categories.AnyAsync(c => c.UserId == userId && c.Name == name && c.Id != exceptId);
        }
    }
}
