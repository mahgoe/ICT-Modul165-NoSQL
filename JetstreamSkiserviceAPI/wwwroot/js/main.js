function main() {
  var themeToggleBtn = document.getElementById("theme-toggle");
  var themeToggleIcon = document.getElementById("theme-toggle-icon");

  function setLogo(path) {
    document.querySelectorAll(".logo").forEach((logo) => {
      logo.src = path;
    });
  }

  // Ändert Logo bei Darkmode, und die Icons fillt es
  function setThemeDark() {
    if (themeToggleIcon.classList.contains("material-fill")) {
      themeToggleIcon.classList.remove("material-fill");
    }
    document.body.classList.add("dark");
    setLogo("./assets/img/logo_white.png");
  }

  function setThemeLight() {
    if (!themeToggleIcon.classList.contains("material-fill")) {
      themeToggleIcon.classList.add("material-fill");
    }
    document.body.classList.remove("dark");
    setLogo("./assets/img/logo.png");
  }

  if (
    localStorage.getItem("color-theme") === "dark" ||
    (!("color-theme" in localStorage) &&
      window.matchMedia("(prefers-color-scheme: dark)").matches)
  ) {
    setThemeDark();
  } else {
    setThemeLight();
  }

  const toggleButton = document.querySelector(
    '[data-collapse-toggle="navbar-default"]'
  );
  const navbarMenu = document.getElementById("navbar-menu");

  toggleButton.addEventListener("click", () => {
    navbarMenu.classList.toggle("hidden");
    navbarMenu.classList.toggle("block");
  });
  themeToggleBtn.addEventListener("click", function () {
    if (localStorage.getItem("color-theme")) {
      if (localStorage.getItem("color-theme") === "light") {
        setThemeDark();
        localStorage.setItem("color-theme", "dark");
      } else {
        setThemeLight();
        localStorage.setItem("color-theme", "light");
      }
    } else {
      if (document.body.classList.contains("dark")) {
        setThemeLight();
        localStorage.setItem("color-theme", "light");
      } else {
        setThemeDark();
        localStorage.setItem("color-theme", "dark");
      }
    }
  });
  handleNavbar();
  getWrapperHeight();
}

function setActiveNavigation(name) {
  document.querySelectorAll(`a[data-page="*"]`).forEach((elm) => {
    elm.classList.remove("active");
  });

  document.querySelector(`a[data-page="${name}"]`).classList.add("active");
}

function setMargin(name, height) {
  const elm = document.getElementById(name);
  if (elm) {
    elm.style.marginTop = height + "px";
  }
}
// Aufruf der Hauptfunktion, wenn das Fenster geladen wird
document.addEventListener("DOMContentLoaded", main);
