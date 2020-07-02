$(function () {
    var userTypeId = $("#hdnUserTypeId").val();
    if (userTypeId != null && userTypeId ==24) {
        $("#PriceTierId").prop("disabled", true);
        $("#CustomerId").prop("disabled", true);
        $("#PriceTierId").val('');
        $("#CustomerId").val('');
    }

    $("#UserTypeId").change(function () {
        var type = $(this).val();
        if (type !== "" && type !== "24") {
            $("#PriceTierId").prop("disabled", false);
        }
        else {
            $("#PriceTierId").prop("disabled", true);
        }
        $("#CustomerId").prop("disabled", true);
        $("#CustomerId").val("");
        $("#PriceTierId").val("");
    });
    $("#PriceTierId").change(function () {
        var tier = $(this).val();
        if (tier !== "") {
            getCustomers();
        }
        else {
            $("#CustomerId").prop("disabled", true);
        }
    });
});

getCustomers = function () {
    var tier = $("#PriceTierId").val();//customer price tier
    var type = $("#UserTypeId").val();//customer type
    var url = '/User/GetCustomers';
    $.getJSON(url, { customerTypeId: type, priceTierId: tier }, function (result) {
        var items = '';
        $.each(result, function (i, plan) {
            items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
        });
        $("#CustomerId").empty();
        $("#CustomerId").html(items);
        $("#CustomerId").prop("disabled", false);
    });
}