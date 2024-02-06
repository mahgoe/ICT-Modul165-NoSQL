using System.ComponentModel.DataAnnotations;

namespace JetstreamSkiserviceAPI.DTO
{
    public class CreateRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [RegularExpression(@"/^((?!.)[w-_.]*[^.])(@w+)(.w+(.w+)?[^.W])$/gm", ErrorMessage = "Invalid E-Mail format.")]
        public string Email { get; set; }

        [RegularExpression(@"/(?:([+]d{1,4})[-.s]?)?(?:[(](d{1,3})[)][-.s]?)?(d{1,4})[-.s]?(d{1,4})[-.s]?(d{1,9})/g", ErrorMessage = "Invalid phone number format.")]
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
