$(document).ready(function() {
    FetchData("STATISTICS");
    FetchData("ITEM");
    FetchData("ENEMY");
});

function FetchData(type) {
    let payload = {
        userid: getCookie("userid"),
        langid: getCookie("langid"),
        request_type: type
    }

    $.ajax({
        type: "GET",
        url: "api/wiki.php",
        data: payload,
        global: false,
        success: (data) => {
            switch(type) {
                case "STATISTICS":
                    writeStats(data[0]);
                    break;
                case "ITEM":
                    showItems(data);
                    break;
                case "ENEMY":
                    showEnemies(data);
                    break;
            }
        },
        error: (data) => {
            Alert("Hiba", "Hiba az adatok lekérésében!");
            console.error(`Hiba az API kérésben: ${data.responseJSON.code} ${data.responseJSON.message}`);
        }
    });
}

function writeStats(data) {
    $("#itemcount").text(data.itemFound + "/" + data.itemAll);
    $("#enemycount").text(data.enemyFound + "/" + data.enemyAll);
}

function showItems(data) {
    for (let i = 0; i < data.length; i++) {
        let offcanvasId = `itemOffcanvas_${data[i].id}`;

        $("#items").append(`
            <div class="card item-card" data-bs-toggle="offcanvas" data-bs-target="#${offcanvasId}" data-item="${data[i].name}">
                <div class="card-body">
                    <h3 class="card-title">${data[i].name}</h3>
                </div>
                <img src="img/items/${data[i].image}" class="card-img-bottom" alt="${data[i].name}">
            </div>

            <div class="offcanvas offcanvas-end" tabindex="-1" id="${offcanvasId}">
                <div class="offcanvas-header">
                    <h3 class="card-title">${data[i].name}</h3>
                    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
                </div>
                <div class="offcanvas-body">
                    <p>${data[i].desc}</p>
                </div>
            </div>
        `);
    }
}

function showEnemies(data) {
    for (let i = 0; i < data.length; i++) {
        let offcanvasId = `enemyOffcanvas_${data[i].id}`;

        $("#enemies").append(`
            <div class="card enemy-card" data-bs-toggle="offcanvas" data-bs-target="#${offcanvasId}" data-enemy="${data[i].name}">
                <div class="card-body">
                    <h3 class="card-title">${data[i].name}</h3>
                </div>
                <img src="img/enemies/${data[i].image}" class="card-img-bottom" alt="${data[i].name}">
            </div>

            <div class="offcanvas offcanvas-end" tabindex="-1" id="${offcanvasId}">
                <div class="offcanvas-header">
                    <h3 class="card-title">${data[i].name}</h3>
                    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
                </div>
                <div class="offcanvas-body">
                    <p>Max HP: ${data[i].hp}</p>
                    <p>${data[i].desc}</p>
                </div>
            </div>
        `);
    }
}