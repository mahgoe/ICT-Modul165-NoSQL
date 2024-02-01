using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JetstreamSkiserviceAPI.Models
{
    /// <summary>
    /// Employee Class to create Database (Code first) - Database connection
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Unique identifier for Employee
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Username of the employee
        /// </summary>
        [BsonElement("username")]
        public string Username { get; set; }

        /// <summary>
        /// Password of the employee
        /// </summary>
        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("groupname")]
        public string GroupName { get; set; }

        /// <summary>
        /// Attempts the employee needed - account gets banned after 3 false login attempts
        /// </summary>
        [BsonElement("attempts")]
        public int Attempts { get; set; }
    }
}
