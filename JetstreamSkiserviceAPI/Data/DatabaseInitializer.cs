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
