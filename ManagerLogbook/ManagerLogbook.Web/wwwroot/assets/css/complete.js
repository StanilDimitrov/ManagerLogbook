$(function () {
    $('#autocomplete-search-business-unit').autocomplete({
        source: function (request, responce) {
            $.ajax({
                url: '@Url.Action("Complete", "Home")',
                type: "POST",
                datatype: "json",
                data: { currentInput: request.term },
                //data: { search: $("#autocomplete-payee-account").val() },
                //data: { prefix: request.term },
                //contentType: "application/json; charset=utf-8",
                success: function (data) {
                    responce($.map(data, function (item) {
                        //return item;
                        return { label: item.Name, value: item.Id };
                    }))
                },
            });
        },
        minLength: 3,
        messages: {
            noResults: "Such account does not exist.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }
    });
});