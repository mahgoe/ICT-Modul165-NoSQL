using JetstreamSkiserviceAPI.Models;
using MongoDB.Driver;

namespace JetstreamSkiserviceAPI.Data
{
    public static class DatabaseHelper
    {
        private static IMongoCollection<Status> _statusCollection;
        private static IMongoCollection<Priority> _priorityCollection;
        private static IMongoCollection<Service> _serviceCollection;

        public static void Initialize(IMongoDatabase database)
        {
            _statusCollection = database.GetCollection<Status>("status");
            _priorityCollection = database.GetCollection<Priority>("priority");
            _serviceCollection = database.GetCollection<Service>("services");
        }

        public static async Task<string> GetStatusIdByNameAsync(string statusName)
        {
            var status = await _statusCollection.Find(s => s.StatusName == statusName).FirstOrDefaultAsync();
            return status?.Id;
        }

        public static async Task<string> GetPriorityIdByNameAsync(string priorityName)
        {
            var priority = await _priorityCollection.Find(p => p.PriorityName == priorityName).FirstOrDefaultAsync();
            return priority?.Id;
        }

        public static async Task<string> GetServiceIdByNameAsync(string serviceName)
        {
            var service = await _serviceCollection.Find(s => s.ServiceName == serviceName).FirstOrDefaultAsync();
            return service?.Id;
        }
    }
}
