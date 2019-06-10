$(function () {
    const $submitForm = $('#create-business-unit');     

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        
        var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
            console.log(response.firstName);
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

$(function () {
    const $submitForm = $('#update-business-unit');
       
    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
                var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
            console.log(response.firstName);
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


$('#add-moderator-global-button').click(function (event) {
    $.get("/Manager/BusinessUnits/GetAllModerators")
        .done(function (response) {
            var s = '<option value="-1" selected disabled hidden>Please Select Moderator</option>';
            for (var i = 0; i < response.length; i++) {
                s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
            }

            $("#moderator-selector").html(s);

        }).fail(function (response) {

        });
});