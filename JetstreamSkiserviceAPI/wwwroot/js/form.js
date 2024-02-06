function showErrorMessage(field, errorMessageId, message) {
  document.getElementById(errorMessageId).innerText = message;
  field.classList.remove("border-gray-300", "dark:border-gray-600");
  field.classList.add("border-red-500");
}

function clearErrorMessage(field, errorMessageId) {
  document.getElementById(errorMessageId).innerText = "";
  field.classList.remove("border-red-500");
  field.classList.add("border-gray-300", "dark:border-gray-600");
}

function validateForm(e) {
  e.preventDefault();

  const firstName = document.getElementById("firstname");
  const lastName = document.getElementById("lastname");
  const email = document.getElementById("email");
  const phone = document.getElementById("phone");

  // Trim für alle Felder um keine Leerzeichen zu haben
  firstName.value = firstName.value.trim();
  lastName.value = lastName.value.trim();
  email.value = email.value.trim();
  phone.value = phone.value.trim();

  // Validation
  let isValid = true;

  if (!validateRequiredField(firstName) || !validateNameFormat(firstName)) {
    isValid = false;
    showErrorMessage(
      firstName,
      "errorMessageFirstName",
      "Bitte einen gültigen Vornamen eingeben"
    );
  } else {
    clearErrorMessage(firstName, "errorMessageFirstName");
  }

  if (!validateRequiredField(lastName) || !validateNameFormat(lastName)) {
    isValid = false;
    showErrorMessage(
      lastName,
      "errorMessageLastName",
      "Bitte einen gültigen Nachnamen eingeben."
    );
  } else {
    clearErrorMessage(lastName, "errorMessageLastName");
  }

  if (!validateRequiredField(email) || !validateEmailFormat(email)) {
    isValid = false;
    showErrorMessage(
      email,
      "errorMessageEmail",
      "Bitte eine gültige E-Mail eingeben."
    );
  } else {
    clearErrorMessage(email, "errorMessageEmail");
  }

  if (!validateRequiredField(phone) || !validatePhoneNumber(phone)) {
    isValid = false;
    showErrorMessage(
      phone,
      "errorMessagePhone",
      "Bitte eine gültige Telefonnummer eingeben"
    );
  } else {
    clearErrorMessage(phone, "errorMessagePhone");
  }

  // Formular abschicken bei erfolgreicher Validierung
  if (isValid) {
    postData(firstName.value, lastName.value, email.value);
    console.log(
      "firstName: " + firstName.value,
      "lastName: " + lastName.value,
      "email: " + email.value
    );
  }
}

// Funktionen für Validierungen und Formularverarbeitung
function validateRequiredField(field) {
  if (field.value === "") {
    field.classList.add("error");
    return false;
  } else {
    field.classList.remove("error");
    return true;
  }
}

// Funktion die nur Buchstaben und Bindestriche erlaubt
function validateNameFormat(field) {
  const namePattern = /^[a-zA-ZäöüÄÖÜ]+([- ][a-zA-ZäöüÄÖÜ]+)*$/;

  if (!namePattern.test(field.value)) {
    return false;
  }
  return true;
}

function validateEmailFormat(field) {
  const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  if (!emailPattern.test(field.value)) {
    return false;
  }
  return true;
}

function validatePhoneNumber(field) {
  const phonePattern =
      /(?:([+]\d{1,4})[-.\s]?)?(?:[(](\d{1,3})[)][-.\s]?)?(\d{1,4})[-.\s]?(\d{1,4})[-.\s]?(\d{1,9})/g;
  if (!phonePattern.test(field.value)) {
    return false;
  }
  return true;
}
function postData(firstName, lastName, email) {
  fetch("/Registrations", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      firstName: firstName,
      lastName: lastName,
      email: email,
      phone: document.getElementById("phone").value,
      priority: document.querySelector('input[name="list-radio"]:checked')
        .value,
      service: document.getElementById("serviceDropdown").value,
      create_date: document.getElementById("startDate").value, // Datum direkt übernehmen
      pickup_date: document.getElementById("endDate").value, // Datum direkt übernehmen
      status: "Offen",
      service: document.getElementById("serviceDropdown").value,
      price: document.getElementById("total").value,
      comment: "",
    }),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      window.location.href = "formconfirm.html";
    })
    .catch((error) => {
      console.error(
        "There has been a problem with your fetch operation:",
        error
      );
      window.location.href = "formerror.html";
    });
}

