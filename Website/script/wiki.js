$(document).ready(function() {
    FetchData("STATISTICS");
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