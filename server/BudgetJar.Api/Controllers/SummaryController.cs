using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetJar.Api.Data;
using BudgetJar.Api.Dtos;
using BudgetJar.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetJar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly BudgetContext _db;

        public SummaryController(BudgetContext db)
        {
            _db = db;
        }

        [HttpGet("monthly")]
        public async Task<ActionResult<List<MonthlyTotalDto>>> Monthly([FromQuery] int? year)
        {
            var y = year ?? DateTime.Today.Year;

            var rows = await _db.Transactions
                .Where(t => t.Date.Year == y)
                .Select(t => new { t.Date.Month, t.Category.Type, t.Amount })
                .GroupBy(x => new { x.Month, x.Type })
                .Select(g => new { g.Key.Month, g.Key.Type, Total = g.Sum(x => x.Amount) })
                .ToListAsync();

            return rows
                .GroupBy(r => r.Month)
                .OrderBy(g => g.Key)
                .Select(g => new MonthlyTotalDto
                {
                    Year = y,
                    Month = g.Key,
                    Income = g.Where(r => r.Type == TransactionType.Income).Sum(r => r.Total),
                    Expense = g.Where(r => r.Type == TransactionType.Expense).Sum(r => r.Total)
                })
                .ToList();
        }
    }
}
