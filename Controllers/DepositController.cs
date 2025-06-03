using Microsoft.AspNetCore.Mvc;
using test_expense_control_api.DomainApi.Services.Interfaces;
using test_expense_control_api.DomainApi.Services;
using test_expense_control_api.DomainApi.Model;

namespace test_expense_control_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {

        private readonly IDepositService _depositService;
        private readonly ILogger<DepositController> _logger;

        public DepositController(DepositService depositService, ILogger<DepositController> logger)
        {
            _depositService = depositService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddDeposit(Deposit deposit)
        {
            try
            {

                if (String.IsNullOrEmpty(deposit.Account))
                {
                    return BadRequest("You must enter all required fields");
                }

                var result = await _depositService.AddDepositAsync(deposit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeposit(int id, [FromBody] Deposit deposit)
        {
            try
            {
                if (id != deposit.IdDeposit)
                {
                    return BadRequest("Deposit ID mismatch");
                }

                if (String.IsNullOrEmpty(deposit.Account))
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingDeposit = await _depositService.GetDepositByIdAsync(id);
                if (existingDeposit == null)
                {
                    return NotFound("Deposit not found");
                }
                var result = await _depositService.UpdateDepositAsync(deposit);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            try
            {
                var deposit = await _depositService.GetDepositByIdAsync(id);
                if (deposit == null)
                {
                    return NotFound("Deposit not found");
                }
                await _depositService.DeleteDepositAsync(deposit);
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
                var deposit = await _depositService.GetDepositByIdAsync(id);
                if (deposit == null)
                {
                    return NotFound("Deposit not found");
                }
                return Ok(deposit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeposit()
        {
            try
            {
                var deposits = await _depositService.GetAllDepositsAsync();
                return Ok(deposits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}