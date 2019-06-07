$(function () {

    $("#myNoteModal").validate({
        rules: {
            pName: {
                required: true,
                minlength: 8
            },
            action: "required"
        },
        messages: {
            pName: {
                required: "Please enter some data",
                minlength: "Your data must be at least 8 characters"
            },
            action: "Please provide some data"
        }
    });
});