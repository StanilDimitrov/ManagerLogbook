$(function () {
    const $submitForm = $('#submit-form-logbook');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();

        //var url = "/Manager/Users/SwitchLogbook/";
        var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
            //console.log(dataToSend);

            toastr.options = {
                "debug": false,
                "positionClass": "toast-top-center",
                "onclick": null,
                "fadeIn": 300,
                "fadeOut": 1000,
                "timeOut": 3000,
                "extendedTimeOut": 3000,
                "closeButton": true
            }

            toastr.success(response);
            $('#myModalLogbook').modal('hide');

        }).fail(function (response) {
            toastr.options = {
                "debug": false,
                "positionClass": "toast-top-center",
                "onclick": null,
                "fadeIn": 300,
                "fadeOut": 1000,
                "timeOut": 3000,
                "extendedTimeOut": 3000,
                "closeButton": true
            }
            toastr.error(response.responseText);
        });
    });
});