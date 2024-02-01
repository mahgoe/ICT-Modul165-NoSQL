document
  .getElementById("loginForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    fetch("/employees/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username: username, password: password }),
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          if (response.status === 401) {
            return response.text().then((text) => {
              throw new Error(text);
            });
          }
          throw new Error("Failed to login");
        }
      })
      .then((data) => {
        localStorage.setItem("token", data.token);
        window.location.href = "/adminconfirm.html";
      })
      .catch((error) => {
        if (
          error.message.includes(
            "Your account is locked. Please contact administrator."
          )
        ) {
          window.location.href = "/adminshutdown.html";
        } else {
          console.error("Login Error:", error);
        }
      });
  });
