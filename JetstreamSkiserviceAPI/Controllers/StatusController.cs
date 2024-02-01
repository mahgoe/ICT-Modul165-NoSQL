using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JetstreamSkiserviceAPI.Controllers
{
    /// <summary>
    /// Controller for handling HTTP requests related to status
    /// </summary>
    /// <remarks>
    /// Use these statuses (Offen/InArbeit/abgeschlossen)
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly ILogger<StatusController> _logger;

        /// <summary>
        /// Constructor for the StatusController
        /// </summary>
        /// <param name="status">Implementation of the IStatusService interface</param>
        /// <param name="logger">Logger for logging information and errors</param>
        public StatusController(IStatusService status, ILogger<StatusController> logger)
        {
            _statusService = status;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to retrieve all status entities
        /// </summary>
        /// <returns>A list sorted by status</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StatusDto>>> GetAll()
        {
            try
            {
                return Ok(await _statusService.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error occured, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }

        /// <summary>
        /// Handles GET requests to retrieve a specific status entity by its name
        /// </summary>
        /// <param name="statusName">The name of the status to retrieve (Offen/InArbeit/abgeschlossen)</param>
        /// <returns>Registrations by the name of the status to retrieve</returns>
        [HttpGet("{statusName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StatusDto>>> GetByStatus(string statusName)
        {
            var statusDto = await _statusService.GetByStatus(statusName);

            try
            {
                if (statusDto == null)
                {
                    return NotFound();
                }
                return Ok(statusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error occured, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
            }
        }
    }
}
