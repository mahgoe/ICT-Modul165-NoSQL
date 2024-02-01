using JetstreamSkiserviceAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JetstreamSkiserviceAPI.Data
{
    public class DatabaseInitializer
    {
        private readonly IMongoDatabase _database;

        public DatabaseInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitalizeAsync()
        {
            await ApplySchemaValidation("registrations", CreateRegistrationSchema());
            await ApplySchemaValidation("status", CreateStatusSchema());
            await ApplySchemaValidation("priority", CreatePrioritySchema());
            await ApplySchemaValidation("services", CreateServiceSchema());
            await CreateIndex();
            await SeedData();
        }

        private BsonDocument CreateRegistrationSchema()
        {
            return new BsonDocument("$jsonSchema", new BsonDocument
            {
                { "bsonType", "object" },
                { "required", new BsonArray { "firstName", "lastName", "email" } },
                { "properties", new BsonDocument
                    {
                        { "firstName", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "description", "must be a string and is required" }
                            }
                        },
                        { "lastName", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "description", "must be a string and is required" }
                            }
                        },
                        { "email", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "description", "must be a string and is required" }
                            }
                        },
                        { "phone", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "description", "must be a string" }
                            }
                        },
                        { "createdate", new BsonDocument
                            {
                                { "bsonType", "date" },
                                { "description", "must be a date and is UTC" }
                            }
                        },
                        { "pickupdate", new BsonDocument
                            {
                                { "bsonType", "date" },
                                { "description", "must be a date and is UTC" }
                            }
                        },
                        { "status_id", new BsonDocument
                            {
                                { "bsonType", "objectId" },
                                { "description", "must be an objectId referencing Status collection" }
                            }
                        },
                        { "priority_id", new BsonDocument
                            {
                                { "bsonType", "objectId" },
                                { "description", "must be an objectId referencing Priority collection" }
                            }
                        },
                        { "service_id", new BsonDocument
                           {
                               { "bsonType", "objectId" },
                               { "description", "must be an objectId referencing Service collection" }
                           }
                        },
                        { "price", new BsonDocument
                           {
                               { "bsonType", "string" },
                               { "description", "must be an string" }
                           }
                        },
                        { "comment", new BsonDocument
                           {
                               { "bsonType", "string" },
                               { "description", "must be an string" }
                           }
                        },
                    }
                }
            });
        }

        private BsonDocument CreateStatusSchema()
        {
            return new BsonDocument("$jsonSchema", new BsonDocument
            {
                { "bsonType", "object" },
                { "properties", new BsonDocument
                    {
                        { "statusname", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "enum", new BsonArray { "Offen", "InArbeit", "abgeschlossen", "storniert" } },
                                { "description", "can only be one of the enum values" }
                            }
                        }
                    }
                }
            });
        }

        private BsonDocument CreatePrioritySchema()
        {
            return new BsonDocument("$jsonSchema", new BsonDocument
            {
                { "bsonType", "object" },
                { "properties", new BsonDocument
                    {
                        { "priorityname", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "enum", new BsonArray { "Tief", "Standard", "Express" } },
                                { "description", "can only be one of the enum values" }
                            }
                        }
                    }
                }
            });
        }

        private BsonDocument CreateServiceSchema()
        {
            return new BsonDocument("$jsonSchema", new BsonDocument
            {
                { "bsonType", "object" },
                { "properties", new BsonDocument
                    {
                        { "servicename", new BsonDocument
                            {
                                { "bsonType", "string" },
                                { "enum", new BsonArray { "Kleiner Service", "Grosser Service", "Rennski Service", "Bindungen montieren und einstellen", "Heisswachsen" } },
                                { "description", "can only be one of the enum values" }
                            }
                        }
                    }
                }
            });
        }

        private async Task ApplySchemaValidation(string collectionName, BsonDocument schema)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = await _database.ListCollectionNamesAsync(new ListCollectionNamesOptions { Filter = filter });
            var exists = await collections.AnyAsync();

            if (!exists)
            {
                await _database.CreateCollectionAsync(collectionName);
            }

            // Anwenden der Schemavalidierung
            var command = new BsonDocument
            {
                { "collMod", collectionName },
                { "validator", new BsonDocument
                    {
                        { "$jsonSchema", schema }
                    }
                }
            };

            try
            {
                await _database.RunCommandAsync<BsonDocument>(command);
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Fehler beim Anwenden der Schema-Validierung: {ex.Message}");
            }
        }

        public async Task CreateIndex()
        {
            var indexKeys = Builders<Registration>.IndexKeys.Ascending(reg => reg.Email);
            await _database.GetCollection<Registration>("registrations").Indexes.CreateOneAsync(new CreateIndexModel<Registration>(indexKeys));
        }

        public async Task SeedData()
        {
            // Überprüfen Sie zuerst, ob Daten bereits vorhanden sind, um Duplikate zu vermeiden.
            if (!await _database.GetCollection<Priority>("priority").Find(_ => true).AnyAsync())
            {
                var priorities = new List<Priority>
                {
                    new Priority { PriorityName = "Tief" },
                    new Priority { PriorityName = "Standard" },
                    new Priority { PriorityName = "Express" }
                };
                await _database.GetCollection<Priority>("priority").InsertManyAsync(priorities);
            }

            if (!await _database.GetCollection<Service>("services").Find(_ => true).AnyAsync())
            {
                var services = new List<Service>
                {
                    new Service { ServiceName = "Kleiner Service" },
                    new Service { ServiceName = "Grosser Service" },
                    new Service { ServiceName = "Rennski Service" },
                    new Service { ServiceName = "Bindungen montieren und einstellen" },
                    new Service { ServiceName = "Fell zuschneiden" },
                    new Service { ServiceName = "Heisswachsen" }
                };
                await _database.GetCollection<Service>("services").InsertManyAsync(services);
            }

            if (!await _database.GetCollection<Status>("status").Find(_ => true).AnyAsync())
            {
                var statuses = new List<Status>
                {
                    new Status { StatusName = "Offen" },
                    new Status { StatusName = "InArbeit" },
                    new Status { StatusName = "abgeschlossen" },
                    new Status { StatusName = "storniert" }
                };
                await _database.GetCollection<Status>("status").InsertManyAsync(statuses);
            }

            if (!await _database.GetCollection<Employee>("employees").Find(_ => true).AnyAsync())
            {
                var employees = new List<Employee>
                {
                    new Employee { Username = "admin", Password = "password", Attempts = 0 },
            
                };
                await _database.GetCollection<Employee>("employees").InsertManyAsync(employees);
            }
            await InsertRegistrationDataAsync();
        }

        public async Task InsertRegistrationDataAsync()
        {
            var registrationExists = await _database.GetCollection<Registration>("registrations")
            .Find(_ => true)
            .AnyAsync();


            if (!registrationExists)
            {
                // Beispiel: Ermitteln der IDs
                var priorityStandardId = await _database.GetCollection<Priority>("priority")
                    .Find(p => p.PriorityName == "Standard")
                    .Project(p => p.Id)
                    .FirstOrDefaultAsync();

                var statusOffenId = await _database.GetCollection<Status>("status")
                    .Find(s => s.StatusName == "Offen")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                var serviceKleinId = await _database.GetCollection<Service>("services")
                    .Find(s => s.ServiceName == "Kleiner Service")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                // Hier müssten Sie die entsprechenden Abfragen für Service und weitere machen

                // Einfügen der Registration-Daten mit den ermittelten IDs
                var registrations = new List<Registration>
            {
                new Registration
                {
                    FirstName = "Max",
                    LastName = "Muster",
                    Email = "max@mustermann.com",
                    Phone = "0791234567",
                    CreateDate = DateTime.UtcNow,
                    PickupDate = DateTime.UtcNow.AddDays(3),
                    PriorityId = priorityStandardId,
                    StatusId = statusOffenId,
                    ServiceId = serviceKleinId,
                    Price = "199",
                    Comment = "Testkommentar"
                },
            };

                await _database.GetCollection<Registration>("registrations").InsertManyAsync(registrations);
            } 
        }
    }
}
