namespace JetstreamSkiserviceAPI.DTO
{
    public class CreateRegistrationDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime Create_date { get; set; }

        public DateTime Pickup_date { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Service { get; set; }

        public double Price { get; set; }

        public string Comment { get; set; }
    }
}
