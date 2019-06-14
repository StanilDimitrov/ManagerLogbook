$(function () {
    const $submitForm = $('#submit-review');

    //if ($submitForm.valid()) {

    $submitForm.on('submit', function (event2) {
        event2.preventDefault();

        var $this = $(this);

        var inputs = $submitForm.find('input');
        var dataToSend = inputs.serialize();

        var businessId = parseInt($('#business-unit-id-input')[0].value);
        var reviewDescription = $('#reviewDescription')[0].value;

        finalDataToSend = dataToSend + "&BusinessUnitId=" + businessId + "&OriginalDescription=" + reviewDescription;

        //console.log(reviewDescription);
        //debugger;

        var url = "/Reviews/Create/";

        //console.log();
        //debugger;

        $.post(url, finalDataToSend).done(function (response) {

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

            var getReviewsUrl = "/BusinessUnits/GetReviewsList?id=" + businessId;

            $.get(getReviewsUrl).done(function (reviewData) {
                $('#reviews-list').empty();

                console.log(reviewData);
                debugger;

                $('#reviews-list').append(reviewData);

                $("#submit-review")[0].reset();
            }).fail(function (reviewData) {

            })


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


$('.submit-edit-form-button').click(function (eventEdit) {

    eventEdit.preventDefault();
    $thisEditForm = $(this);
    var id = $thisEditForm.attr('noteid');

    var urlToEdit = "/Manager/Notes/Edit";
    var concreteFormOfClickedButton = $('#editModalNote-' + id);
    var inputs = concreteFormOfClickedButton.find('input');
    var textarea = concreteFormOfClickedButton.find('textarea');
    var textFromDescription = textarea[0].value;
    var image = inputs[1];
    var token = inputs[2];
    var imageInputIdSelector = "#image-input-" + id;

    var file = $(imageInputIdSelector)[0];
    var fileInput = file.files[0];

    var fdata = new FormData();
    fdata.append("Id", id);
    fdata.append("Description", textFromDescription);
    fdata.append("NoteImage", fileInput);
    fdata.append(token.name, token.value);

    $.ajax({
        type: 'post',
        url: urlToEdit,
        data: fdata,
        processData: false,
        contentType: false,
    }).done(function (response) {

        toastr.options =
            {
                "debug": false,
                "positionClass": "toast-top-center",
                "onclick": null,
                "fadeIn": 300,
                "fadeOut": 1000,
                "timeOut": 3000,
                "extendedTimeOut": 3000,
                "onHidden": function () {
                }
            }
        $('#editModalNote -' + id).modal('hide');
        toastr.success(response);

        //window.setTimeout(function () { location.reload() }, 1500);
        //location.reload();
    }).fail(function (response) {
        toastr.options =
            {
                "debug": false,
                "positionClass": "toast-top-center",
                "onclick": null,
                "fadeIn": 300,
                "fadeOut": 1000,
                "timeOut": 3000,
                "extendedTimeOut": 3000
            }
        toastr.error(response.responseText);
    });
});
