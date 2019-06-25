
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


$('#search-business-form').on('submit', function (eventSearchBusiness) {
    eventSearchBusiness.preventDefault();

    //var currentKeypress = new Date();
    var searchPhrase = $('#search-for-business-phrase').val();

    //var shouldRequest = // check if at least 500 ms have passed
    //    eventSearchBusiness.type === 'submit' ||
    //    (eventSearchBusiness.type === 'input' && searchPhrase.length > 2);

    //lastKeypress = currentKeypress;
   
        $this = $(this);

        var inputs = $this.find('input');

        var categoryId = $('#category-id-from-selector').val();
        var townId = $('#town-id-from-selector').val();

        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&CategoryId=" + categoryId + "&TownId=" + townId;

        $.get("/Home/Search", inputsToSend)
            .done(function (data) {

                $('#business-partial-holder').empty();
                $('#business-partial-holder').append(data);
                var form = document.getElementById("search-business-form");
                form.reset();

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
});

function test(id) {

    $.get("/Manager/Notes/Deactivate/" + id)
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
}

//$('#search-criterias-holder').on('click', '#deactivate-note', function (eventDeactivateNote) {

//    eventDeactivateNote.preventDefault();

//    $this = $(this);
//    var btn = $(this).attr('value');

//    $.get("/Manager/Notes/Deactivate/" + btn)
//        .done(function (dataAll) {

//            $('#note-partial-holder').empty();

//            $('#note-partial-holder').append(dataAll);

//            shortenTextFunction();
//            showImage();

//        }).fail(function (dataAll) {
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
//            toastr.error(dataAll.responseText);
//        });
//});

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

$("#myModalNote").on('show.bs.modal', function () {
    $(this).find('.preview1').empty();
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



function readURL(input, imgControlName) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(imgControlName).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}


$('.blabla').click(function (eventForPicUpload)
{
    eventForPicUpload.preventDefault();

    $this = $(this);

    var idForNotePic = $this.attr('note-id');

    $("#image-input-" + idForNotePic).change(function () {
        var imgControlName = "#ImgPreview-" + idForNotePic;
        readURL(this, imgControlName);
        $('.preview1').addClass('it');
        $('.btn-rmv1').addClass('rmv').attr('note-id', idForNotePic);

        $('.rmv').click(function (rmv) {
            rmv.preventDefault();
            $("#image-input-" + idForNotePic).val("");
            $("#ImgPreview-" + idForNotePic).attr("src", "");
            $('.preview1').removeClass('it');
            $('.btn-rmv1').removeClass('rmv');
        });        
    });

    $('#editModalNote-' + idForNotePic).on('hidden.bs.modal', function () {
        $("#image-input-" + idForNotePic).val("");
        $("#ImgPreview-" + idForNotePic).attr("src", "");
        $('.preview1').removeClass('it');
        $('.btn-rmv1').removeClass('rmv');
    });   
});
















$("#imag").change(function () {
    var imgControlName = "#ImgPreview";
    readURL(this, imgControlName);
    $('.preview1').addClass('it');
    $('.btn-rmv1').addClass('rmv');
});

$("#removeImage1").click(function (e) {
    e.preventDefault();
    $("#imag").val("");
    $("#ImgPreview").attr("src", "");
    $('.preview1').removeClass('it');
    $('.btn-rmv1').removeClass('rmv');
});

$('#myModalNote').on('hidden.bs.modal', function () {
    $("#imag").val("");
    $("#ImgPreview").attr("src", "");
    $('.preview1').removeClass('it');
    $('.btn-rmv1').removeClass('rmv');
});


$("#imag-create-account").change(function () {
    var imgControlName = "#ImgPreview-create-account";
    readURL(this, imgControlName);
    $('.preview1').addClass('it');
    $('.btn-rmv1').addClass('rmv');
});

$("#removeImage1-create-account").click(function (e) {
    e.preventDefault();
    $("#imag-create-account").val("");
    $("#ImgPreview-create-account").attr("src", "");
    $('.preview1').removeClass('it');
    $('.btn-rmv1').removeClass('rmv');
});

$('#myModalRegister').on('hidden.bs.modal', function () {
    $("#imag-create-account").val("");
    $("#ImgPreview-create-account").attr("src", "");
    $('.preview1').removeClass('it');
    $('.btn-rmv1').removeClass('rmv');
});

