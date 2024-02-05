// Script to create indexes for the JetstreamDB database
db = db.getSiblingDB("JetstreamDB");

// Index for registrations
db.registrations.createIndex({ firstname: 1 }, { name: "firstname_index" });
db.registrations.createIndex({ lastname: 1 }, { name: "lastname_index" });
db.registrations.createIndex({ email: 1 }, { name: "email_index" });
db.registrations.createIndex({ pickup_date: 1 }, { name: "pickupdate_index" });

// Index for services
db.services.createIndex({ servicename: 1 }, { name: "servicename_index" });

// Index for status
db.status.createIndex({ statusname: 1 }, { name: "statusname_index" });

// Index for priority
db.priority.createIndex({ priorityname: 1 }, { name: "priorityname_index" });
