
$(function () {
    const $submitForm = $('#submit-form');

    $submitForm.on('submit', function (event) {
        event.preventDefault();
        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        //var url = "/Admin/Create/Register";
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

        const dataToSend = $submitForm.serialize();
        //var url = "/Admin/Create/Register";
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

            $('#myModalRegister').modal('hide');
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
    const $submitForm = $('#deactivate-status');

    $submitForm.on('click', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        //var url = "/Admin/Create/Register";
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
$('#search-business-form').on('input submit', function (eventSearchBusiness) {
    eventSearchBusiness.preventDefault();
    console.log(eventSearchBusiness);

    var currentKeypress = new Date();
    var searchPhrase = $('#search-for-business-phrase').val();

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


$('#search-criterias-holder').on('click', '#search-notes-7-days', function (eventSearchNotes) {
    eventSearchNotes.preventDefault();

    $this = $(this);
    var btn = $(this).attr('value');
    //var clickedBtnID = $(this).attr('id');

    //console.log(btn);

    $.post("/Manager/Notes/NotesForDaysBefore/" + btn)
        .done(function (dataFrom7Days) {

            $('#note-partial-holder').empty();

            $('#note-partial-holder').append(dataFrom7Days);

            shortenTextFunction();
            showImage();

        }).fail(function (dataFrom7Days) {
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
            toastr.error(dataFrom7Days.responseText);

        });
});

$('#search-criterias-holder').on('click', '#search-notes-30-days', function (eventSearchNotes) {
    eventSearchNotes.preventDefault();

    $this = $(this);
    var btn = $(this).attr('value');
    //var clickedBtnID = $(this).attr('id');

    //console.log(btn);

    $.post("/Manager/Notes/NotesForDaysBefore/" + btn)
        .done(function (dataFrom30Days) {

            $('#note-partial-holder').empty();

            $('#note-partial-holder').append(dataFrom30Days);

            shortenTextFunction();
            showImage();

        }).fail(function (dataFrom30Days) {
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
            toastr.error(dataFrom30Days.responseText);

        });
});

$('#search-criterias-holder').on('click', '#search-notes-active', function (eventSearchNotes) {
    eventSearchNotes.preventDefault();


    $.get("/Manager/Notes/ActiveNotes/")
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

$('#search-criterias-holder').on('click', '#search-notes-all', function (eventSearchNotes) {
    eventSearchNotes.preventDefault();

    $this = $(this);
    var btn = $(this).attr('value');

    $.post("/Manager/Notes/NotesForDaysBefore/" + btn)
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

$('#search-criterias-holder').on('click', '#search-notes-today', function (eventSearchNotes) {
    eventSearchNotes.preventDefault();

    $this = $(this);
    var btn = $(this).attr('value');

    $.post("/Manager/Notes/NotesForDaysBefore/" + btn)
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
    console.log($(this).find('.field-validation-error'));
});

$("#myModalBusinessUnit").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    console.log($(this).find('.field-validation-error'));
});

$("#myModalBusinessUnit").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    console.log($(this).find('.field-validation-error'));
});

$("#myModalUpdateBusinessUnit").on('show.bs.modal', function () {
    $(this).find('.text-danger-custom').empty();
    console.log($(this).find('.field-validation-error'));
});

$("#note-partial-holder").on('scroll',  function (someEvent)
{
    debugger;
    //if ($(window).scrollTop() + $(window).height() > $(document).height() - 100)
    //{    
        setTimeout(2000);

        var searchFormToGetParams = $('#search-notes-form');
        var searchPhrase = $('#search-for-note-phrase').val();
        var inputs = searchFormToGetParams.find('input');
        var categoryId = $('#category-id-from-selector').val();
        var urlencodedInputs = inputs.serialize();
        var scrollPageCount = parseInt($('#scroll-page-counter').value);
        var inputsToSend = urlencodedInputs + "&CategoryId=" + categoryId + "&ScrollPage=" + scrollPageCount;

       $.post("/Manager/Notes/SearchNotesScrollResult", inputsToSend)
           .done(function (dataFromScrollSearch)
           {
               debugger
               $('#addintional-notes-scroll').append(dataFromScrollSearch);
               //totalPagesForScrollSearch += 1;

               //$('#addintional-notes-scroll').append("MAIKA MU DEIBA sqlAAAAAAAAAAAAAAAA");

               //$("#addintional-notes-scroll").append($("<li>DADSADASA</li>").html('something')
               //    .addClass('myclass'));


           }).fail(function (dataFromScrollSearch)
           {

       });

    //}
});


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

