  $(function () {
        const $submitForm = $('#b-note');

        //if ($submitForm.valid()) {

        $submitForm.on('submit', function (event2) {
        event2.preventDefault();

        var $this = $(this);

        var inputs = $submitForm.find('input');
        var dataToSend = inputs.serialize();

        var businessId = parseInt($('#business-unit-id-input')[0].value);
        var reviewDescription = $('#reviewDescription')[0].value;
        
        finalDataToSend = dataToSend + "&BusinessUnitId="+businessId + "&OriginalDescription="+reviewDescription;

        //console.log(reviewDescription);
        //debugger;

        var url = "/Reviews/Create/";

            //console.log();
            //debugger;

        $.post(url, finalDataToSend, function (response) {

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

        //}

        });