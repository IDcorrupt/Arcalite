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
            alert(`Hiba az adatok lekérésében!`);
            console.error(`Hiba az API kérésben: ${data.code} ${data.message}`);
        }
    });
}

function writeStats(data) {
    $("#itemcount").text(data.itemFound + "/" + data.itemAll);
    $("#enemycount").text(data.enemyFound + "/" + data.enemyAll);
}

function showItems(data) {
    for (let i = 0; i < data.length; i++) {
        $("#items").append(`
            <article id='item_${data[i].id}'>
                <h3>${data[i].name}</h3>
                <img src="img/items/${data[i].image}" alt="${data[i].name}">
                <p>${data[i].desc}</p>
            </article>
        `);
    }
}


function showEnemies(data) {
    for (let i = 0; i < data.length; i++) {
        $("#enemies").append(`
            <article id='enemy_${data[i].id}'>
                <h3>${data[i].name}</h3>
                <img src="img/enemies/${data[i].image}" alt="${data[i].name}">
                <p>Max. életpont: ${data[i].hp}</p>
                <p>${data[i].desc}</p>
            </article>
        `);
    }
}