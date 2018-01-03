(function (searchCredits, $, undefined)
{
    $(document).ajaxStop(function ()
    {
        AjaxUpdateProgressHide();
    });

    //kdp
    var now = new Date();
    var minDate = now.setHours(now.getHours() + gmt);
    minDate = parseDate(minDate);
    var maxDate = now.setMonth(now.getMonth() + 1);
    maxDate = parseDate(maxDate);
    $("input[name='date']").kendoDatePicker({
        value: minDate,
        min: minDate,
        max: maxDate
    });

    $(".search-credits").click(function ()
    {
        loadData();
    });

    //View model
    searchCredits.viewModel = kendo.observable
    ({
        gridData: []
    });
    kendo.bind($("#credits-container"), searchCredits.viewModel);

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // PRIVATE FUNCTIONS
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function loadData()
    {
        var val = $("input[name='date']").data("kendoDatePicker").value();
        if (!val)
        {
            var now = new Date();
            var minDate = now.setHours(now.getHours() + gmt);
            val = parseDate(minDate);

            $("input[name='date']").data("kendoDatePicker").value = val;
        }
        var myDate = kendo.toString(val, "d");

        $.ajax
        ({
            url: urlGetData,
            type: "POST",
            data:
            {
                date: myDate
            },
            dataType: "json",
            beforeSend: function ()
            {
                AjaxUpdateProgressShow();
            },
            success: function (result)
            {
                data = result;
                if (data.total === 0)
                {
                    $("#grid-container").hide();
                    informationMessage("Sin Creditos", "El cliente actual no tiene créditos disponibles para la fecha seleccionada");
                }
                else
                {
                    searchCredits.viewModel.set("gridData", data.saldos);
                    $("#grid-container").show();
                }

                fixFooter();
            },
            error: function (data)
            {
                $("#grid-container").hide();
                errorMessage("Error", "Se presento un problema generando la consulta. Por favor intente nuevamente. Si el problema continua por favor comuniquese al 657-5900");
            }
        });
    }



    //Fix window heigh according to data displayed (PONER EN GENERAL.JS)
    function fixFooter() {
        $('footer').removeClass('fixedfooter');
        var WindowHeight = $(window).outerHeight();
        var HeaderHeight = $('.menu-site').outerHeight();
        var ContainerHeight = $('.main-container').outerHeight();
        var FooterHeight = $('footer').outerHeight();
        var ContentPageHeight = HeaderHeight + ContainerHeight + FooterHeight;
        if (ContentPageHeight < WindowHeight) {
            $('footer').addClass('fixedfooter');
        }
        else {
            $('footer').removeClass('fixedfooter');
        }
    }


    $(document).ready(function() 
    {
        fixFooter();
    });

    $(window).resize(function ()
    {
        fixFooter();
    });


}(window.searchCredits = window.searchCredits || {}, jQuery));