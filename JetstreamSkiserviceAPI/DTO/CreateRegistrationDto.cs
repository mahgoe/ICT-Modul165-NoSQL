using System.ComponentModel.DataAnnotations;

namespace JetstreamSkiserviceAPI.DTO
{
    public class CreateRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid E-Mail format.")]
        public string Email { get; set; }

        [RegularExpression(@"^\+?(\d{1,4})?([- .])?((\(\d{1,3}\))|\d{1,3})?([- .])?\d{1,4}([- .])?\d{1,4}([- .])?\d{1,9}$", ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        public DateTime Create_date { get; set; }

        public DateTime Pickup_date { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Service { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public double Price { get; set; }

        public string Comment { get; set; }
    }
}
