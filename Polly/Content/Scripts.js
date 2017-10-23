$(function () {
    // vote!

    $(".votes").on("click", ".vote", function () {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var data = { Answer: $(this).data("id") ,  __RequestVerificationToken: token };
        $.ajax({
            type: "POST",
            url: "https://pollyy.azurewebsites.net/Polls/Vote/" + $(this).data("poll"),
            data: data
        }).done(function (data) {
            $(".votes").html(data);
            drawChart();
        });
    });



    unHideInput(2);

    $(".add-input").click(function (event) {
        event.preventDefault();
        unHideInput(1);
    });

    $(".remove-input").click(function (event) {
        event.preventDefault();
        hideInput(1);
    });

    $(".submit-new").click(function (e) {
        e.preventDefault();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var data = { Answer: $(".new-answer").val(), __RequestVerificationToken: token  };
 
        $.ajax({
            type: "POST",
            url: "https://pollyy.azurewebsites.net/Polls/AddAnswer/" + $("h2").data("poll"),
            data: data
        }).done(function (data) {
            $(".votes").html(data);
            drawChart();
        });
    });

        // check if on poll detail page
        if (window.location.pathname.includes("Details")) {
            // Load the Visualization API and the corechart package.
            google.charts.load('current', { 'packages': ['corechart'] });

            // Set a callback to run when the Google Visualization API is loaded.
            google.charts.setOnLoadCallback(drawChart);

            // Callback that creates and populates a data table,
            // instantiates the pie chart, passes in the data and
            // draws it.

        }




        function drawChart() {

            $.ajax({
                type: "GET",
                url: "https://pollyy.azurewebsites.net/Polls/data/" + $("h2").data("poll")
            }).done(function (data) {
                var chartdata = JSON.parse(data);
                CreateDataTable(chartdata);
            });

            // Create the data table.
            function CreateDataTable(chartdata) {
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Option');
                data.addColumn('number', 'votes');
                chartdata.forEach(function (answer) {
                    data.addRows([
                        [answer.Option, answer.Votes]
                    ]);
                });

                // Set chart options
                var options = {
                    height: 400,
                    width: 340,
                    legend: 'none',
                    pieHole: 0.4,
                    chartArea : 100
                };
                // Instantiate and draw our chart, passing in some options.
                var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
                chart.draw(data, options);
            }

        }



});



function unHideInput(x) {
    for (var i = 0; i < x; i++) {
            $('.hidden').first().removeClass("hidden");
        }
    }

    function hideInput() {
        if ($(".answer").not(".hidden").length === 2) {
            return;
        }
        var input = $(".answer").not(".hidden").last();
        $(input).addClass("hidden");
        $(input).val("");
    }
