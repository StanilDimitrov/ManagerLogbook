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
            $('#myModalBusinessUnit').modal('hide');
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

//$(function () {
//    const $submitForm = $('#create-business-unit');

//    //if ($submitForm.valid()) {

//    $submitForm.on('submit', function (event) {
//        event.preventDefault();

//        var $this = $(this);

//        const dataToSend = $submitForm.serialize();
//        //var url = "/Admin/Create/Register";
//        var url = $this.attr('action');

//        $.post(url, dataToSend, function (response) {
//            console.log(response.firstName);
//            toastr.options = {
//                "debug": false,
//                "positionClass": "toast-top-center",
//                "onclick": null,
//                "fadeIn": 300,
//                "fadeOut": 1000,
//                "timeOut": 3000,
//                "extendedTimeOut": 3000,
//                "closeButton": true
//            }
//            $('#myModalBusinessUnit').modal('hide');
//            toastr.success(response);

//        }).fail(function (response) {
//            toastr.options = {
//                "debug": false,
//                "positionClass": "toast-top-center",
//                "onclick": null,
//                "fadeIn": 300,
//                "fadeOut": 1000,
//                "timeOut": 3000,
//                "extendedTimeOut": 3000,
//                "closeButton": true
//            }
//            toastr.error(response.responseText);
//        });

//    });
//});



$(function () {
    const $submitForm = $('#update-business-unit');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
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
            $('#myModalUpdateBusinessUnit').modal('hide');
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
    $.get("/Admin/BusinessUnits/GetAllModerators")
        .done(function (response) {
            var s = '<option value="-1" selected disabled hidden>Please Select Moderator</option>';
            for (var i = 0; i < response.length; i++) {
                s += '<option value="' + response[i].id + '">' + response[i].userName + '</option>';
            }

            $("#moderator-selector").html(s);

        }).fail(function (response) {

        });
});

$('#remove-moderator-global-button').click(function (event) {
    $.get("/Admin/BusinessUnits/GetAllModerators")
        .done(function (response) {
            var s = '<option value="-1" selected disabled hidden>Please Select Moderator</option>';
            for (var i = 0; i < response.length; i++) {
                s += '<option value="' + response[i].id + '">' + response[i].userName + '</option>';
            }

            $("#moderator-selector-remove").html(s);

        }).fail(function (response) {

        });
});

$('#add-business-unit-global-button').click(function (event) {
    $.get("/Admin/BusinessUnits/GetAllBusinessUnitCategories")
        .done(function (response) {
            var s = '<option value="-1" selected disabled hidden>Please Select Category</option>';
            for (var i = 0; i < response.length; i++) {
                s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
            }

            $("#business-unit-categories-selector").html(s);

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

    $.get("/Admin/BusinessUnits/GetAllTowns")
        .done(function (response) {
            var s = '<option value="-1" selected disabled hidden>Select City</option>';
            for (var i = 0; i < response.length; i++) {
                s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
            }

            $("#towns-selector").html(s);

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

$(function () {
    const $submitForm = $('#submit-form-add-moderator');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
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

            $('#myModalAddModerator').modal('hide');
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
    const $submitForm = $('#submit-form-remove-moderator');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
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

            $('#myModalAddModerator').modal('hide');
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
