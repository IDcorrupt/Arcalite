$(document).ready(function() {

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
                    alert(data.message)
                    $('#register_error').html("")
                },
                error: (data) => {
                    console.log(data)
                    $('#register_error').html(data.responseText)
                }
            });
        }
    });

    $('#loginform')
    
});