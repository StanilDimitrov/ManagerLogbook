
$(function () {
    const $submitForm = $('#submit-form');

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
    const $submitForm = $('#submit-form-account');

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
                $('#myModalRegister').modal('hide');
                toastr.success(data);
            },

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
    const $submitForm = $('#deactivate-status');

    $submitForm.on('click', function (event) {
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

//document.getElementById("#create-note")[0].reset();

//$("a").click(function () {
//    //get data from link
//    var noteId = $(this).data('noteid');
//    var modalFormAction = $('#editModalNote').find('form').attr('action');

//    // update modal with new data
//    var actionWithId = (modalFormAction + '/' + noteId).replace(/[0-9]/g, '');
//        actionWithId = modalFormAction + '/' + noteId;
//    // find textarea in modal
//    // todo by stanil

//    // update textarea with description
//    // todo by stanil
//    $('#editModalNote').find('form').attr('action', actionWithId);


//    var modalSelected = '#editModalNote';
//});

// Resets modal
$('#myModalRegister').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$('#myModalLogin').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})


$('#myModalNote').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$('#editModalNote').on('hidden.bs.modal', function () {
})

$('#logbookModalNote').on('hidden.bs.modal', function () {
})

$('#myModalBusinessUnit').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$('#myModalBusinessUnit').on('show.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$('input.myTextInput').on('input', function (e) {
    alert('Changed!')
});



var lastKeypress = null;
$('#search-business-form').on('submit', function (eventSearchBusiness) {
    eventSearchBusiness.preventDefault();
    //console.log(eventSearchBusiness);

    var currentKeypress = new Date();
    var searchPhrase = $('#search-for-business-phrase').val();

    setTimeout(function () {


    }, 500);

    var shouldRequest = // check if at least 500 ms have passed
        eventSearchBusiness.type === 'submit' ||
        (eventSearchBusiness.type === 'input' && searchPhrase.length > 2);

    lastKeypress = currentKeypress;
    if (shouldRequest) {
        $this = $(this);

        var inputs = $this.find('input');

        var categoryId = $('#category-id-from-selector').val();
        var townId = $('#town-id-from-selector').val();

        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&CategoryId=" + categoryId + "&TownId=" + townId;
        //console.log(inputs);
        //debugger;

        $.get("/Home/Search", inputsToSend)
            .done(function (data) {

                //console.log(data);
                //debugger;

                $('#business-partial-holder').empty();
                $('#business-partial-holder').append(data);

                //shortenTextFunction();

            }).fail(function (data) {
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

            });
    }
});

function test(id) {

    $.get("/Manager/Notes/Deactivate/" + id)
        .done(function (dataAll) {

            $('#note-partial-holder').empty();

            $('#note-partial-holder').append(dataAll);

            shortenTextFunction();
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

$('#search-criterias-holder').on('click', '#deactivate-note', function (eventDeactivateNote) {

    eventDeactivateNote.preventDefault();

    $this = $(this);
    var btn = $(this).attr('value');

    $.get("/Manager/Notes/Deactivate/" + btn)
        .done(function (dataAll) {

            $('#note-partial-holder').empty();

            $('#note-partial-holder').append(dataAll);

            shortenTextFunction();
            showImage();

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
});

$("#myModalRegister").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    //console.log($(this).find('.field-validation-error'));
});

$("#myModalNote").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    $(this).find('#image-upload').empty();
    //console.log($(this).find('.field-validation-error'));
});

$("#myEditModalNote").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    //console.log($(this).find('.field-validation-error'));
});

$("#myModalBusinessUnit").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    //console.log($(this).find('.field-validation-error'));
});

$("#myModalUpdateBusinessUnit").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    console.log($(this).find('.field-validation-error'));
});

dataLoading = false;
var scrollPageCount = 2;

function shortenTextFunction() {
    $(".comment").shorten({
        "showChars": 100,
        "moreText": "More",
        "lessText": "Less",
    });
};

function showImage() {
    $('.image-link').magnificPopup({ type: 'image' });
};

function centerModal() {
    $(this).css('display', 'block');
    var $dialog = $(this).find(".modal-dialog");
    var offset = ($(window).height() - $dialog.height()) / 2;
    // Center modal vertically in window
    $dialog.css("margin-top", offset);
}

$('.modalModalImage').on('show.bs.modal', centerModal);
$(window).on("resize", function () {
    $('.modalModalImage:visible').each(centerModal);
});




