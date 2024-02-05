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
            await CreateIndex();
            await SeedData();
        }

        public async Task CreateIndex()
        {
            var indexKeys = Builders<Registration>.IndexKeys.Ascending(reg => reg.Email);
            await _database.GetCollection<Registration>("registrations").Indexes.CreateOneAsync(new CreateIndexModel<Registration>(indexKeys));
        }

        public async Task SeedData()
        {
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
                // Prioritys
                var priorityTiefId = await _database.GetCollection<Priority>("priority")
                    .Find(p => p.PriorityName == "Tief")
                    .Project(p => p.Id)
                    .FirstOrDefaultAsync();

                var priorityStandardId = await _database.GetCollection<Priority>("priority")
                    .Find(p => p.PriorityName == "Standard")
                    .Project(p => p.Id)
                    .FirstOrDefaultAsync();

                var priorityExpressId = await _database.GetCollection<Priority>("priority")
                    .Find(p => p.PriorityName == "Express")
                    .Project(p => p.Id)
                    .FirstOrDefaultAsync();

                // Status
                var statusOffenId = await _database.GetCollection<Status>("status")
                    .Find(s => s.StatusName == "Offen")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                var statusInArbeitId = await _database.GetCollection<Status>("status")
                    .Find(s => s.StatusName == "InArbeit")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                var statusAbgeschlossenId = await _database.GetCollection<Status>("status")
                    .Find(s => s.StatusName == "abgeschlossen")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                var statusStorniertId = await _database.GetCollection<Status>("status")
                    .Find(s => s.StatusName == "storniert")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                // Service
                var serviceKleinId = await _database.GetCollection<Service>("services")
                    .Find(s => s.ServiceName == "Kleiner Service")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                var serviceGrossId = await _database.GetCollection<Service>("services")
                    .Find(s => s.ServiceName == "Grosser Service")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

                var serviceRennId = await _database.GetCollection<Service>("services")
                    .Find(s => s.ServiceName == "Rennski Service")
                    .Project(s => s.Id)
                    .FirstOrDefaultAsync();

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
                    Price = 188,
                    Comment = "Testkommentar"
                },
                new Registration
                {
                    FirstName = "Tim",
                    LastName = "Musterfrau",
                    Email = "tim@musterfrau.com",
                    Phone = "0791234564",
                    CreateDate = DateTime.UtcNow,
                    PickupDate = DateTime.UtcNow.AddDays(4),
                    PriorityId = priorityTiefId,
                    StatusId = statusInArbeitId,
                    ServiceId = serviceGrossId,
                    Price = 200,
                    Comment = "Testkommentar"
                },
                new Registration
                {
                    FirstName = "Jenny",
                    LastName = "Klein",
                    Email = "jenny@gmail.com",
                    Phone = "0791334534",
                    CreateDate = DateTime.UtcNow,
                    PickupDate = DateTime.UtcNow.AddDays(5),
                    PriorityId = priorityExpressId,
                    StatusId = statusAbgeschlossenId,
                    ServiceId = serviceRennId,
                    Price = 203,
                    Comment = "Testkommentar"
                },
                new Registration
                {
                    FirstName = "Chris",
                    LastName = "Gross",
                    Email = "c.gross@gmx.com",
                    Phone = "079133341",
                    CreateDate = DateTime.UtcNow,
                    PickupDate = DateTime.UtcNow.AddDays(1),
                    PriorityId = priorityExpressId,
                    StatusId = statusStorniertId,
                    ServiceId = serviceRennId,
                    Price = 345,
                    Comment = "Testkommentar"
                }
            };

                await _database.GetCollection<Registration>("registrations").InsertManyAsync(registrations);
            } 
        }
    }
}
