using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JetstreamSkiserviceAPI.Models
{
    public class Service
    {
        /// <summary>
        /// Unique identifier for the service
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("servicename")]
        public string ServiceName { get; set; }

        [BsonElement("registration_id")]
        public List<string> RegistrationId { get; set; }
    }
}
