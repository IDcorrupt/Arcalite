const verification_num_of_digits = 6;
var verification_code;

/* ---------- PROFILE FUNCTIONS ---------- */

//PASSWORD CHANGE

function changePassword() {
    Alert(
        "Jelszóváltoztatás",
        "A jelszavát kijelentkezés után az \"Elfelejtett jelszó\" funkcióval állíthatja át. Szeretne most kijelentkezni?",
        true,
        () => {
            logout();
            window.open("login.html", "_self");
        },
        "Igen");
}

//NAME CHANGE

function changeName() {
    Alert(
        "Névváltoztatás",
        "Biztosan meg szeretné változtatni a felhasználónevét?",
        true,
        () => {
            Prompt(
                "Névváltoztatás",
                "Adja meg az új felhasználónevet:",
                true,
                () => {
                    updateData($("#popup-textbox").val(), "");
                });
        },
        "Igen");
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
            document.cookie = `username=${data.username}; path=/; secure; SameSite=Strict`;
            Alert("Üzenet", "Adatok sikeresen frissítve!", false, () => {
                window.open("profile.html", "_self");
            });
 
        },
        error: () => {
            Alert("Üzenet", "Hiba történt! Az adatok nem kerültek frissítésre!");
        }
    });
}

//EMAIL CHANGE

function askEmailChange() {
    Alert(
        "E-mail cím változtatás",
        "Biztosan meg szeretné változtatni a fiókhoz tartozó e-mail címet?",
        true,
        () => {
            getNewEmail();
        },
        "Igen");
}

function getNewEmail() {
    Prompt(
        "E-mail cím változtatás",
        "Adja meg az új e-mail címet:",
        true,
        () => {
            verifyEmail($("#popup-textbox").val());
        },
        () => {
            return validEmail($("#popup-textbox").val());
        },
        "Helytelen formátum!");
}

function verifyEmail(email) {
    generateVerificationCode();
    sendVerificationEmail(email, getCookie("username"), verification_code);
    
    Prompt(
        "E-mail cím változtatás",
        "Adja meg az e-mailben megkapott ellenőrző kódot:",
        true,
        () => {
            updateData("", email);
        },
        () => {
            return validateAuthcode($("#popup-textbox").val());
        },
        "Helytelen kód!"
    );
}

function validateAuthcode(authcode) {
    return !isNaN(authcode) && parseInt(authcode) == verification_code;
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
            Alert("Hiba", "E-mail elküldése sikertelen!");
            console.error(data);  
        }
    });
}

//LOGOUT

function askLogout() {
    Alert(
        "Kijelentkezés",
        "Biztosan ki szerente jelentkezni?",
        true,
        () => {
            logout();
            window.open("index.html", "_self");
        },
        "Igen");
}

function logout() {
    document.cookie = "  userid=; path=/; expires="+ new Date(1970, 1, 1);
    document.cookie = "username=; path=/; expires="+ new Date(1970, 1, 1);
}

//DELETE

function askDeletion() {
    Alert(
        "Profiltörlés",
        "Bizosan törölni szeretni a profilját?",
        true,
        () => {
            Alert(
                "Profiltörlés",
                "Törlés után csak munkatársaink segítségével nyerheti vissza a fiókját. Biztosan törölni szeretné a profilt?",
                true,
                () => { deleteProfile(); },
                "Igen");
        },
        "Igen");  
}

function deleteProfile() {
    let payload = {
        userid: getCookie("userid")
    }

    $.ajax({
        type: "POST",
        url: "api/delete_profile.php",
        data: payload,
        global: false,
        success: () => {
            logout();
            Alert("Üzenet", "Profil sikeresen törölve!", false, () => {
                window.open("index.html", "_self");
            });
        },
        error: (data) => {
            Alert("Hiba", `Profil törlése sikertelen!\nHiba: ${data.message}`);
        }
    });
}

/* ---------- FILLING UP CHARACTER TABS ---------- */

$(document).ready(() => {
    let payload = {
        userid: getCookie("userid"),
        langid: getCookie("langid")
    };

    $.ajax({
        type: "GET",
        url: "api/profile.php",
        data: payload,
        global: false,
        success: (data) => {
            FillTabs(data);
        },
        error: (data) => {
            Alert("Hiba", "Hiba a karakteradatok lekérésében!");
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
                <p>Manapont: ${data[i].mp}</p>
            </div>
            <div class="profile-details">
                <p>Játszott idő: ${data[i].playtime}</p>
            </div>
        </div>`);

    }
    
    $("#defaultOpen").click();
}