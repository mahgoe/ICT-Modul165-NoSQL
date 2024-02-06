// To run this script in your MongoDB database:
// 1. Open a terminal or command-line interface.
// 2. Connect to your MongoDB instance with the mongo shell command.
// 3. Use the Database: JetstreamDB with `use JetstreamDB` command.
// 3. Run the script by using the `load("/path/to/your/script.js")` command.

// Schema validation for 'registrations' collection
db.createCollection("registrations", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["firstname", "lastname", "email"],
      properties: {
        firstname: {
          bsonType: "string",
          description: "must be a string and is required",
        },
        lastname: {
          bsonType: "string",
          description: "must be a string and is required",
        },
        email: {
          bsonType: "string",
          pattern: "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$",
          description: "must be a string and is required",
        },
        phone: {
          bsonType: "string",
          pattern:
            "^\\+?(\\d{1,4})?([- .])?((\\(\\d{1,3}\\))|\\d{1,3})?([- .])?\\d{1,4}([- .])?\\d{1,4}([- .])?\\d{1,9}$",
          description: "must be a valid phone number",
        },
        createdate: {
          bsonType: "date",
          description: "must be a date in UTC",
        },
        pickupdate: {
          bsonType: "date",
          description: "must be a date in UTC",
        },
        status_id: {
          bsonType: "objectId",
          description: "must be an objectId referencing Status collection",
        },
        priority_id: {
          bsonType: "objectId",
          description: "must be an objectId referencing Priority collection",
        },
        services_id: {
          bsonType: "objectId",
          description: "must be an objectId referencing Services collection",
        },
        price: {
          bsonType: "double",
          description: "must be a double",
        },
        comments: {
          bsonType: "string",
          description: "must be a string",
        },
      },
    },
  },
});

// Schema validation for 'status' collection
db.createCollection("status", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      properties: {
        statusname: {
          bsonType: "string",
          enum: ["Offen", "InArbeit", "abgeschlossen", "storniert"],
          description: "can only be one of the enum values",
        },
      },
    },
  },
});

// Schema validation for 'priority' collection
db.createCollection("priority", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      properties: {
        priorityname: {
          bsonType: "string",
          enum: ["Tief", "Standard", "Express"],
          description: "can only be one of the enum values",
        },
      },
    },
  },
});

// Schema validation for 'services' collection
db.createCollection("services", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      properties: {
        servicename: {
          bsonType: "string",
          enum: [
            "Kleiner Service",
            "Grosser Service",
            "Rennski Service",
            "Bindungen montieren und einstellen",
            "Fell zuschneiden",
            "Heisswachsen",
          ],
          description: "can only be one of the enum values",
        },
      },
    },
  },
});
