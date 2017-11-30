﻿$(document).ready(function () {

    var btn = document.getElementById("send");
    var ws = new WebSocket("ws://127.0.0.1:8000/mac")

    btn.addEventListener('click', function () {
       

        ws.onopen = function () {
            var message = {
                serviceTypes: document.getElementById("serviceId").value,
                regNumber: document.getElementById("regNumber").value
            };

            ws.send(JSON.stringify(message));
        }
        $("#regModal").modal("toggle");
        $("#infoModal").modal("toggle");
        setTimeout(function () {
            $("#infoModal").modal("toggle");
        }, 3000);

    })

    $(".box").click(function () {

        var serviceNumber = $(this).attr("id");
        var service = $(this).children("h4").html();

        $("#serviceId").val(serviceNumber);
        $("#choosenService").html("Du har valt: " + service)

    })
});

