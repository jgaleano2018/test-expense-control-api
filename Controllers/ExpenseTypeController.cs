using Microsoft.AspNetCore.Mvc;
using test_expense_control_api.DomainApi.Services.Interfaces;
using test_expense_control_api.DomainApi.Services;
using test_expense_control_api.DomainApi.Model;

namespace test_expense_control_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseTypeController : ControllerBase
    {

        private readonly IExpenseTypeService _expenseTypeService;
        private readonly ILogger<ExpenseTypeController> _logger;

        public ExpenseTypeController(ExpenseTypeService expenseTypeService, ILogger<ExpenseTypeController> logger)
        {
            _expenseTypeService = expenseTypeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpenseType(ExpenseType expenseType)
        {
            try
            {

                if (String.IsNullOrEmpty(expenseType.Name))
                {
                    return BadRequest("You must enter all required fields");
                }

                var result = await _expenseTypeService.AddExpenseTypeAsync(expenseType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeposit(int id, [FromBody] ExpenseType expenseType)
        {
            try
            {
                if (id != expenseType.IdExpenseType)
                {
                    return BadRequest("Expense Type ID mismatch");
                }

                if (String.IsNullOrEmpty(expenseType.Name))
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingExpenseType = await _expenseTypeService.GetExpenseTypeByIdAsync(id);
                if (existingExpenseType == null)
                {
                    return NotFound("Deposit not found");
                }
                var result = await _expenseTypeService.UpdateExpenseTypeAsync(expenseType);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseType(int id)
        {
            try
            {
                var expenseType = await _expenseTypeService.GetExpenseTypeByIdAsync(id);
                if (expenseType == null)
                {
                    return NotFound("Expense Type not found");
                }
                await _expenseTypeService.DeleteExpenseTypeAsync(expenseType);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeposit(int id)
        {
            try
            {
                var expenseType = await _expenseTypeService.GetExpenseTypeByIdAsync(id);
                if (expenseType == null)
                {
                    return NotFound("Expense Type not found");
                }
                var result = _expenseTypeService.UpdateExpenseTypeAsync(expenseType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenseType()
        {
            try
            {
                var expenseType = await _expenseTypeService.GetAllExpenseTypeAsync();
                return Ok(expenseType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}
