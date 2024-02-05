namespace JetstreamSkiserviceAPI.DTO
{
    /// <summary>
    /// Data Transfer Object representing priority
    /// </summary>
    public class PriorityDto
    {
        public string PriorityId { get; set; }
        public string? PriorityName { get; set; }
        public List<RegistrationDto> Registration { get; set; } = new List<RegistrationDto>();
    }
}