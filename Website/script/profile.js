$(document).ready(function() {

    $('#pwdagain_error').hide();

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
                $('#register_error').html(data.responseText)
            }
        });
    
    });

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
                data = JSON.parse(data);
                alert(data.id);
            },
            error: (data) => {
                $('#login_error').html(data.responseText)
            }
        });
    });
    
});