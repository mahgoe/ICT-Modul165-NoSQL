using JetstreamSkiserviceAPI.DTO;

namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// Interface for order by priorities
    /// </summary>
    public interface IPriorityService
    {
        /// <summary>
        /// Retrieves all registrations ordered by priorities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PriorityDto>> GetAll();

        /// <summary>
        /// Retrieves all registrations in a specific priority
        /// </summary>
        /// <param name="priorityName"></param>
        /// <returns></returns>
        Task<PriorityDto> GetByPriority(string priorityName);
    }
}
