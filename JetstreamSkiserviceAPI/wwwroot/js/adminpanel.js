function formatDateTime(dateTimeString) {
  const date = new Date(dateTimeString);
  return date.toLocaleDateString("de-CH", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
}

function fetchData() {
  fetch("/Registrations")
    .then((response) => response.json())
    .then((data) => {
      const tableHeaders = Object.keys(data[0])
        .map((key) => {
          return `<th class="px-5 py-3 border-b-2 border-gray-200 bg-gray-100 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">${key}</th>`;
        })
        .join("");

      const tableRows = data
        .map((row) => {
          const formattedCreateDate = formatDateTime(row.create_date);
          const formattedPickupDate = formatDateTime(row.pickup_date);

          const rowData = Object.entries(row)
            .map(([key, value]) => {
              if (key === "create_date") {
                return `<td class="px-5 py-5 border-b border-gray-200 bg-white text-sm" data-key="${key}">${formattedCreateDate}</td>`;
              } else if (key === "pickup_date") {
                return `<td class="px-5 py-5 border-b border-gray-200 bg-white text-sm" data-key="${key}">${formattedPickupDate}</td>`;
              } else {
                return `<td class="px-5 py-5 border-b border-gray-200 bg-white text-sm" data-key="${key}">${value}</td>`;
              }
            })
            .join("");

          return `
          <tr id="row-${row.registrationId}">
            ${rowData}
            <td class="px-5 py-5 border-b border-gray-200 bg-white text-sm">
              <button onclick="editEntry(${row.registrationId})" class="edit-button border border-gray-300 shadow-sm rounded px-2 py-1 m-1">Ändern</button>
              <button onclick="saveEntry(${row.registrationId})" class="save-button hidden border border-gray-300 shadow-sm rounded px-2 py-1 m-1">Speichern</button>
              <button onclick="deleteEntry(${row.registrationId})" class="delete-button border border-gray-300 shadow-sm rounded px-2 py-1 m-1">Löschen</button>
            </td>
          </tr>
        `;
        })
        .join("");

      document.getElementById("output").innerHTML = `
        <table class="min-w-full leading-normal">
          <thead>
            <tr>${tableHeaders}<th class="px-5 py-3 border-b-2 border-gray-200 bg-gray-100 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Aktionen</th></tr>
          </thead>
          <tbody>
            ${tableRows}
          </tbody>
        </table>
      `;
    })
    .catch((error) => console.error("Fetch Fehler:", error));
}

function fetchOrdersByStatus(status) {
  fetch(`/Status/${status}`)
    .then((response) => response.json())
    .then((data) => {
      updateTableWithOrders(data);
    })
    .catch((error) => console.error(`Fetch Fehler bei ${status}:`, error));
}

function fetchOrdersByPriority(priority) {
  fetch(`/Priority/${priority}`)
    .then((response) => response.json())
    .then((data) => {
      updateTableWithOrders(data);
    })
    .catch((error) =>
      console.error(`Fetch Fehler bei Priorität ${priority}:`, error)
    );
}
function updateTableWithOrders(data) {
  if (!data || !data.registration || data.registration.length === 0) {
    document.getElementById("output").innerHTML =
      "<p>Keine Daten gefunden.</p>";
    return;
  }

  const registrations = data.registration;

  const tableHeaders = Object.keys(registrations[0])
    .map(
      (key) =>
        `<th class="px-5 py-3 border-b-2 border-gray-200 bg-gray-100 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">${key}</th>`
    )
    .concat(
      '<th class="px-5 py-3 border-b-2 border-gray-200 bg-gray-100 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">Aktionen</th>'
    )
    .join("");

  const tableRows = registrations
    .map((registration) => {
      const formattedCreateDate = formatDateTime(registration.create_date);
      const formattedPickupDate = formatDateTime(registration.pickup_date);

      const rowData = Object.entries(registration)
        .map(([key, value]) => {
          if (key === "create_date") {
            return `<td class="px-5 py-5 border-b border-gray-200 bg-white text-sm" data-key="${key}">${formattedCreateDate}</td>`;
          } else if (key === "pickup_date") {
            return `<td class="px-5 py-5 border-b border-gray-200 bg-white text-sm" data-key="${key}">${formattedPickupDate}</td>`;
          } else {
            return `<td class="px-5 py-5 border-b border-gray-200 bg-white text-sm" data-key="${key}">${value}</td>`;
          }
        })
        .join("");

      const actionButtons = `
      <td class="px-5 py-5 border-b border-gray-200 bg-white text-sm">
        <button onclick="editEntry(${registration.registrationId})" class="edit-button border border-gray-300 shadow-sm rounded px-2 py-1 m-1">Ändern</button>
        <button onclick="saveEntry(${registration.registrationId})" class="save-button hidden border border-gray-300 shadow-sm rounded px-2 py-1 m-1">Speichern</button>
        <button onclick="deleteEntry(${registration.registrationId})" class="delete-button border border-gray-300 shadow-sm rounded px-2 py-1 m-1">Löschen</button>
      </td>`;

      return `<tr id="row-${registration.registrationId}">${rowData}${actionButtons}</tr>`;
    })
    .join("");

  document.getElementById("output").innerHTML = `
    <table class="min-w-full leading-normal">
      <thead>
        <tr>${tableHeaders}</tr>
      </thead>
      <tbody>
        ${tableRows}
      </tbody>
    </table>
  `;
}

function editEntry(registrationId) {
  const row = document.getElementById(`row-${registrationId}`);
  const cells = row.querySelectorAll("td");

  cells.forEach((cell) => {
    const key = cell.getAttribute("data-key");
    let value = cell.textContent.trim();

    if (key === "status") {
      cell.innerHTML = createStatusDropdown(value);
    } else if (key === "priority") {
      cell.innerHTML = createPriorityDropdown(value);
    } else if (key === "service") {
      cell.innerHTML = createServiceDropdown(value);
    } else if (key) {
      cell.contentEditable = true;
    }
  });

  row.querySelector(".edit-button").classList.add("hidden");
  row.querySelector(".save-button").classList.remove("hidden");
}

function getToken() {
  return localStorage.getItem("token");
}

function formatDate(dateString) {
  const parts = dateString.split(".");

  const newDate = new Date(parts[2], parts[1] - 1, parts[0]);

  const now = new Date();

  if (!isNaN(newDate)) {
    return (
      newDate.getFullYear() +
      "-" +
      String(newDate.getMonth() + 1).padStart(2, "0") +
      "-" +
      String(newDate.getDate()).padStart(2, "0") +
      "T" +
      String(now.getHours()).padStart(2, "0") +
      ":" +
      String(now.getMinutes()).padStart(2, "0") +
      ":" +
      String(now.getSeconds()).padStart(2, "0") +
      "." +
      String(now.getMilliseconds()).padStart(3, "0")
    );
  } else {
    return "";
  }
}

function saveEntry(id) {
  const token = getToken();
  const row = document.getElementById(`row-${id}`);
  const cells = row.querySelectorAll("td");
  const data = { registrationId: id };

  cells.forEach((cell) => {
    const key = cell.getAttribute("data-key");
    if (!key) return;

    let value;

    if (cell.querySelector("select")) {
      value = cell.querySelector("select").value;
    } else if (cell.isContentEditable) {
      value = cell.textContent.trim();
      if (key === "create_date" || key === "pickup_date") {
        value = formatDate(value);
      }
    } else {
      value = cell.innerText.trim();
    }

    data[key] = value;
  });

  fetch(`/Registrations/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  })
    .then((response) => {
      if (!response.ok) {
        alert(`Fehler: ${response.statusText}`);
        throw new Error(`HTTP Fehler: ${response.status}`);
      }
      return response.json();
    })
    .then((updatedEntry) => {
      alert("Eintrag erfolgreich gespeichert.");
      row.querySelector(".edit-button").classList.remove("hidden");
      row.querySelector(".save-button").classList.add("hidden");
      Array.from(cells).forEach((cell) => {
        cell.contentEditable = false;
      });
      fetchData();
    })
    .catch((error) => {
      alert(`Fehler beim Speichern: ${error.message}`);
    });
}

function deleteEntry(id) {
  const token = getToken();
  const row = document.getElementById(`row-${id}`);
  const cells = row.querySelectorAll("td");
  const data = { registrationId: id };

  cells.forEach((cell) => {
    const key = cell.getAttribute("data-key");
    if (!key) return;

    let value = cell.innerText.trim();

    if (cell.querySelector("select")) {
      value = cell.querySelector("select").value;
    } else if (cell.isContentEditable) {
      value = cell.textContent.trim();
    }

    if (key === "create_date" || key === "pickup_date") {
      value = formatDate(value);
    }

    data[key] = value;
  });

  data.status = "storniert"; // Status explizit auf "storniert" setzen

  fetch(`/Registrations/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP Fehler: ${response.status}`);
      }
      return response.json();
    })
    .then(() => {
      alert("Eintrag erfolgreich storniert.");
      fetchData(); // Neuladen der Daten
    })
    .catch((error) => {
      alert(`Fehler beim Aktualisieren des Status: ${error.message}`);
    });
}

function createStatusDropdown(selectedValue) {
  const options = ["Offen", "InArbeit", "abgeschlossen", "storniert"]
    .map(
      (option) =>
        `<option value="${option}" ${
          option === selectedValue ? "selected" : ""
        }>${option}</option>`
    )
    .join("");
  return `<select class="status-dropdown">${options}</select>`;
}

function createPriorityDropdown(selectedValue) {
  const options = ["Tief", "Standard", "Express"]
    .map(
      (option) =>
        `<option value="${option}" ${
          option === selectedValue ? "selected" : ""
        }>${option}</option>`
    )
    .join("");
  return `<select class="priority-dropdown">${options}</select>`;
}

function createServiceDropdown(selectedValue) {
  const services = [
    "Kleiner Service",
    "Grosser Service",
    "Rennski Service",
    "Bindung montieren und einstellen",
    "Fell zuschneiden",
    "Heisswachsen",
  ];
  const options = services
    .map(
      (service) =>
        `<option value="${service}" ${
          service === selectedValue ? "selected" : ""
        }>${service}</option>`
    )
    .join("");
  return `<select class="service-dropdown">${options}</select>`;
}

function showSuccessModal() {
  document.getElementById("successModal").classList.remove("hidden");
  setTimeout(() => {
    document.getElementById("successModal").classList.add("hidden");
  }, 3000);
}

function showErrorModal(message) {
  document.getElementById("errorMessage").textContent = message;
  document.getElementById("errorModal").classList.remove("hidden");
  setTimeout(() => {
    document.getElementById("errorModal").classList.add("hidden");
  }, 3000);
}

document.addEventListener("DOMContentLoaded", () => {
  document.getElementById("allOrdersBtn").addEventListener("click", fetchData);
  document
    .getElementById("openOrdersBtn")
    .addEventListener("click", () => fetchOrdersByStatus("Offen"));
  document
    .getElementById("inWorkOrdersBtn")
    .addEventListener("click", () => fetchOrdersByStatus("InArbeit"));
  document
    .getElementById("doneOrdersBtn")
    .addEventListener("click", () => fetchOrdersByStatus("abgeschlossen"));
  document
    .getElementById("tiefOrdersBtn")
    .addEventListener("click", () => fetchOrdersByPriority("Tief"));
  document
    .getElementById("standardOrdersBtn")
    .addEventListener("click", () => fetchOrdersByPriority("Standard"));
  document
    .getElementById("expressOrdersBtn")
    .addEventListener("click", () => fetchOrdersByPriority("Express"));
});
