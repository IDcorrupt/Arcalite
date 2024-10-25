const xhttp = new XMLHttpRequest();

function getFormData(id) {
    var urlap = document.getElementById(id);
    var params = "";
    new FormData(urlap).forEach((value, key) => {
        if (params != "") { params += '&'; }
        params += key + "=" + value;
    });
    return params;
}

function login() {
    var params = getFormData("bejel");

    xhttp.open("GET", "login.php?"+params);    
    xhttp.send();

    xhttp.onreadystatechange = function() {
        if (xhttp.readyState == 4) {
            console.log(JSON.parse(xhttp.responseText))
        }
    }
}

function register() {
    var params = getFormData("regisz");

    xhttp.open("POST", "register.php");
    xhttp.setRequestHeader('Content-Type','application/x-www-form-urlencoded');
    xhttp.send(params);

    xhttp.onreadystatechange = function() {
        if (xhttp.readyState == 4) {
            document.getElementById('reg_eredmeny').innerHTML = xhttp.responseText;
        }
    }
}