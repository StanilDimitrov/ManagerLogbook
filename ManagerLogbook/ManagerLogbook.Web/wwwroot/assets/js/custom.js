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
                    // this will be executed after fadeout, i.e. 2secs after notification has been show
                    //window.location.href = "/AppUsers/Transactions/GetTransaction?=" + transactionId;
                }
            }
        toastr.success(response);
        window.setTimeout(function () { location.reload() }, 1500);
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
    const $submitForm = $('#submit-form');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        //var url = "/Admin/Create/Register";
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
    const $submitForm = $('#create-note');

    //if ($submitForm.valid()) {

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);

        const dataToSend = $submitForm.serialize();
        //var url = "/Admin/Create/Register";
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

        $('#myModalNote').modal('hide');

    });

    //}

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


$('#add-note-global-button').click(function (event)
{
    $.get("/Manager/Notes/GetAllNoteCategories")
        .done(function (response)
        {          
            var s = '<option value="-1" selected disabled hidden>Please Select Category</option>';
            for (var i = 0; i < response.length; i++)
            {
                s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
            }
            
            $("#notes-categories-selector").html(s); 

        }).fail(function (response)
        {
            
        });
});

$('#change-logbook-global-button').click(function (event) {
    $.get("/Manager/Notes/GetAllLogbooksByUser")
        .done(function (response) {
            var s = '<option value="-1" selected disabled hidden>Please Select Logbook</option>';
            for (var i = 0; i < response.length; i++) {
                s += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
            }

            $("#logbooks-selector").html(s);

        }).fail(function (response) {

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

$('#myModalNote').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$('#editModalNote').on('hidden.bs.modal', function () {
})

$('#logbookModalNote').on('hidden.bs.modal', function () {
})

$('input.myTextInput').on('input', function (e) {
    alert('Changed!')
});

$('#search-notes-form').on('input submit', function (eventSearchNote)
{
    eventSearchNote.preventDefault();

    var searchPhrase = $('#search-for-note-phrase').val();

    if (searchPhrase.length > 2)
    {
        
        $this = $(this);

        var inputs = $this.find('input');
        var categoryId = $('#category-id-from-selector').val();

        var urlencodedInputs = inputs.serialize();

        var inputsToSend = urlencodedInputs + "&CategoryId=" + categoryId;


        $.post("/Manager/Notes/Search", inputsToSend)
            .done(function (data) {

                console.log(data);
                

                $('#note-partial-holder').empty();
                $('#note-partial-holder').append(data);


            }).fail(function (data) {

            });
    }   

});

