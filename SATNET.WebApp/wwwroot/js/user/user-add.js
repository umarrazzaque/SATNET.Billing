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
            $("#RoleId").empty();
            $("#RoleId").prop("disabled", false);
            getRoles(33);//satnet role
        }
        else if (type == "15") {//direct customer user type
            $("#PriceTierId").prop("disabled", true);
            $("#PriceTierId").val('11');
            $("#RoleId").empty();
            $("#RoleId").prop("disabled", true);
        }
        else if (type == "16") {//reseller user type
            $("#PriceTierId").prop("disabled", false);
            $("#RoleId").empty();
            $("#RoleId").prop("disabled", false);
            getRoles(34);//reseller role
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

getRoles = function (roleType) {
    var url = '/User/GetRoles';
    $.getJSON(url, { type: roleType}, function (result) {
        var items = '';
        $.each(result, function (i, plan) {
            items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
        });
        $("#RoleId").empty();
        $("#RoleId").html(items);
        $("#RoleId").prop("disabled", false);
    });
}