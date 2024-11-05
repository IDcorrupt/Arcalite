$(document).ready(function() {

    $('#pwdagain_error').hide();

    $('#registerform').on('submit', function(event) {
        event.preventDefault();
        if ($('#password').val() != $('#passwordagain').val()) {
            $('#pwdagain_error').show();
        } else {
            $('#pwdagain_error').hide();

            let payload = {
                username: $('#username').val(),
                email: $('#email').val(),
                password: $('#password').val()
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
        }
    });

    $('#loginform').on('submit', function(event) {
        event.preventDefault();
        //weewoo
    });
    
});