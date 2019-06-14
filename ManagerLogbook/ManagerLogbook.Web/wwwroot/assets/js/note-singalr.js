//await hubContext.Clients.All.SendAsync("Index");


//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/NoteHub")
//    .build();

//debugger;

//connection.start().then(function () {
//    document.getElementById("AddComment").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});


//connection.on("Index", function () {

//    var url = "/Manager/Notes/Index/"
//    //var id = logbookid
//    //console.log(logbookid);
//    //debugger;
//    $.ajax({
//        type: 'Get',
//        url: url,
//        data: id,
//        // processData: false,
//        success: function (result) {
//            $("#note-partial-holder").empty();
//            $("#note-partial-holder").append(result);
//        },
//    }) 
//});