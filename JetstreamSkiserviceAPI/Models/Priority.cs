using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JetstreamSkiserviceAPI.Models
{
    public class Priority
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("priorityname")]
        public string PriorityName { get; set; }
    }
}
