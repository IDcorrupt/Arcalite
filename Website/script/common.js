function getCookie(cookieName) {
    let cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        if (cookies[i].includes(cookieName+"=")) {
            return cookies[i].split('=')[1].trim();
        }
    }
    return null;
}

$(document).ready(() => {
    if (getCookie("userid") == null) {
        $("#nav_profil").hide();
        $("#nav_login").show();
    } else {
        $("#nav_profil").show();
        $("#nav_login").hide();
    }
});