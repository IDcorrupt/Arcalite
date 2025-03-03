function getCookie(cookieName) {
    let cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        if (cookies[i].includes(cookieName+"=")) {
            return cookies[i].split('=')[1].trim();
        }
    }
    return null;
}

function logout() {
    if (confirm("Biztosan ki szeretne jelentkezni?")) {
        document.cookie = "  userid=; path=/; expires="+ new Date(1970, 1, 1);
        document.cookie = "username=; path=/; expires="+ new Date(1970, 1, 1);
        window.open("login.html", "_self");
    }
}

$(document).ready(() => {
    //lang csak ideiglenesen van itt, majd át lehet rakni nyelvválasztóba
    document.cookie = `langid=1; path=/; secure; SameSite=Strict`;
    
    if (getCookie("userid") == null) {
        $("#nav_profil").hide();
        $("#nav_login").show();
    } else {
        $("#nav_profil").show();
        $("#nav_login").hide();
    }
});