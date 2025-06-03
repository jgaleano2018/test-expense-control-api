using Microsoft.AspNetCore.Mvc;
using test_expense_control_api.DomainApi.Services.Interfaces;
using test_expense_control_api.DomainApi.Services;
using test_expense_control_api.DomainApi.Model;
using System.Linq.Expressions;

namespace test_expense_control_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {

        private readonly IDetailExpensesService _detailExpensesService;
        private readonly IHeaderExpensesService _headerExpensesService;

        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(DetailExpensesService detailExpensesService, HeaderExpensesService headerExpensesService, ILogger<ExpensesController> logger)
        {
            _detailExpensesService = detailExpensesService;
            _headerExpensesService = headerExpensesService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpenses(Expenses expenses)
        {
            try
            {
                HeaderExpenses headerExpenses = expenses.HeaderExpenses;
                DetailExpenses detailExpenses = expenses.DetailExpenses;
                    

                if (String.IsNullOrEmpty(headerExpenses.Observations))
                {
                    return BadRequest("You must enter all required fields");
                }

                var resultHeaderExpenses = await _headerExpensesService.AddHeaderExpensesAsync(headerExpenses);

                if (resultHeaderExpenses != null) {

                    detailExpenses.IdHeaderExpenses = resultHeaderExpenses.IdHeaderExpenses;

                    var resultDetailExpenses = await _detailExpensesService.AddDetailExpensesAsync(detailExpenses);

                }

                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpenses(int id, [FromBody] Expenses expenses)
        {
            try
            {
                if (id != expenses.HeaderExpenses.IdHeaderExpenses)
                {
                    return BadRequest("Header Expenses ID mismatch");
                }

                HeaderExpenses headerExpenses = expenses.HeaderExpenses;
                DetailExpenses detailExpenses = expenses.DetailExpenses;


                if (String.IsNullOrEmpty(headerExpenses.Observations))
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingHeaderExpenses = await _headerExpensesService.GetHeaderExpensesByIdAsync(id);
                if (existingHeaderExpenses == null)
                {
                    return NotFound("Header Expenses not found");
                }

                var result = await _headerExpensesService.UpdateHeaderExpensesAsync(headerExpenses);


                var existingDetailExpenses = await _detailExpensesService.GetDetailExpensesByIdAsync(detailExpenses.IdDetailExpenses);
                if (existingHeaderExpenses == null)
                {
                    return NotFound("Header Expenses not found");
                }

                var result2 = await _detailExpensesService.UpdateDetailExpensesAsync(detailExpenses);


                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenses(int id)
        {
            try
            {
                var detailExpenses = await _detailExpensesService.GetDetailExpensesByIdAsync(id);
                if (detailExpenses == null)
                {
                    return NotFound("Detail Expenses not found");
                }
                await _detailExpensesService.DeleteDetailExpensesAsync(detailExpenses);

                var headerExpenses = await _headerExpensesService.GetHeaderExpensesByIdAsync(detailExpenses.IdHeaderExpenses);
                if (headerExpenses == null)
                {
                    return NotFound("Header Expenses not found");
                }

                await _headerExpensesService.DeleteHeaderExpensesAsync(headerExpenses);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("header/{id}")]
        public async Task<IActionResult> GetExpensesHeader(int id)
        {
            try
            {
                var expenseHeader = await _headerExpensesService.GetHeaderExpensesByIdAsync(id);
                if (expenseHeader == null)
                {
                    return NotFound("Expense not found");
                }
                return Ok(expenseHeader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetExpensesDetail(int id)
        {
            try
            {
                var expenseDetail = await _detailExpensesService.GetDetailExpensesByIdAsync(id);
                if (expenseDetail == null)
                {
                    return NotFound("Expense not found");
                }
                return Ok(expenseDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                var expenses = await _headerExpensesService.GetAllHeaderExpensesAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMonetaryFundType()
        {
            try
            {
                var expenses = await _headerExpensesService.GetMonetaryFundTypeAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBusiness()
        {
            try
            {
                var expenses = await _headerExpensesService.GetBusinessAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentType()
        {
            try
            {
                var expenses = await _headerExpensesService.GetDocumentTypeAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}