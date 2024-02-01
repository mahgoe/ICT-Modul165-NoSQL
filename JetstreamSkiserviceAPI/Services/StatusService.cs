using AutoMapper;
using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Models;
using MongoDB.Driver;

namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// The StatusService class provides methods to interact with status data in the database, implementing from IStatusService
    /// </summary>
    public class StatusService : IStatusService
    {
        private readonly IMongoCollection<Status> _statusCollection;
        private readonly IMongoCollection<Registration> _registrationCollection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for the StatusService class
        /// </summary>
        /// <param name="database">The MongoDB database connection</param>
        public StatusService(IMongoDatabase database, IMapper mapper)
        {
            _statusCollection = database.GetCollection<Status>("Status");
            _registrationCollection = database.GetCollection<Registration>("Registrations");
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all statuses along with their associated registrations from MongoDB
        /// </summary>
        /// <returns>A collection of registrations ordered by status</returns>
        public async Task<IEnumerable<StatusDto>> GetAll()
        {
            var statuses = await _statusCollection.Find(_ => true).ToListAsync();
            var statusDtos = new List<StatusDto>();
            foreach (var status in statuses)
            {
                var registrations = await _registrationCollection.Find(r => r.StatusId == status.Id).ToListAsync();
                var statusDto = _mapper.Map<StatusDto>(status);
                statusDto.Registration = _mapper.Map<List<RegistrationDto>>(registrations);
                statusDtos.Add(statusDto);
            }

            return statusDtos;
        }


        /// <summary>
        /// Retrieves a single status by its name along with its associated registrations
        /// </summary>
        /// <param name="statusName">The name of the status to retrieve (Offen/InArbeit/abgeschlossen)</param>
        /// <returns>A collection of Registrations with the associated status</returns>
        public async Task<StatusDto> GetByStatus(string statusName)
        {
            var status = await _statusCollection.Find(s => s.StatusName == statusName).FirstOrDefaultAsync();
            if (status == null) return null;

            var registrations = await _registrationCollection.Find(r => r.StatusId == status.Id).ToListAsync();
            var statusDto = _mapper.Map<StatusDto>(status);
            statusDto.Registration = _mapper.Map<List<RegistrationDto>>(registrations);

            return statusDto;
        }
    }
}
