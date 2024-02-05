using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JetstreamSkiserviceAPI.Controllers
{
    /// <summary>
    /// Controller for handling HTTP requests related to registrations
    /// <remarks>
    /// If the API gets tested and you request a POST/PUT/DELETE change the strings in the body or you get 500 Internal Server Exception!
    ///  "status" : "Offen/InArbeit/abgeschlossen"
    ///  "priority" : "Tief/Standard/Express"
    ///  "service" : "Kleiner Service/Grosser Service/Rennski Service/Bindungen montieren und einstellen/Fell zuschneiden/Heisswachsen"
    ///  </remarks>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<RegistrationsController> _logger;

        /// <summary>
        /// Initializes a new instance of the RegistrationsController
        /// </summary>
        /// <param name="registration">The registration service to be used by the controller</param>
        /// <param name="logger">The logger to be used for logging information and errors</param>
        public RegistrationsController(IRegistrationService registration, ILogger<RegistrationsController> logger)
        {
            _registrationService = registration;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all registrations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RegistrationDto>>> GetRegistrations()
        {
            try
            {
                var registrations = await _registrationService.GetRegistrations();
                var nonCancelledRegistrations = registrations
                    .Where(r => r.Status?.ToLower() != "storniert");

                return Ok(nonCancelledRegistrations);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error occurred, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        /// <summary>
        /// Retrieves a specific registration by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationDto>> GetRegistration(string id)
        {
            try
            {
                var registrationDto = await _registrationService.GetRegistrationById(id);

                if (registrationDto == null || registrationDto.Status?.ToLower() == "storniert")
                {
                    return NotFound();
                }

                return Ok(registrationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error occurred, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        /// <summary>
        /// Creates a new registration
        /// </summary>
        /// <param name="registrationDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<RegistrationDto>> CreateRegistration([FromBody] CreateRegistrationDto createRegistrationDto)
        {
            try
            {
                var registrationDto = await _registrationService.AddRegistration(createRegistrationDto);
                return CreatedAtAction(nameof(GetRegistration), new { id = registrationDto.Id }, registrationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }


        /// <summary>
        /// Updates an existing registration by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registrationDto">The ID of the registration to update</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRegistration(int id, RegistrationDto registrationDto)
        {
            if (registrationDto == null)
            {
                return BadRequest();
            }

            try
            {
                await _registrationService.UpdateRegistration(registrationDto);
                return Ok(registrationDto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"An error occured, {ex.Message}");
                return NotFound($"No Item found with ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }

        /// <summary>
        /// Deletes a specific registration by ID
        /// </summary>
        /// <param name="id">The ID of the registration to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRegistration(string id)
        {
            try
            {
                await _registrationService.DeleteRegistration(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"An error occured, {ex.Message}");
                return NotFound($"No Item found with ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }
    }
}
