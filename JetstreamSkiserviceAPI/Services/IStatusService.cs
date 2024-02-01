using JetstreamSkiserviceAPI.DTO;

namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// Interface for Status
    /// </summary>
    /// <remarks>
    ///  To get Registrations ordered by Status (Offen/InArbeit/abgeschlossen)
    /// </remarks>
    public interface IStatusService
    {
        /// <summary>
        /// Retrieves a list of all status entries
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<StatusDto>> GetAll();

        /// <summary>
        /// Retrieves a specific status entry based on the status name
        /// </summary>
        /// <param name="statusName">The name of the status to retrieve</param>
        /// <returns></returns>
        Task<StatusDto> GetByStatus(string statusName);
    }
}