const params = new URLSearchParams(window.location.search);
const dropdownValue = params.get("service");

if (dropdownValue) {
  const selectElement = document.querySelector("select");
  selectElement.value = dropdownValue;
}

function setEstimatedPickupDate(priority, startDate) {
  let daysToAdd = 0;

  if (priority === "Tief") {
    daysToAdd = 12;
  } else if (priority === "Standard") {
    daysToAdd = 7;
  } else if (priority === "Express") {
    daysToAdd = 5;
  }

  const newDate = new Date(startDate);
  newDate.setDate(newDate.getDate() + daysToAdd);

  document.getElementById("endDate").value = `${newDate.getFullYear()}-${String(
    newDate.getMonth() + 1
  ).padStart(2, "0")}-${String(newDate.getDate()).padStart(2, "0")}`;
}

function calculateTotal() {
  let serviceCost = 0;
  let priorityCost = 0;

  const serviceOption = document.querySelector("select").value;
  if (serviceOption === "Kleiner Service") {
    serviceCost = 49;
  } else if (serviceOption === "Grosser Service") {
    serviceCost = 69;
  } else if (serviceOption === "Rennski Service") {
    serviceCost = 99;
  } else if (serviceOption === "Bindungen montieren und einstellen") {
    serviceCost = 39;
  } else if (serviceOption === "Fell zuschneiden") {
    serviceCost = 25;
  } else if (serviceOption === "Heisswachsen") {
    serviceCost = 18;
  }

  const priority = document.querySelector(
    'input[name="list-radio"]:checked'
  ).value;
  if (priority === "Standard") {
    priorityCost = 5;
  } else if (priority === "Express") {
    priorityCost = 10;
  }

  const total = serviceCost + priorityCost;
  document.getElementById("total").value = `CHF ${total}.-`;
}

window.onload = function () {
  document
    .getElementById("submitForm")
    .addEventListener("submit", validateForm);

  const startDateInput = document.getElementById("startDate");
  const today = new Date();
  const formattedToday = `${today.getFullYear()}-${String(
    today.getMonth() + 1
  ).padStart(2, "0")}-${String(today.getDate()).padStart(2, "0")}`;

  startDateInput.value = formattedToday;
  startDateInput.min = formattedToday; // Setzt das minimale Datum auf das heutige Datum

  const priority = document.querySelector(
    'input[name="list-radio"]:checked'
  ).value;
  setEstimatedPickupDate(priority, today);

  document.querySelectorAll('input[name="list-radio"]').forEach((radio) => {
    radio.addEventListener("change", function () {
      console.log("radio changed to " + this.value);
      const startDateValue = startDateInput.value.split("/");
      const startDate = new Date(
        `${startDateValue[2]}-${startDateValue[1]}-${startDateValue[0]}`
      );
      setEstimatedPickupDate(this.value, startDate);
      calculateTotal();
    });
  });

  startDateInput.addEventListener("change", function () {
    const selectedDateValue = this.value.split("/");
    const selectedDate = new Date(
      `${selectedDateValue[2]}-${selectedDateValue[1]}-${selectedDateValue[0]}`
    );
    const priority = document.querySelector(
      'input[name="list-radio"]:checked'
    ).value;
    setEstimatedPickupDate(priority, selectedDate);
  });

  // Event-Listener für Änderungen an der Service-Dropdown-Auswahl
  document.querySelector("select").addEventListener("change", function () {
    calculateTotal();
  });

  // Startwert für den Gesamtbetrag setzen
  calculateTotal();
};

let lastKnownStartDate = "";

setInterval(() => {
  const startDateInput = document.getElementById("startDate");
  const currentStartDate = startDateInput.value;

  if (currentStartDate !== lastKnownStartDate) {
    lastKnownStartDate = currentStartDate;

    const startDate = new Date(currentStartDate);
    const priority = document.querySelector(
      'input[name="list-radio"]:checked'
    ).value;

    setEstimatedPickupDate(priority, startDate);
  }
}, 500); // alle 500 Millisekunden
