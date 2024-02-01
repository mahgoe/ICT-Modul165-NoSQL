using AutoMapper;
using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JetstreamSkiserviceAPI.Services
{
    /// <summary>
    /// The RegistrationService class provides methods to get and set data from the heart of the database, implementing from IRegistrationService
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private readonly IMongoCollection<Registration> _registrations;
        private readonly IMongoCollection<Status> _status;
        private readonly IMongoCollection<Priority> _priorities;
        private readonly IMongoCollection<Service> _services;
        private readonly IMapper _mapper;


        /// <summary>
        /// Initializes a new instance of the RegistrationService class with the specified database context
        /// </summary>
        /// <param name="context">The database context to be used for data operations</param>
        public RegistrationService(IMongoDatabase database, IMapper mapper)
        {
            _registrations = database.GetCollection<Registration>("registrations");
            _status = database.GetCollection<Status>("status");
            _priorities = database.GetCollection<Priority>("priority");
            _services = database.GetCollection<Service>("services");
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all registrations from the database and returns them as a list of RegistrationDto objects
        /// </summary>
        public async Task<IEnumerable<RegistrationDto>> GetRegistrations()
        {
            var registrations = await _registrations.Find(reg => true).ToListAsync();

            // Dieser Schritt ist notwendig, um die Referenzen manuell aufzulösen
            var registrationDtos = registrations.Select(async reg =>
            {
                var status = await _status.Find(s => s.Id == reg.StatusId).FirstOrDefaultAsync();
                var priority = await _priorities.Find(p => p.Id == reg.PriorityId).FirstOrDefaultAsync();
                var service = await _services.Find(serv => serv.Id == reg.ServiceId).FirstOrDefaultAsync();

                var regDto = _mapper.Map<RegistrationDto>(reg);
                regDto.Status = status?.StatusName;
                regDto.Priority = priority?.PriorityName;
                regDto.Service = service?.ServiceName;

                return regDto;
            }).ToList();

            // Da die obige Selektion asynchrone Operationen enthält, müssen Sie auf die Ergebnisse warten
            return await Task.WhenAll(registrationDtos);
        }


        /// <summary>
        /// Retrieves a single registration by its ID and returns it as a RegistrationDto object.
        /// </summary>
        /// <param name="id">The ID of the registration to retrieve.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Referenced ID or Item not found or doesn't exist</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the process</exception>
        public async Task<RegistrationDto> GetRegistrationById(string id)
        {
            var registration = await _registrations.Find(reg => reg.Id == id).FirstOrDefaultAsync();
            if (registration == null)
            {
                throw new KeyNotFoundException("Referenced ID or Item not found or doesn't exist");
            }

            // Manuelles Auflösen der Referenzen wie oben
            var status = await _status.Find(s => s.Id == registration.StatusId).FirstOrDefaultAsync();
            var priority = await _priorities.Find(p => p.Id == registration.PriorityId).FirstOrDefaultAsync();
            var service = await _services.Find(serv => serv.Id == registration.ServiceId).FirstOrDefaultAsync();

            var registrationDto = _mapper.Map<RegistrationDto>(registration);
            registrationDto.Status = status?.StatusName;
            registrationDto.Priority = priority?.PriorityName;
            registrationDto.Service = service?.ServiceName;

            return registrationDto;
        }


        /// <summary>
        /// Adds a new registration to the database based on the provided RegistrationDto object
        /// </summary>
        /// <param name="registrationDto">The RegistrationDto object containing the registration details</param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the process</exception>
        public async Task<RegistrationDto> AddRegistration(RegistrationDto registrationDto)
        {
            var registration = _mapper.Map<Registration>(registrationDto);

            // Find Status ID by Name
            var status = await _status.Find(s => s.StatusName == registrationDto.Status).FirstOrDefaultAsync();
            registration.StatusId = status?.Id; // Beachten Sie, dass StatusId jetzt ein String ist, da MongoDB ObjectIds verwendet.

            // Find Priority ID by Name
            var priority = await _priorities.Find(p => p.PriorityName == registrationDto.Priority).FirstOrDefaultAsync();
            registration.PriorityId = priority?.Id;

            // Find Service ID by Name
            var service = await _services.Find(s => s.ServiceName == registrationDto.Service).FirstOrDefaultAsync();
            registration.ServiceId = service?.Id;

            await _registrations.InsertOneAsync(registration);
            return _mapper.Map<RegistrationDto>(registration);
        }

        /// <summary>
        /// Updates an existing registration in the database based on the provided RegistrationDto object.
        /// </summary>
        /// <param name="registrationDto">The RegistrationDto object containing the updated registration details</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Referenced ID or Item not found or doesn't exist</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the process</exception>
        public async Task UpdateRegistration(RegistrationDto registrationDto)
        {
            var status = await _status.Find(s => s.StatusName == registrationDto.Status).FirstOrDefaultAsync();
            if (status == null)
            {
                throw new ArgumentException("The status was not found.");
            }

            var priority = await _priorities.Find(s => s.PriorityName == registrationDto.Priority).FirstOrDefaultAsync();
            if (priority == null)
            {
                throw new ArgumentException("The priority was not found.");
            }

            var service = await _services.Find(s => s.ServiceName == registrationDto.Service).FirstOrDefaultAsync();
            if (service == null)
            {
                throw new ArgumentException("The service was not found.");
            }


            var filter = Builders<Registration>.Filter.Eq(r => r.Id, registrationDto.Id);
            var update = Builders<Registration>.Update
                .Set(r => r.FirstName, registrationDto.FirstName)
                .Set(r => r.LastName, registrationDto.LastName)
                .Set(r => r.Email, registrationDto.Email)
                .Set(r => r.Phone, registrationDto.Phone)
                .Set(r => r.CreateDate, registrationDto.Create_date)
                .Set(r => r.PickupDate, registrationDto.Pickup_date)
                .Set(r => r.StatusId, status.Id)
                .Set(r => r.PriorityId, priority.Id)
                .Set(r => r.ServiceId, service.Id)
                .Set(r => r.Price, registrationDto.Price)
                .Set(r => r.Comment, registrationDto.Comment);

            await _registrations.FindOneAndUpdateAsync(filter, update);
        }

        public async Task DeleteRegistration(string id)
        {
            var filter = Builders<Registration>.Filter.Eq(r => r.Id, id);
            await _registrations.DeleteOneAsync(filter);
        }
    }
}