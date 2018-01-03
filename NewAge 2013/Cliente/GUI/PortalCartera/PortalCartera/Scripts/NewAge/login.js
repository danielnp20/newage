(function (login, $, undefined)
{
    var kwError = $("#login-response-window").kendoWindow
    ({
        title: "Login",
        visible: false,
        width: "370px",
        draggable: true,
        modal: true,
        resizable: false
    }).data("kendoWindow");

    var kwForgotPwd = $("#forgot-password-window").kendoWindow
    ({
        title: "Olvido su contraseña",
        visible: false,
        width: "370px",
        draggable: true,
        modal: true,
        resizable: false
    }).data("kendoWindow");
    
    $("#login-form").kendoValidator
    ({
        messages: kendoValidationMessage
    });

    $("#forgot-password-form").kendoValidator
    ({
        messages: kendoValidationMessage
    });

    $("#forgotPassword").click(function ()
    {
        $("input[name=email]").val("");
        $("input[name=id]").val("");

        kwForgotPwd.center();
        kwForgotPwd.open();
    });

    //Login
    $("#login-form").ajaxForm
    ({
        dataType: 'json',
        beforeSubmit: function ()
        {
            AjaxUpdateProgressShow();
        },
        success: function (data)
        {
            if (data.Success)
            {
                window.location.href = data.Url;
            }
            else
            {
                //Title
                var title = $("#login-response-window .title");
                title.html("Error");

                //Message
                var message = $("#login-response-window .message");
                if (data.Code === 8)
                {
                    message.html("Ha ocurrido un error, por favor inténtelo nuevamente. si el problema persiste por favor comuniquese al número 657-5900");
                }
                else
                {
                    message.html("La información ingresada no es válida. Por favor intente nuevamente");
                }

                kwError.center();
                kwError.open();
                $("#login-response-window").show();
                AjaxUpdateProgressHide();
            }
        },
        error: function ()
        {
            var message = $("#login-response-window .message");
            message.html(responseCodeMessages[8]);
            kwError.center();
            kwError.open();
            $("#login-response-window").show();
            AjaxUpdateProgressHide();
        }
    });

    //Forgot password
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
                //Title
                var title = $("#login-response-window .title");
                title.html("Operación Exitosa");

                //Message
                var message = $("#login-response-window .message");
                message.html("Su contraseña ha sido modificada. En breves minutos recibirá un correo de confirmación.");

                kwForgotPwd.close();
                kwError.center();
                kwError.open();
                $("#login-response-window").show();
            }
            else 
            {
                //Title
                var title = $("#login-response-window .title");
                title.html("Error");

                //Message
                var message = $("#login-response-window .message");
                if (!data.ValidUser)
                {
                    message.html("La información ingresada del correo y la cédula no es válida. Por favor verifique sus datos e intente nuevamente");
                }
                else
                {
                    message.html("Ha ocurrido un error, por favor inténtelo nuevamente. si el problema persiste por favor comuniquese al número 657-5900");
                }


                kwError.center();
                kwError.open();
                $("#login-response-window").show();
            }

            AjaxUpdateProgressHide();
        },
        error: function ()
        {
            var message = $("#login-response-window .message");
            message.html(responseCodeMessages[8]);
            kwError.center();
            kwError.open();
            $("#login-response-window").show();
            AjaxUpdateProgressHide();
        }
    });

}(window.login = window.login || {}, jQuery));