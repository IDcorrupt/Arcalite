$(document).ready(function() {
    FetchRankings("Profile");
    FetchRankings("GameThrough");
});

function FetchRankings(type) {
    let payload = {
        type: type,
        langid: 1
    }

    $.ajax({
        type: "GET",
        url: "api/ranking.php",
        data: payload,
        global: false,
        success: (data) => {
            console.log(data);
            FillTable(type, data);
        },
        error: (data) => {
            alert(`Hiba az adatok lekérésében.\n(${data.code} ${data.message})`)
        }
    });
}

function FillTable(type, data) {
    switch(type) {
        case "Profile":
            for (let i=0; i < data.length; i++) {
                $(`#${type} tbody`).append(
                    `<tr>
                        <td>${data[i].Felhasználónév}</td>
                        <td>${data[i].Játékidő}</td>
                        <td>${data[i].Elért_mérföldkövek}</td>
                        <td>${data[i].Végigjátszások}</td>
                    </tr>`);
            }
            break;
        case "GameThrough":
            for (let i=0; i < data.length; i++) {
                $(`#${type} tbody`).append(
                    `<tr>
                        <td>${data[i].Profil}</td>
                        <td>${data[i].Karakter}</td>
                        <td>${data[i].Avatár}</td>
                        <td>${data[i].Játékidő}</td>
                        <td>${data[i].Elért_szint}</td>
                        <td>${data[i].Felfedezett_ellenfelek}</td>
                        <td>${data[i].Felfedezett_tárgyak}</td>
                    </tr>`);
            }
            break;
    
    }
    
}