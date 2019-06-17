$(function () {
    const $submitForm = $('#submit-review');

    $submitForm.on('submit', function (event2) {
        event2.preventDefault();

        var $this = $(this);

        var inputs = $submitForm.find('input');
        var dataToSend = inputs.serialize();

        var businessId = parseInt($('#business-unit-id-input')[0].value);
        var reviewDescription = $('#reviewDescription')[0].value;

        finalDataToSend = dataToSend + "&BusinessUnitId=" + businessId + "&OriginalDescription=" + reviewDescription;

        var url = "/Reviews/Create/";

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

$('.submit-edit-review-form-button').click(function (eventEdit) {

    eventEdit.preventDefault();
    $thisEditForm = $(this);
    var id = $thisEditForm.attr('reviewid');
    var concreteFormOfClickedButton = $('#editModalReview-' + id);
    var inputs = concreteFormOfClickedButton.find('input');
    var originalDescription = $('#original-description').val();
  
    
    var businessUnitId = $('#business-unit-review-id').val();
    var antiForgery = $('#token-review-id').find('input');
    var idOfForm = "#submit-form-" + id;
    var formInputs = $(idOfForm);
    var formInputs2 = formInputs[0];
    var token = formInputs2[5];

    var urlToEdit = "/Moderator/Reviews/Update";
    var concreteFormOfClickedButton = $('#editModalReview-' + id);
   
    var textarea = concreteFormOfClickedButton.find('textarea');
    var textFromDescription = textarea[0].value;
 
    var inputs = "Id=" + id + "&EditedDescription=" + textFromDescription + "&OriginalDescription=" + originalDescription + "&BusinessUnitId=" + businessUnitId + "&" + token.name + "=" + token.value;

    $.post(urlToEdit, inputs, function (response) {
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


    //console.log(inputs);

    //var fdata = new FormData();
    //fdata.append("Id", id);
    //fdata.append("EditedDescription", textFromDescription);
    //fdata.append("OriginalDescription", originalDescription);
    //fdata.append("BusinessUnitId", businessUnitId);
    //fdata.append(token.name, token.value);

    
      

    //$.ajax({
    //    type: 'post',
    //    url: urlToEdit,
    //    data: fdata,
    //    processData: false,
    //    contentType: false,
    //}).done(function (response) {

    //    toastr.options =
    //        {
    //            "debug": false,
    //            "positionClass": "toast-top-center",
    //            "onclick": null,
    //            "fadeIn": 300,
    //            "fadeOut": 1000,
    //            "timeOut": 3000,
    //            "extendedTimeOut": 3000,
    //            "onHidden": function () {
    //            }
    //        }
    //    $('#editModalReview-' + id).modal('hide');
    //    window.location = "/Moderator/Reviews/Index/";
    //    toastr.success(response);

    //    //window.setTimeout(function () { location.reload() }, 1500);
    //    //location.reload();
    //}).fail(function (response) {
    //    toastr.options =
    //        {
    //            "debug": false,
    //            "positionClass": "toast-top-center",
    //            "onclick": null,
    //            "fadeIn": 300,
    //            "fadeOut": 1000,
    //            "timeOut": 3000,
    //            "extendedTimeOut": 3000
    //        }
    //    toastr.error(response.responseText);
    //});
//});

function test1(id) {

    $.get("/Moderator/Reviews/Deactivate/" + id)
        .done(function (dataAll) {
            debugger;
            $('#review-partial-holder').empty();

            $('#review-partial-holder').append(dataAll);

            //shortenTextFunction();
            //showImage();

        }).fail(function (dataAll) {
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
            toastr.error(dataAll.responseText);
        });
}


$(function () {
    const $submitForm = $('#disable-review');

    $submitForm.on('click', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        var url = $this.attr('action');

        $.post(url, dataToSend, function (response) {
            //console.log(response.firstName);
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
            window.location = "/Moderator/Reviews/Index/";
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
