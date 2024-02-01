namespace JetstreamSkiserviceAPI.DTO
{
    /// <summary>
    /// Data Transfer Object representing a registration
    /// </summary>
    public class RegistrationDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime Create_date { get; set; }

        public DateTime Pickup_date { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Service { get; set; }

        public string Price { get; set; }

        public string Comment { get; set; }
    }
}
