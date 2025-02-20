
document.querySelectorAll(".character-card").forEach(card => {
    card.addEventListener("click", function () {
        let characterName = this.getAttribute("data-character");
        
        // Update Offcanvas content
        document.getElementById("offcanvasTitle").textContent = characterName;
        document.getElementById("offcanvasContent").textContent = characterName + " details will be displayed here.";

        // Show Offcanvas
        let offcanvas = new bootstrap.Offcanvas(document.getElementById("characterOffcanvas"));
        offcanvas.show();
    });
});


document.querySelectorAll(".enemy-card").forEach(card => {
    card.addEventListener("click", function () {
        let enemyName = this.getAttribute("data-enemy");
        
        // Update Offcanvas content
        document.getElementById("offcanvasTitle").textContent = enemyName;
        document.getElementById("offcanvasContent").textContent = enemyName + " details will be displayed here.";

        // Show Offcanvas
        let offcanvas = new bootstrap.Offcanvas(document.getElementById("enemyOffcanvas"));
        offcanvas.show();
    });
});

document.querySelectorAll(".item-card").forEach(card => {
    card.addEventListener("click", function () {
        let itemName = this.getAttribute("data-item");
        
        // Update Offcanvas content
        document.getElementById("offcanvasTitle").textContent = itemName;
        document.getElementById("offcanvasContent").textContent = itemName + " details will be displayed here.";

        // Show Offcanvas
        let offcanvas = new bootstrap.Offcanvas(document.getElementById("itemOffcanvas"));
        offcanvas.show();
    });
});

function themeChange() {
    var element = document.body;
    element.dataset.bsTheme =
    element.dataset.bsTheme == "light" ? "dark" : "light";
}