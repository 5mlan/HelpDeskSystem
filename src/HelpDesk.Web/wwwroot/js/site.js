document.addEventListener("DOMContentLoaded", () => {
  const menuButton = document.getElementById("menuButton");
  const sidebar = document.getElementById("sidebar");

  menuButton?.addEventListener("click", () => sidebar?.classList.toggle("open"));

  document.addEventListener("click", (event) => {
    if (window.innerWidth <= 900 && sidebar?.classList.contains("open") &&
        !sidebar.contains(event.target) && !menuButton?.contains(event.target)) {
      sidebar.classList.remove("open");
    }
  });

  document.querySelectorAll(".alert").forEach((alert) => {
    setTimeout(() => alert.classList.add("fade-out"), 4500);
  });
});
