const verification_num_of_digits = 6;
var verification_code;

$(document).ready(function() {
    $('#registerform').on('submit', register);
    $('#register_verify').click(verifyRegister);

    $('#loginform').on('submit', login);
    
    $('#forgotpwd').click(forgotten_sendCode);
    $('#newpwd_verify').click(forgotten_verifyCode);
    $('#newpwd_set').click(forgotten_setNewPassword);
});

//----------REGISZTRÁCIÓ----------

function register(event) {
    event.preventDefault();
    
    if ($('#reg_password').val() != $('#reg_passwordagain').val()) {
        $('#pwdagain_error').show();
        return;
    }
    $('#pwdagain_error').hide();

    generateVerificationCode();
    sendVerificationEmail($('#reg_email').val(), $('#reg_username').val(), verification_code, "Email");

    $('#register_base').hide();
    $('#register_verification').show();
}

function verifyRegister() {
    if ($('#register_code').val() == "" || isNaN($('#register_code').val())) {
        $("#newpwd_authcode_error").text("Adja meg az e-mailben kapott kódot!");
        return;
    }

    if (parseInt($('#register_code').val()) != verification_code) {
        $("#newpwd_authcode_error").text("Helytelen kód!");
        return;
    }
    
    uploadNewProfile();
    /* window.open("login.html", "_self"); */
}

function uploadNewProfile() {
    let payload = {
        username: $('#reg_username').val(),
        email: $('#reg_email').val(),
        password: $('#reg_password').val()
    }
    
    $.ajax({
        type: "POST",
        url: "api/register.php",
        data: payload,
        global: false,
        success: (data) => {
            $('#register_error').html("");
            Alert("Üzenet", data.message, false, () => {
                location.replace(location.href.split("?")[0]);
                location.reload();
            });
        },
        error: (data) => {
            $('#register_error').html(data.message);
        }
    });
}

//----------BEJELENTKEZÉS----------

function login(event) {
    event.preventDefault();

    let payload = {
        email: $('#login_email').val(),
        password: $('#login_password').val()
    }

    $.ajax({
        type: "GET",
        url: "api/login.php",
        data: payload,
        global: false,
        success: (data) => {
            window.open("index.html", "_self");
            document.cookie = `  userid=${data.id}      ; path=/; secure; SameSite=Strict`;
            document.cookie = `username=${data.username}; path=/; secure; SameSite=Strict`;
        },
        error: (data) => {
            $('#login_error').html(data.responseJSON.message);
        }
    });
}

//----------ELFELEJTETT JELSZÓ----------

function forgotten_sendCode() {
    $('#newpwd_authcode_container').hide();
    $('#newpwd_newpwd_container').hide();

    //----------ELLENŐRZÉS HOGY AZ E-MAIL LÉTEZIK-E----------

    let email_input = $('#login_email').val();

    if (email_input == "") {
        $("#login_error").text("Adjon meg egy e-mail címet!");
        return;   
    }

    if (!validEmail(email_input)) {
        $("#login_error").text("Helytelen e-mail cím formátum!");
        return;
    }

    //----------KÓD KÜLDÉSE----------

    let payload = {
        email: $('#login_email').val()
    }

    $.ajax({
        type: "GET",
        url: "api/email_exists.php",
        data: payload,
        global: false,
        success: (data) => {
            if (!data.exists) {
                $('#login_error').html("Nincs ilyen e-mail cím regisztrálva!");
                return;
            }
            $('#login_error').html("");

            generateVerificationCode();
            sendVerificationEmail(data.email, data.username, verification_code, "Forgotten");
            
            $('#login_base').hide();
            $('#newpwd_authcode').val("");
            $('#newpwd_authcode_container').show();
            $('#newpwd_newpwd_container').hide();
        },
        error: (data) => {
            $('#login_error').html(data.message);
        }
    })
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

function getTemplateCode(template) {
    switch (template) {
        case "Forgotten":
            return "template_5phw0br";
        case "Email":
            return "template_hbtshdc";
        default:
            return;
    }
}

function sendVerificationEmail(email, name, code, template) {
    let templateID = getTemplateCode(template);

    let payload = {
        service_id: "service_0bn5ksk",
        template_id: templateID,
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
            Alert("Hiba", "E-mail elküldése sikertelen!", false, () => {
                window.open("login.html", "_self");
                console.error(data);  
            });
        }
    })
}

function forgotten_verifyCode() {
    if ($('#newpwd_authcode').val() == "" || isNaN($('#newpwd_authcode').val())) {
        $("#newpwd_authcode_error").text("Adja meg az e-mailben kapott kódot!");
        return;
    }

    if (parseInt($('#newpwd_authcode').val()) != verification_code) {
        $("#newpwd_authcode_error").text("Helytelen kód!");
        return;
    }

    $('#newpwd_authcode_container').hide();
    $('#newpwd_newpwd_container').show();
    $('#newpwd_newpwd').val("");
    $('#newpwd_again').val("");
}

function forgotten_setNewPassword(){
    if ($('#newpwd_newpwd').val() == "") {
        $("#newpwd_newpwd_error").text("Adjon meg egy új jelszót!");
        return;
    }

    if ($('#newpwd_newpwd').val() != $('#newpwd_again').val()) {
        $("#newpwd_newpwd_error").text("A megerősítő jelszó nem egyezik az előtte megadottal!");
        return;
    }

    let payload = {
        email: $('#login_email').val(),
        password: $('#newpwd_newpwd').val()
    }

    $.ajax({
        type: "POST",
        url: 'api/new_password.php',
        data: payload,
        global: false,
        success: (data) => {
            Alert("Üzenet", data.message, false, () => {
                $('#newpwd_newpwd_container').hide();
                $('#login_base').show();
            });
        },
        error: (data) => {
            Alert("Hiba", data.message, false, () => {
                $('#newpwd_newpwd_container').hide();
                $('#login_base').show();
            });
        }
    });

}

function switchToLogin() {
    $("#registerdiv").hide();
    $("#logindiv").show();
}

function switchToRegister() {
    $("#registerdiv").show();
    $("#logindiv").hide();
}