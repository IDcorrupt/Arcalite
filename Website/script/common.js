/* ---------- POPUP FUNCTIONALITIES ---------- */

function showPopup() {
    $("#popup-div").css("transform", "translateX(calc(50vw - 50%)) translateY(0%)");
}

function setupPopup(title, message, hasCancel) {
    $("#popup-title").text(title);
    $("#popup-message").text(message);
    
    if (hasCancel) {
        $("#popup-cancel").show();
    } else {
        $("#popup-cancel").hide();
    }

    $("#popup-send").off("click");
    $("#popup-ok").off("click");
}

function setAlert(title, message, hasCancel = false, callback = () => {}, okText = "OK") {
    setupPopup(title, message, hasCancel);

    $("#popup-ok").text(okText);
    
    $("#popup-ok").show();
    $("#popup-send").hide();
    $("#popup-textbox").hide();
    $("#popup-error").hide();
    
    $("#popup-ok").click(() => {
        callback();
        closePopup();
    });
}

function setPrompt(title, message, hasCancel = false, callback = () => {}, verify = () => { return $("#popup-textbox").val() != "";}, failMessage = "Töltse ki a mezőt!") {
    setupPopup(title, message, hasCancel);
    
    $("#popup-textbox").val("");
    $("#popup-textbox").css("border", "none");
    $("#popup-error").text("");

    $("#popup-ok").hide();
    $("#popup-send").show();
    $("#popup-textbox").show();
    $("#popup-error").show();
    
    $("#popup-send").click(() => {
        if (verify()) {
            $("#popup-error").text("");
            callback();
            closePopup();
        } else {
            $("#popup-textbox").css("border", "2px solid red");
            $("#popup-error").text(failMessage);
        }
    });
}

function closePopup() {
    $("#popup-div").css("transform", "translateX(calc(50vw - 50%)) translateY(-100%)");
}

function Alert(title, message, hasCancel = false, callback = () => {}, okText = "OK") {
    closePopup();
    setTimeout(() => {
        setAlert(title, message, hasCancel, callback, okText);
        showPopup();
    }, 500);
}

function Prompt(title, message, hasCancel = false, callback = () => {}, verify = () => { return $("#popup-textbox").val() != "";}, failMessage = "Töltse ki a mezőt!") {
    closePopup();
    setTimeout(() => {
        setPrompt(title, message, hasCancel, callback, verify, failMessage);
        showPopup();
    }, 500);
}

$("#popup-cancel").click(closePopup);

/* ---------- OTHER MISC FUNCTIONALITIES ---------- */

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