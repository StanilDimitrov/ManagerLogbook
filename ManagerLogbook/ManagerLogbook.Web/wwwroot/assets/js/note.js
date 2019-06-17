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
        toastr.success(response);
        $('#editModalNote-' + id).modal('hide');
        window.location = "/Manager/Notes/Index/";

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

$(function () {
    const $submitForm = $('#create-note');

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
                $('#myModalNote').modal('hide');
                //$("#create-note")[0].reset();
                //$(this).find('form')[0].reset();
                //$(this).find('#blah').empty();
               
                toastr.success(data);
                $(this).find('#clear-image').empty();
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

//$(function () {
//    const $submitForm = $('#create-note');

//    //if ($submitForm.valid()) {

//    $submitForm.on('submit', function (event) {
//        event.preventDefault();

//        var $this = $(this);
//        var inputs = $this.find('input');


//        var formData = new FormData(this);

//        $.ajax({
//            type: 'POST',
//            url: $(this).attr('action'),
//            data: formData,
//            cache: false,
//            contentType: false,
//            processData: false,
//            success: function (data) {
//                toastr.options = {
//                    "debug": false,
//                    "positionClass": "toast-top-center",
//                    "onclick": null,
//                    "fadeIn": 300,
//                    "fadeOut": 1000,
//                    "timeOut": 3000,
//                    "extendedTimeOut": 3000,
//                    "closeButton": true
//                }
//                $('#myModalNote').modal('hide');
//                toastr.success(data);
//            },
//            error: function (data) {
//                toastr.options = {
//                    "debug": false,
//                    "positionClass": "toast-top-center",
//                    "onclick": null,
//                    "fadeIn": 300,
//                    "fadeOut": 1000,
//                    "timeOut": 3000,
//                    "extendedTimeOut": 3000,
//                    "closeButton": true
//                }
//                toastr.error(data.responseText);
//            }
//        });

//    });


    //var image = $('#image-input-create-note').val();
    //var file = $(image)[0];
    //var fileInput = file.files[0];

    // var urlencodedInputs = inputs.serialize();

    //var inputsToSend = urlencodedInputs + "&NoteImage=" + image;

    //var $this = $(this);

    //const dataToSend = $submitForm.serialize();

    //var url = "/Admin/Create/Register";
    //var url = $this.attr('action');

    //    var $this = $(this);

    //    const dataToSend = $submitForm.serialize();

    //    var url = $this.attr('action');

    //    $.post(url, formData, function (response) {

    //        toastr.options = {
    //            "debug": false,
    //            "positionClass": "toast-top-center",
    //            "onclick": null,
    //            "fadeIn": 300,
    //            "fadeOut": 1000,
    //            "timeOut": 3000,
    //            "extendedTimeOut": 3000,
    //            "closeButton": true
    //        }
    //        $('#myModalNote').modal('hide');
    //        toastr.success(response);

    //    }).fail(function (response) {
    //        toastr.options = {
    //            "debug": false,
    //            "positionClass": "toast-top-center",
    //            "onclick": null,
    //            "fadeIn": 300,
    //            "fadeOut": 1000,
    //            "timeOut": 3000,
    //            "extendedTimeOut": 3000,
    //            "closeButton": true
    //        }
    //        toastr.error(response.responseText);
    //    });
    //});




    //$(function () {
    //    const $submitForm = $('#submit-form-logbook');

    //    $submitForm.on('submit', function (event) {
    //        event.preventDefault();

    //        var $this = $(this);

    //        const dataToSend = $submitForm.serialize();

    //        //var url = "/Manager/Users/SwitchLogbook/";
    //        var url = $this.attr('action');

    //        $.post(url, dataToSend, function (response) {
    //            //console.log(dataToSend);

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

    //            toastr.success(response);
    //            $('#myModalLogbook').modal('hide');

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


    $('#add-note-global-button').click(function (event) {
        $.get("/Manager/Notes/GetAllNoteCategories")
            .done(function (response) {
                var s = '<option value="null" selected disabled hidden>Please Select Category</option>';
                if (response.length) {
                    for (var i = 0; i < response.length; i++) {
                        s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                    }
                }
                else {
                    s += '<option value="null">No categories available</option>';
                }

                $("#notes-categories-selector").html(s);

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

    

$('#ordersTable').on('click', '#edit-note-global-button', function (event) {

    var $this = $(this);
    var idForNote = parseInt($this.attr('data-noteid'));
    


        $.get("/Manager/Notes/GetAllNoteCategories")
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
                
           

                

              
                var finalSelector = "#notes-categories-selector-edit-" + idForNote;


                console.log(finalSelector);
                debugger;


                $(finalSelector).html(s);

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

$(document).ready(function () {
    $('.image-link').magnificPopup({ type: 'image' });
});

//$(document).ready(function () {
//    $('.image-link').magnificPopup({ type: 'image' });
//});


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

    $('#search-criterias-holder').on('click', '#search-notes-7-days', function (eventSearchNotes) {
        eventSearchNotes.preventDefault();

        $this = $(this);
        var btn = $(this).attr('value');

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

    $('#search-criterias-holder').on('click', '#search-notes-active', function (eventSearchNotesActive) {
        eventSearchNotesActive.preventDefault();

        $.get("/Manager/Notes/ActiveNotes")
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


    $("#note-partial-holder").on('click', '.note-pagination-button-table', function (someEvent) {

        var $this = $(this)

        var currPage = parseInt($this.attr('at'));

        var searchFormToGetParams = $('#search-notes-form');
        var searchPhrase = $('#search-for-note-phrase').val();
        var inputs = searchFormToGetParams.find('input');
        var categoryId = $('#category-id-from-selector').val();
        var urlencodedInputs = inputs.serialize();
        var daysBefore = $('.search-notes-common-days');
        var days = parseInt(daysBefore.value);

        var inputsToSend = urlencodedInputs + "&CategoryId=" + categoryId + "&CurrPage=" + currPage + "&DaysBefore=" + days;
        dataLoading = true;
        $.post("/Manager/Notes/GetNotesInPage", inputsToSend)
            .done(function (dataFromScrollSearch) {

                $('#note-partial-holder').empty();
                $('#note-partial-holder').append(dataFromScrollSearch);
                shortenTextFunction();
                showImage();
                var form = document.getElementById("search-notes-form");
                form.reset();

            }).fail(function (dataFromScrollSearch) {

            });

    });

    var lastKeypress = null;
    $('#search-notes-form').on('submit', function (eventSearchNote) {
        eventSearchNote.preventDefault();
        var searchPhrase = $('#search-for-note-phrase').val();
        debugger
        //var currentKeypress = new Date();


        var shouldRequest = // check if at least 500 ms have passed
            eventSearchNote.type === 'submit' ||
            (eventSearchNote.type === 'input' && searchPhrase.length > 2);

        //lastKeypress = currentKeypress;


        //if (searchPhrase.length > 2) {

        //if (shouldRequest) {
        $this = $(this);

        var inputs = $this.find('input');
        var categoryId = $('#category-id-from-selector').val();
        debugger
        var currentPage = $('#current-page-hidden-input').val();
        var totalPages = $('#total-pages-hidden-input').val();

        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&CategoryId=" + categoryId + "&CurrPage=" + currentPage + "&TotalPages=" + totalPages;


        $.post("/Manager/Notes/Search", inputsToSend)
            .done(function (data) {

                $('#note-partial-holder').empty();
                $('#note-partial-holder').append(data);
                shortenTextFunction();
                showImage();
                var form = document.getElementById("search-notes-form");
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
        //}
    });


