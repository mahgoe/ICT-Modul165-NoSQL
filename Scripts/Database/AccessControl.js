DbAdmin = db.getSiblingDB("admin");
BackendAccess = db.getSiblingDB("JetstreamDB");

// Admin Access
if (!adminDatabase.getUser("admin")) {
  adminDatabase.createUser({
    user: "admin",
    pwd: "password",
    roles: ["root"],
  });
}

// Second User Access
if (!BackendAccess.getUser("Lukas")) {
  BackendAccess.createUser({
    user: "Lukas",
    pwd: "password",
    roles: ["customRole"],
  });
}

// WebAPI
if (!BackendAccess.getRole("customRole")) {
  BackendAccess.createRole({
    role: "customRole",
    privileges: [
      {
        resource: { db: "JetstreamDB", collection: "" },
        actions: ["find", "insert", "update", "remove"],
      },
    ],
    roles: [],
  });
}
