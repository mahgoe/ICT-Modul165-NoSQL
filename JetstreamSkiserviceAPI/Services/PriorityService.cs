using AutoMapper;
using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Models;
using MongoDB.Driver;


namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// The PriorityService class provides methods to interact with priority data in the database, implementing from IPriorityService
    /// </summary>
    public class PriorityService : IPriorityService
    {
        private readonly IMongoCollection<Priority> _priorityCollection;
        private readonly IMongoCollection<Registration> _registrationCollection;
        private readonly IMongoCollection<Status> _statusCollection;
        private readonly IMongoCollection<Service> _serviceCollection;
        private readonly IMapper _mapper;


        /// <summary>
        /// Constructor for the PriorityService class
        /// </summary>
        public PriorityService(IMongoDatabase database, IMapper mapper)
        {
            _priorityCollection = database.GetCollection<Priority>("Priority");
            _registrationCollection = database.GetCollection<Registration>("Registrations");
            _statusCollection = database.GetCollection<Status>("Status");
            _serviceCollection = database.GetCollection<Service>("Services");
            _mapper = mapper;
        }


        /// <summary>
        /// Retrieves all priorities along with their associated registrations from the database
        /// </summary>
        /// <returns>A collection of registrations ordered by priorities</returns>
        public async Task<IEnumerable<PriorityDto>> GetAll()
        {
            var priorities = await _priorityCollection.Find(_ => true).ToListAsync();
            var priorityDtos = new List<PriorityDto>();
            foreach (var priority in priorities)
            {
                var registrations = await _registrationCollection.Find(r => r.PriorityId == priority.Id).ToListAsync();
                var priorityDto = _mapper.Map<PriorityDto>(priority);
                priorityDto.Registration = _mapper.Map<List<RegistrationDto>>(registrations);
                priorityDtos.Add(priorityDto);
            }

            return priorityDtos;
        }

        /// <summary>
        /// Retrieves a single priority by its name along with its associated registrations
        /// </summary>
        /// <param name="priorityName">The name of the priority to retrieve (Tief/Standard/Express)</param>
        /// <returns>A collection of registrations with the associated priority</returns>
        public async Task<PriorityDto> GetByPriority(string priorityName)
        {
            var priority = await _priorityCollection.Find(p => p.PriorityName == priorityName).FirstOrDefaultAsync();

            if (priority == null) return null;

            var registrations = await _registrationCollection.Find(r => r.PriorityId == priority.Id).ToListAsync();

            var registrationDtos = new List<RegistrationDto>();

            foreach (var registration in registrations)
            {
                var status = await _statusCollection.Find(s => s.Id == registration.StatusId).FirstOrDefaultAsync();
                var service = await _serviceCollection.Find(s => s.Id == registration.ServiceId).FirstOrDefaultAsync();

                var registrationDto = _mapper.Map<RegistrationDto>(registration);
                registrationDto.Status = status?.StatusName;
                registrationDto.Service = service?.ServiceName;

                registrationDtos.Add(registrationDto);
            }

            var priorityDto = new PriorityDto
            {
                PriorityId = priority.Id,
                PriorityName = priority.PriorityName,
                Registration = registrationDtos
            };

            return priorityDto;
        }
    }
}
