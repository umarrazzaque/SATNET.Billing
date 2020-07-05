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

        if(type == "24") {//satnet user type
            $("#PriceTierId").prop("disabled", true);
            $("#RoleName").empty();
            $("#RoleName").prop("disabled", false);
            getRoles(33);//satnet role
        }
        else if (type == "15") {//direct customer user type
            $("#PriceTierId").val('11');
            $("#PriceTierId").prop("disabled", true);
            $("#RoleName").empty();
            $("#RoleName").prop("disabled", true);
            getCustomers();
        }
        else if (type == "16") {//reseller user type
            $("#PriceTierId").prop("disabled", false);
            $("#PriceTierId").val('');
            $("#RoleName").empty();
            $("#RoleName").prop("disabled", false);
            getRoles(34);//reseller role
        }
        $("#CustomerId").prop("disabled", true);
        $("#CustomerId").val("");
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

getRoles = function (roleType) {
    var url = '/User/GetRoles';
    $.getJSON(url, { type: roleType}, function (result) {
        var items = '';
        $.each(result, function (i, plan) {
            items += "<option value='" + plan.text + "'>" + plan.text + "</option>";
        });
        $("#RoleName").empty();
        $("#RoleName").html(items);
        $("#RoleName").prop("disabled", false);
    });
}