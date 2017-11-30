// Write your JavaScript code.
$(".box").click(function () {

    var serviceNumber = $(this).attr("id");
    var service = $(this).children("h4").html();

    $("#serviceId").val(serviceNumber);
    $("#choosenService").html("Du har valt: " + service)
    
})