$(function () {
    $('#autocomplete-search-business-unit').autocomplete({
        source: function (request, responce) {
            $.ajax({
                url: "/Home/Complete/",
                type: "GET",
                datatype: "json",
                data: { currentInput: request.term },

                success: function (data) {
                    responce($.map(data, function (item) {
                        //return item;
                        return { label: item.Name, value: item.Name };
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