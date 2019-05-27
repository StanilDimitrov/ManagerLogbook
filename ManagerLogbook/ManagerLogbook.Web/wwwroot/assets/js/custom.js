

$(function () {
    const $submitForm = $('#submit-form');

    $submitForm.on('submit', function (event) {
        event.preventDefault();

        const dataToSend = $submitForm.serialize();
        var url = "/Admin/Create/Register";

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

$(".rename").click(function (e) {
    e.preventDefault();
    var $this = $(this);
    var fileName = $(this).data("file");
    $("#basicModal").data("fileName", fileName).modal("toggle", $this);

});

$("#basicModal").on("shown.bs.modal", function (e) {
    //data-fileName attribute associated with the modal added in the click event of the button
    alert($(this).data("fileName"));
    //my data-file associated with the button 
    alert($(e.relatedTarget).data("file"));
})

