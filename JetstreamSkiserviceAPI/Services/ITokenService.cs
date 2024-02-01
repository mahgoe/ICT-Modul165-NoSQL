using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Models;

namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// Interface for Authentification Token
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates an authentication token for a given username
        /// </summary>
        /// <param name="username">The username for which the token is created</param>
        /// <returns>A string representing the authentication token</returns>
        string CreateToken(string username);

        /// <summary>
        /// Handles the login process
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeDto>> GetEmployees();

        /// <summary>
        /// Records a login attempt for a specified employee
        /// </summary>
        /// <param name="employeeId">The ID of the employee attempting to log in</param>
        Task Attempts(string employeeId);

        /// <summary>
        /// Removes a ban from an employee, allowing them to log in again
        /// </summary>
        /// <param name="employeeId">The ID of the employee to be unbanned</param>
        Task Unban(string employeeId);
    }
}
