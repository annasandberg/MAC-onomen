$(document).ready(function () {


    var ws = new WebSocket("ws://127.0.0.1:8000/mac")

    ws.onopen = function () {
        var btn = document.getElementById("send");
        btn.addEventListener('click', function () {
            var message = {
                serviceTypes: document.getElementById("serviceId").value,
                regNumber: document.getElementById("regNumber").value
            };

            ws.send(JSON.stringify(message));
            ws.close();
            $("#regModal").modal("toggle");
            $("#infoModal").modal("toggle");
            setTimeout(function () {
                $("#infoModal").modal("toggle");
            }, 3000);
        });
        

    };

    $(".box").click(function () {

        var serviceNumber = $(this).attr("id");
        var service = $(this).children("h4").html();

        $("#serviceId").val(serviceNumber);
        $("#choosenService").html("Du har valt: " + service)

    });
});

