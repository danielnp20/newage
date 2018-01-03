(function (general, $, undefined)
{
    var kwResetPassword = $("#reset-password-window").kendoWindow
    ({
        title: "Cambiar Contraseña",
        visible: false,
        width: "370px",
        draggable: true,
        modal: true,
        resizable: false
    }).data("kendoWindow");

    $("#update-password").click(function ()
    {
        $("input[name=password]").val("");
        $("input[name=confirmPassword]").val("");

        kwResetPassword.center();
        kwResetPassword.open();
    });

    $("#forgot-password-form").kendoValidator
    ({
        rules:
        {
            verifyPasswords: function (input)
            {
                var ret = true;
                if (input.is("#reset-password-form input[name='confirmPassword']")) {
                    ret = input.val() === $("#reset-password-form input[name='password']").val();
                }
                return ret;
            }
        },
        messages: kendoValidationMessage
    });

    //Reset password
    $("#forgot-password-form").ajaxForm
    ({
        dataType: 'json',
        beforeSubmit: function ()
        {
            AjaxUpdateProgressShow();
        },
        success: function (data)
        {
            var message = $("#login-response-window .message");
            if (data.Success)
            {
                if (data.MailSent)
                {
                    informationMessage("Operación Exitosa", "Su contraseña ha sido modificada. En breves minutos recibirá un correo de confirmación");
                }
                else
                {
                    informationMessage("Operación Fallida", "Su contraseña ha sido modificada de manera existosa, sin embargo se presento un problema enviando el correo. Por favor cambiela nuevamente o puede comunicarse al  657-5900");
                }
                
                kwResetPassword.close();
            }
            else
            {
                errorMessage("Error", "Se presento un problema. Por favor intente nuevamente y si el problema continua por favor comuniquese al 657-5900");
            }

            AjaxUpdateProgressHide();
        },
        error: function ()
        {
            errorMessage("Error", "Se presento un problema. Por favor intente nuevamente y si el problema continua por favor comuniquese al 657-5900");
            AjaxUpdateProgressHide();
        }
    });


    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // PUBLIC FUNCTIONS
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



}(window.login = window.login || {}, jQuery));