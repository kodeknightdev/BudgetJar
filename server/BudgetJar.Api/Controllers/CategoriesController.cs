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
            var categories = await _db.Categories
                .OrderBy(c => c.Type)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return categories.Select(ToDto).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return ToDto(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(SaveCategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name.Trim(),
                Type = request.Type,
                Color = request.Color
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = category.Id }, ToDto(category));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaveCategoryRequest request)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = request.Name.Trim();
            category.Type = request.Type;
            category.Color = request.Color;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static CategoryDto ToDto(Category c)
        {
            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                Color = c.Color
            };
        }
    }
}
