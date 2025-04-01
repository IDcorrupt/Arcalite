$(document).ready(() => {
    let payload = {
        langid: getCookie("langid")
    }

    $.ajax({
        type: "GET",
        url: "api/index.php",
        data: payload,
        global: false,
        success: (data) => {
            showCharacters(data);
        },
        error: (data) => {
            Alert("Hiba", "Hiba az adatok lekérésében!");
            console.error(`Hiba az API kérésben: ${data.responseJSON.code} ${data.responseJSON.message}`);
        }
    });
});

function showCharacters(data) {
    for (let i = 0; i < data.length; i++) {
        let offcanvasId = `characterOffcanvas_${data[i].id}`;

        $("#characters").append(`
            <div class="card item-card" data-bs-toggle="offcanvas" data-bs-target="#${offcanvasId}" data-character="${data[i].name}">
                <div class="card-body">
                    <h3 class="card-title">${data[i].name}</h3>
                </div>
                <img src="img/avatars/${data[i].image}" class="card-img-bottom" alt="${data[i].name}">
            </div>

            <div class="offcanvas offcanvas-end" tabindex="-1" id="${offcanvasId}">
                <div class="offcanvas-header">
                    <h3 class="card-title">${data[i].name}</h3>
                    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
                </div>
                <div class="offcanvas-body">
                    <p>${data[i].description}</p>
                </div>
            </div>
        `);
    }
}