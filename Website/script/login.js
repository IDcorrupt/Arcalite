const verification_num_of_digits = 6;
var verification_code;

$(document).ready(function() {

    //----------REGISZTRÁCIÓ----------

    $('#registerform').on('submit', function(event) {
        event.preventDefault();
        
        if ($('#reg_password').val() != $('#reg_passwordagain').val()) {
            $('#pwdagain_error').show();
            return;
        }

        $('#pwdagain_error').hide();

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
                alert(data.message);
                location.replace(location.href.split("?")[0]);
                location.reload();
            },
            error: (data) => {
                $('#register_error').html(data.message);
            }
        });
    
    });

    //----------BEJELENTKEZÉS----------

    $('#loginform').on('submit', function(event) {
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
                open("index.html", "_self");
                document.cookie = `userid=${data.id}; path=/; secure; SameSite=Strict`;
                document.cookie = `username=${data.username}; path=/; secure; SameSite=Strict`;
                document.cookie = `langid=1; path=/; secure; SameSite=Strict`;
            },
            error: (data) => {
                $('#login_error').html(data.responseJSON.message);
            }
        });
    });

    //----------ELFELEJTETT JELSZÓ----------

    $('#forgotpwd').click(function() {
        $('#newpwd_authcode_container').hide();
        $('#newpwd_newpwd_container').hide();

        let email_input = $('#login_email').val();

        if (email_input == "") {
            alert('Adjon meg egy e-mail címet!');
            return;   
        }
        if ( !email_input.includes('@') || !email_input.includes('.') || 
              email_input.split('@').length != 2 || !email_input.split('@')[1].includes('.')) 
        {
            alert('Adjon meg egy helyes e-mail címet!');
            return;
        }

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

                verification_code = "";
                for (let i = 0; i < verification_num_of_digits; i++) { verification_code += Math.floor(Math.random() * 10); }
                alert(`Az email-ben megkapott kód: ${verification_code}`);
                
                $('#newpwd_authcode').val("");
                $('#newpwd_authcode_container').show();
                $('#newpwd_newpwd_container').hide();
            },
            error: (data) => {
                $('#login_error').html(data.message);
            }
        })
    });

    $('#newpwd_verify').click(function() {
        if ($('#newpwd_authcode').val() == "" || !isFinite($('#newpwd_authcode').val())) {
            alert("Adja meg az e-mailben kapott kódot!");
            return;
        }

        if ( parseInt($('#newpwd_authcode').val()) != verification_code ) {
            alert("Helytelen kód!");
            return;
        }

        $('#newpwd_authcode_container').hide();
        $('#newpwd_newpwd_container').show();
        $('#newpwd_newpwd').val("");
        $('#newpwd_again').val("");
    });

    $('#newpwd_set').click(function(){
        if ($('#newpwd_newpwd').val() == "") {
            alert("Adjon meg egy új jelszót!");
            return;
        }

        if ($('#newpwd_newpwd').val() != $('#newpwd_again').val()) {
            alert("A megerősítő jelszó nem egyezik az előtte megadottal!");
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
                alert(data.message);
            },
            error: (data) => {
                alert(data.message);
            }
        });


        $('#newpwd_newpwd_container').hide();
    });
});