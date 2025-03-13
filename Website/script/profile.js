const verification_num_of_digits = 6;
var verification_code;

/* ---------- PROFILE FUNCTIONS ---------- */

//PASSWORD CHANGE

function changePassword() {
    if (confirm("A jelszavát kijelentkezés után az \"Elfelejtett jelszó\" funkcióval állíthatja át. Szeretne most kijelentkezni?")) {
        logout();
        window.open("login.html", "_self");
    }
}

//NAME CHANGE

function changeName() {
    if (confirm("Biztosan meg szeretné változtatni a felhasználónevét?")) {
        let newName = prompt("Adja meg az új felhasználónevet:", getCookie("username"));

        if (newName == null || newName== "") { return; }

        updateData(newName, "");
    }
}

function updateData(newName, newEmail) {
    let payload = {
        userid: getCookie("userid"),
        username: newName,
        email: newEmail
    }

    $.ajax({
        type: "POST",
        url: "api/update.php",
        data: payload,
        global: false,
        success: (data) => {
            alert("Adatok sikeresen frissítve!");
            document.cookie = `username=${data.username}; path=/; secure; SameSite=Strict`;
            window.open("profile.html", "_self");
        },
        error: () => {
            alert("Hiba történt! Az adatok nem kerültek frissítésre!");
        }
    });
}

//EMAIL CHANGE

function changeEmail() {
    if (confirm("Biztosan meg szeretné változtatni a fiókhoz tartozó e-mail címet?")) {
        let newEmail;
        do {
            newEmail = prompt("Adja meg az új e-mail címet:");
        } while (newEmail != null && !validEmail(newEmail));

        if (newEmail == null || newEmail == "") {return;}

        generateVerificationCode();
        sendVerificationEmail(newEmail, getCookie("username"), verification_code);

        let authcode;
        do {
            authcode = prompt("Adja meg az e-mailben kapott kódot:");
        } while (authcode != null && (isNaN(authcode) || parseInt(authcode) != verification_code));

        if (authcode == null) {return;}

        updateData("", newEmail);
    }
}

function validEmail(email_input) {
    return email_input.includes('@') && email_input.includes('.') && email_input.split('@').length == 2 && email_input.split('@')[1].includes('.');
}

function generateVerificationCode() {
    verification_code = "";
    for (let i = 0; i < verification_num_of_digits; i++) { 
        verification_code += Math.floor(Math.random() * 10); 
    }
}

function sendVerificationEmail(email, name, code) {
    let payload = {
        service_id: "service_0bn5ksk",
        template_id: "template_hbtshdc",
        user_id: "zXoo2LhCRJHkRSH4q",
        template_params: {
            'to_name': name,
            'to_email':email,
            'message': code
        }
    };

    $.ajax({
        type: "POST",
        url: "https://api.emailjs.com/api/v1.0/email/send",
        data: JSON.stringify(payload),
        contentType: "application/json",
        global: false,
        error: (data) => {
            alert("E-mail elküldése sikertelen!");
            console.error(data);  
        }
    })
}

//LOGOUT

function askLogout() {
    if (confirm("Biztosan ki szeretne jelentkezni?")) {
        logout();
        window.open("index.html", "_self");
    }
}

function logout() {
    document.cookie = "  userid=; path=/; expires="+ new Date(1970, 1, 1);
    document.cookie = "username=; path=/; expires="+ new Date(1970, 1, 1);
}

//DELETE

function deleteProfile() {
    if (confirm("Biztosan törölni szeretné a profilját?") && confirm("Ez a lépés nem visszafordítható. Biztosan törölni szeretné a profilt?")) {
        let payload = {
            userid: getCookie("userid")
        }

        $.ajax({
            type: "POST",
            url: "api/delete_profile.php",
            data: payload,
            global: false,
            success: (data) => {
                alert(data.message);
                logout();
                window.open("index.html", "_self");
            },
            error: (data) => {
                alert(data.message);
            }
        });
    }
}

/* ---------- FILLING UP CHARACTER TABS ---------- */

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
            FillTabs(data);
        },
        error: (data) => {
            alert(`Hiba az adatok lekérésében!`);
            console.error(`Hiba az API kérésben: ${data.code} ${data.message}`);
        }
    });
});

function FillTabs(data) {
    $("#h2_username").text(getCookie("username"));
    
    $("#characterTabs").html("");
    $("#characterContents").html("");

    for (let i = 0; i < data.length; i++) {
        
        $("#characterTabs").append(`<button ${(i == 0 ? 'id = \"defaultOpen\"' : '')} class="tablinks" onclick="openRank(event, 'Player_${data[i].id}')">${data[i].name}</button>`);

        $("#characterContents").append(`
        <div id="Player_${data[i].id}" class="profile tabcontent">
            <div class="profile-details">
                <img class="profile-img" src="./img/Avatars/${data[i].image}" alt="${data[i].image}">
            </div>
            <div class="profile-details" id="profile-mid">
                <p>Szoba: ${data[i].level}</p>
            </div>
            <div class="profile-details">
                <p>Életpont: ${data[i].hp}</p>
            </div>
            <div class="profile-details">
                <p>Játszott idő: ${data[i].playtime}</p>
            </div>
        </div>`);

    }
    
    $("#defaultOpen").click();
}