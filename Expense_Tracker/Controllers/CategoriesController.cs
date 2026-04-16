using Expense_Tracker.Data;
using Expense_Tracker.DTO;
using ExpenseTracker.Data;
using ExpenseTracker.DTOs;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        //We use this method to identify the currently logged-in user inside the controller.
        private int GetUserId() =>
            int.Parse(User.FindFirst("UserId")!.Value);

        // GET /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = GetUserId();
            var categories = await _context.Categories
                .Where(c => c.IsDefault || c.UserId == userId)
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Color = c.Color,
                    Icon = c.Icon,
                    IsDefault = c.IsDefault
                })
                .ToListAsync();

            return Ok(categories);
        }

        // POST /api/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            int userId = GetUserId();
            var category = new Category
            {
                UserId = userId,
                Name = dto.Name,
                Color = dto.Color,
                Icon = dto.Icon,
                IsDefault = false
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        // PUT /api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
        {
            int userId = GetUserId();
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.UserId == userId);

            if (category == null) return NotFound("Category not found.");
            if (category.IsDefault) return BadRequest("Cannot edit default categories.");

            category.Name = dto.Name;
            category.Color = dto.Color;
            category.Icon = dto.Icon;

            await _context.SaveChangesAsync();
            return Ok(category);
        }

        // DELETE /api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetUserId();
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.UserId == userId);

            if (category == null) return NotFound("Category not found.");
            if (category.IsDefault) return BadRequest("Cannot delete default categories.");

            // Reassign expenses to "Uncategorized"
            var uncategorized = await _context.Categories
                .FirstOrDefaultAsync(c => c.IsDefault && c.Name == "Uncategorized");

            var expenses = _context.Expenses.Where(e => e.CategoryId == id);
            await expenses.ForEachAsync(e => e.CategoryId = uncategorized!.CategoryId);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Category deleted and expenses reassigned.");
        }
    }
}