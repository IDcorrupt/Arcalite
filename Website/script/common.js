function getCookie(cookieName) {
    let cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        if (cookies[i].includes(cookieName+"=")) {
            return cookies[i].split('=')[1].trim();
        }
    }
    return null;
}