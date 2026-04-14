using Expense_Tracker.Services;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _service;

        public ExpenseController(ExpenseService service)
        {
            _service = service;
        }

        // ➕ ADD EXPENSE
        [HttpPost]
        public async Task<IActionResult> AddExpense(ExpenseCreateDTO dto)
        {
            var message = _service.AddExpense(dto);
            return Ok(message);
        }

        // 📄 GET EXPENSES
        [HttpGet("user/{userId}")]
        public IActionResult GetExpenses(int userId)
        {
            var data = _service.GetExpensesByUserId(userId);
            return Ok(data);
        }

        // ❌ DELETE EXPENSE
        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var result = _service.DeleteExpense(id);

            if (result == "Expense not found")
                return NotFound("Expense not found");

            return Ok("Deleted successfully");
        }
    }
}