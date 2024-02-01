using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JetstreamSkiserviceAPI.Models;

namespace JetstreamSkiserviceAPI.Controllers
{
    /// <summary>
    /// Controller for handling HTTP requests related to login/employee-login
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<EmployeesController> _logger;

        /// <summary>
        /// Constructor for the EmployeeController
        /// </summary>
        /// <param name="tokenService">Implementation of the ITokenService interface</param>
        /// <param name="logger">Logger for logging information and errors</param>
        public EmployeesController(ITokenService tokenService, ILogger<EmployeesController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// List all existing Employees
        /// </summary>
        public List<Employee> Employee { get; private set; }

        /// <summary>
        /// Handles user login requests by checking credentials and returns a token if successful
        /// </summary>
        /// <param name="employee">Contains the username and password for authentication</param>
        /// <returns>A response indicating if the login was successful or not</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] AuthDto employee)
        {
            try
            {
                var employees = await _tokenService.GetEmployees();

                foreach (var user in employees)
                {
                    if (employee.Username == user.Username)
                    {
                        if (employee.Password == user.Password)
                        {
                            if (user.Attempts >= 3)
                            {
                                return Unauthorized("Your account is locked. Please contact administrator.");
                            }
                            else
                            {
                                _tokenService.Unban(user.EmployeeId);
                                return Ok(new { username = user.Username, token = _tokenService.CreateToken(user.Username) });
                            }
                        }
                        else
                        {
                            _tokenService.Attempts(user.EmployeeId);
                            if (user.Attempts >= 3)
                            {
                                return Unauthorized("Your account is locked. Please contact administrator.");
                            }
                            return Unauthorized($"Invalid password!! {3 - user.Attempts} attempts left.");
                        }
                    }
                }
                return Unauthorized("User not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
            }
        }

        /// <summary>
        /// Unbans a user account based on the provided ID
        /// </summary>
        /// <param name="id">The ID of the employee to unban</param>
        /// <returns>A response indicating whether the unban was successful or not</returns>
        [HttpPut("unban/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Unban(string id) 
        {
            try
            {
                _tokenService.Unban(id);
                return Ok("Account unbanned");
            } catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
            }
        }
    }
}