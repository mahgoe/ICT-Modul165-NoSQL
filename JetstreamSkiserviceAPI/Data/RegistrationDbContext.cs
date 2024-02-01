using MongoDB.Driver;
using JetstreamSkiserviceAPI.Models;

namespace JetstreamSkiserviceAPI.Data
{
    public class RegistrationDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<RegistrationDbContext> _logger;

        public RegistrationDbContext(IConfiguration configuration, ILogger<RegistrationDbContext> logger)
        {
            _logger = logger;

            try
            {
                var connectionString = configuration.GetConnectionString("JetstreamSkiserviceNoSQL");
                var mongoUrl = new MongoUrl(connectionString);
                var client = new MongoClient(mongoUrl);
                _database = client.GetDatabase(mongoUrl.DatabaseName);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Something went wrong with the connection to the MongoDB");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected failure came up");
                throw;
            }
        }
        public IMongoCollection<Registration> Registrations => _database.GetCollection<Registration>("registrations");
        public IMongoCollection<Priority> Priority => _database.GetCollection<Priority>("priority");
        public IMongoCollection<Service> Services => _database.GetCollection<Service>("services");
        public IMongoCollection<Status> Status => _database.GetCollection<Status>("status");
        public IMongoCollection<Employee> Employees => _database.GetCollection<Employee>("employees");
    }
}
