function logout() {
    if (confirm("Biztosan ki szeretne jelentkezni?")) {
        document.cookie = "  userid=; path=/; expires="+ new Date(1970, 1, 1);
        document.cookie = "username=; path=/; expires="+ new Date(1970, 1, 1);
        window.open("index.html", "_self");
    }
}

$(document).ready(() => {
    let payload = {
        userid: getCookie("userid"),
        langid: getCookie("langid")
    }

    $.ajax({
        type: "GET",
        url: "api/profile.php",
        data: payload,
        global: false,
        success: (data) => {
            console.log(data);
            FillTabs(data);
        },
        error: (data) => {
            alert(`Hiba az adatok lekérésében!`);
            console.error(`Hiba az API kérésben: ${data.code} ${data.message}`);
        }
    });
});

function FillTabs(data) {
    $("#characterTabs").html("");
    $("#characterContents").html("");

    for (let i = 0; i < data.length; i++) {
        
        $("#characterTabs").append(`<button ${(i == 0 ? 'id = \"defaultOpen\"' : '')} class="tablinks" onclick="openRank(event, 'Player_${data[i].id}')">${data[i].name}</button>`);

        $("#characterContents").append(`
        <div id="Player_${data[i].id}" class="profile tabcontent">
            <div class="profile-details">
                <img class="profile-img" src="./img/${data[i].image}.png" alt="${data[i].image}">
            </div>
            <div class="profile-details" id="profile-mid">
                <p>Szoba: ${data[i].level}</p>
            </div>
            <div class="profile-details">
                <p>Életpont: ${data[i].hp}</p>
                <p>Mana: </p>
            </div>
        </div>`);

    }
    
    $("#defaultOpen").click();
}