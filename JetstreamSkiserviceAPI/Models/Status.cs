using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JetstreamSkiserviceAPI.Models
{
    /// <summary>
    /// Status Class to create Database (Code first) - Database connection
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Unique identifier for the service
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("statusname")]
        public string StatusName { get; set; }
    }
}
