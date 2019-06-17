$(function () {
    const $submitForm = $('#create-business-unit');
        $(this).find('form').trigger('reset');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        var inputs = $this.find('input');
        var formData = new FormData(this);

        $.ajax({
            type: 'POST',
            url: $(this).attr('action'),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
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
                toastr.success(data);
            },
            error: function (data) {
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
                toastr.error(data.responseText);
            }
        });
       
    });
});

$(function () {
    const $submitForm = $('#update-business-unit');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        var btn = $("#update-business-unit-id").attr('value');
        var $this = $(this);
        var inputs = $this.find('input');
        var formData = new FormData(this);

        $.ajax({
            type: 'POST',
            url: $(this).attr('action'),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
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
                window.location = "/BusinessUnits/Details/" + btn;
                toastr.success(data);
            },
            error: function (data) {
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
                toastr.error(data.responseText);
            }
        });

    });
});

$('#add-moderator-global-button').click(function (event) {

    var btn = $("#my-add-moderator-value").attr('value');

    $.get("/Admin/BusinessUnits/GetAllModeratorsNotPresent/" + btn)
  
        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Moderator</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].userName + '</option>';
                }
            }
            else {
                s += '<option value="null">No moderators available</option>';
            }
            

            $("#moderator-selector").html(s);

        }).fail(function (response) {

        });
});

$('#remove-moderator-global-button').click(function (event) {

    $this = $(this);
  
    var btn = $("#my-remove-moderator-value").attr('value');
    
    $.get("/Admin/BusinessUnits/GetAllModeratorsPresent/" + btn)
        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Moderator</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].userName + '</option>';
                }
            }
            else {
                s += '<option value="null">No moderators available</option>';
            }
            $("#moderator-selector-remove").html(s);

        }).fail(function (response) {

        });
});

$('#add-business-unit-global-button').click(function (event) {

    $this = $(this);

    var btn = $("#my-add-moderator-value").attr('value');

    $.get("/Admin/BusinessUnits/GetAllBusinessUnitCategories/" + btn)
        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Category</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                }
            }
            else {
                s += '<option value="null">No categories available</option>';
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
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                }
            }
            else {
                s += '<option value="null">No cities available</option>';
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

$('#update-global-button').click(function (event) {

    $this = $(this);

    $.get("/Admin/BusinessUnits/GetAllBusinessUnitCategories/")
        .done(function (response) {
            var s = '<option value="null" selected disabled hidden>Please Select Category</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                }
            }
            else {
                s += '<option value="null">No categories available</option>';
            }
          
            $("#business-unit-categories-selector-update").html(s);

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
            var s = '<option value="null" selected disabled hidden>Select City</option>';
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                }
            }
            else {
                s += '<option value="null">No cities available</option>';
            }
            

            $("#towns-selector-update").html(s);

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
        var inputs = $this.find('input');
        var moderatorId = $('#moderator-selector').val();
        var businessUnitId = $('#my-add-moderator-value').val();
        
        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&ModeratorId=" + moderatorId + "&Id=" + businessUnitId;

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
        var inputs = $this.find('input');
        var moderatorId = $('#moderator-selector-remove').val();
        var businessUnitId = $('#my-remove-moderator-value').val();

        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&ModeratorId=" + moderatorId + "&Id=" + businessUnitId;
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

            $('#myModalRemoveModerator').modal('hide');
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
