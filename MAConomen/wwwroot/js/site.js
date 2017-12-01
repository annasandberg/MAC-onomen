$(document).ready(function () {
    const regex = /^[A-Z]{3}\d{3}/g;

    var ws = new WebSocket("ws://127.0.0.1:8000/customer")

    ws.onopen = function () {
        var btn = document.getElementById("send");
        btn.addEventListener('click', function () {
            var message = {
                serviceTypes: document.getElementById("serviceId").value,
                regNumber: document.getElementById("regNumber").value.toUpperCase()
            };

            if (regex.test(message.regNumber))
            {
                ws.send(JSON.stringify(message));

                $("#regNumber").val('');

                $("#infoModal").modal("toggle");

                setTimeout(function () {
                    $("#infoModal").modal("toggle");
                }, 3000);
            }
            else
            {
                $("#regNumber").val('');
                $("#validation").html("Felaktigt regnummer, försök igen!");
                setTimeout(function () {
                    $("#validation").html("");
                }, 2000)
              
            }
        
        });
       
    };

    $(".box").click(function () {

        var serviceNumber = $(this).attr("id");
        var service = $(this).children("h4").html();

        $("#serviceId").val(serviceNumber);
        $("#choosenService").html("Du har valt: " + service)

    });

    
});

