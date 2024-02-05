using JetstreamSkiserviceAPI.DTO;

namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// Interface for Registrations
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Retrieves all registrations
        /// </summary>
        /// <returns>A collection of all RegistrationDto objects</returns>
        Task<IEnumerable<RegistrationDto>> GetRegistrations();

        /// <summary>
        /// Retrieves a registration by its ID
        /// </summary>
        /// <param name="id">The ID of the registration to retrieve</param>
        /// <returns></returns>
        Task<RegistrationDto> GetRegistrationById(string id);

        /// <summary>
        /// Adds a new registration
        /// </summary>
        /// <param name="registrationDto">The RegistrationDto object containing the registration details to be added</param>
        /// <returns></returns>
        Task<RegistrationDto> AddRegistration(CreateRegistrationDto createRegistrationDto);

        /// <summary>
        /// Updates an existing registration
        /// </summary>
        /// <param name="registrationDto">The RegistrationDto object containing the updated registration details</param>
        /// <returns></returns>
        Task UpdateRegistration(RegistrationDto registrationDto);

        /// <summary>
        /// Deletes a registration by its ID
        /// </summary>
        /// <param name="id">The ID of the registration to be deleted</param>
        /// <returns><returns>
        Task DeleteRegistration(string id);
    }
}
