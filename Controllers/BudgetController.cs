using Microsoft.AspNetCore.Mvc;
//using AutoMapper;
using test_expense_control_api.DomainApi.Services.Interfaces;
using test_expense_control_api.DomainApi.Services;
using test_expense_control_api.DomainApi.Model;

namespace test_expense_control_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {

        private readonly IBudgetService _budgetService;
        private readonly ILogger<BudgetController> _logger;
        //private readonly IMapper _mapper;

        public BudgetController(BudgetService budgetService, ILogger<BudgetController> logger)
        {
            _budgetService = budgetService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddBudget(Budget budget)
        {
            try
            {
                if (String.IsNullOrEmpty(budget.Account) || String.IsNullOrEmpty(budget.Description))
                {
                    return BadRequest("You must enter all required fields");
                }

                var result = await _budgetService.AddBudgetAsync(budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] Budget budget)
        {
            try
            {
                if (id != budget.IdBudget)
                {
                    return BadRequest("Budget ID mismatch");
                }

                if (!String.IsNullOrEmpty(budget.Account) || !String.IsNullOrEmpty(budget.Description))
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingBudget = await _budgetService.GetBudgetByIdAsync(id);
                if (existingBudget == null)
                {
                    return NotFound("Budget not found");
                }
                var result = await _budgetService.UpdateBudgetAsync(budget);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            try
            {
                var budget = await _budgetService.GetBudgetByIdAsync(id);
                if (budget == null)
                {
                    return NotFound("Budget not found");
                }
                await _budgetService.DeleteBudgetAsync(budget);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBudget(int id)
        {
            try
            {
                var budget = await _budgetService.GetBudgetByIdAsync(id);
                if (budget == null)
                {
                    return NotFound("Budget not found");
                }
                var result = _budgetService.UpdateBudgetAsync(budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBudget()
        {
            try
            {
                var budgets = await _budgetService.GetAllBudgetsAsync();
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}