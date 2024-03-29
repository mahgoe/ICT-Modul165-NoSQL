﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace JetstreamSkiserviceAPI.Models
{
    public class Registration
    {
        /// <summary>
        /// Unique identifier for the registration
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; }

        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("createdate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("pickupdate")]
        public DateTime PickupDate { get; set; }

        // References to other Documents
        [BsonElement("status_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StatusId { get; set; }

        [BsonElement("priority_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PriorityId { get; set; }

        [BsonElement("service_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ServiceId { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }
    }
}
