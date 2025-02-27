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
    //magyar nyelv alapbeállítása
    if (getCookie("langid") == null) { document.cookie = `langid=1; path=/; secure; SameSite=Strict`; }
    
    //világos téma alapbeállítása
    if (getCookie("theme") == null) { document.cookie = `theme=light; path=/; secure; SameSite=Strict`; }
    setTheme()

    if (getCookie("userid") == null) {
        $("#nav_profil").hide();
        $("#nav_login").show();
        if (!$("#nav_wiki").hasClass("disabled")) { 
            $("#nav_wiki").addClass("disabled");
            document.querySelector(".nav-item:has(a#nav_wiki)").title = "Jelentkezzen be a lexikon eléréséhez"; 
        }
    } else {
        $("#nav_profil").show();
        $("#nav_login").hide();
        if ($("#nav_wiki").hasClass("disabled")) { 
            $("#nav_wiki").removeClass("disabled");
            document.querySelector(".nav-item:has(a#nav_wiki)").title = "";
        }
    }
});