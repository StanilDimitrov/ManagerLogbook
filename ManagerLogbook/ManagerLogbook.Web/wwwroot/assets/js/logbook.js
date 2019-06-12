
$('#add-manager-global-button').click(function (event) {

    var btn = $("#logbook-id").attr('value');

    $.get("/Admin/Logbooks/GetAllManagersNotPresent/" + btn)

        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Manager</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].userName + '</option>';
                }
            }
            else {
                s += '<option value="null">No managers available</option>';
            }

            $("#manager-selector").html(s);

        }).fail(function (response) {

        });
});

$('#remove-manager-global-button').click(function (event) {

    var btn = $("#logbook-id-remove").attr('value');
    
    $.get("/Admin/Logbooks/GetAllManagersPresent/" + btn)

        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Manager</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].userName + '</option>';
                }
            }
            else {
                s += '<option value="null">No managers available</option>';
            }
            
            $("#manager-selector-remove").html(s);

        }).fail(function (response) {

        });
});

$(function () {
    const $submitForm = $('#submit-form-add-manager');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        var inputs = $this.find('input');
        var managerId = $('#manager-selector').val();
        var logbookId = $('#logbook-id').val();
       
        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&ManagerId=" + managerId + "&Id=" + logbookId;
       
        var url = $this.attr('action');

        $.post(url, inputsToSend, function (response) {
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

            $('#myModalAddManager').modal('hide');
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
    const $submitForm = $('#submit-form-remove-manager');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        var inputs = $this.find('input');
        var managerId = $('#manager-selector-remove').val();
        var logbookId = $('#logbook-id-remove').val();

        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&ManagerId=" + managerId + "&Id=" + logbookId;

        var url = $this.attr('action');

        $.post(url, inputsToSend, function (response) {
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

            $('#myModalRemoveManager').modal('hide');
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

$('#update-logbook-global-button').click(function (event) {

    $this = $(this);

    $.get("/Admin/BusinessUnits/GetAllBusinessUnits/")
        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Business unit</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                }
            }
            else {
                s += '<option value="null">No business units available</option>';
            }
            
            $("#business-unit-update-selector").html(s);

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

$('#create-logbook-global-button').click(function (event) {

    $this = $(this);

    $.get("/Admin/BusinessUnits/GetAllBusinessUnits/")
        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Business unit</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                }
            }
            else {
                s += '<option value="null">No business units available</option>';
            }
            
            $("#business-unit-create-selector").html(s);

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
    const $submitForm = $('#create-logbook');

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
            $('#myModalCreateLogbook').modal('hide');
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
    const $submitForm = $('#update-logbook');

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
            $('#myModalUpdateLogbook').modal('hide');
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
