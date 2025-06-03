using Microsoft.AspNetCore.Mvc;
using test_expense_control_api.DomainApi.Services.Interfaces;
using test_expense_control_api.DomainApi.Services;
using test_expense_control_api.DomainApi.Model;

namespace test_expense_control_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Phone) || String.IsNullOrEmpty(user.Address) || String.IsNullOrEmpty(user.UserName) || String.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("You must enter all required fields");
                }

                var result = await _userService.AddUserAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeposit(int id, [FromBody] User user)
        {
            try
            {
                if (id != user.IdUser)
                {
                    return BadRequest("User ID mismatch");
                }

                if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Phone) || String.IsNullOrEmpty(user.Address) || String.IsNullOrEmpty(user.UserName) || String.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("You must enter all required fields");
                }

                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound("User not found");
                }
                var result = await _userService.UpdateUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                await _userService.DeleteUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                var result = _userService.UpdateUserAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var user = await _userService.GetAllUserAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}
