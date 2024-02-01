namespace JetstreamSkiserviceAPI.DTO
{
    /// <summary>
    /// Data Transfer Object representing employee
    /// </summary>
    public class EmployeeDto
    {
        public string EmployeeId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string GroupName { get; set; }

        public int Attempts { get; set; }
    }
}
