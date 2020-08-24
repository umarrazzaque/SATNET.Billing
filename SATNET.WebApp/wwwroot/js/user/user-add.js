
var customerType = new Map();
    customerType.set('Customer', '15');
    customerType.set('Reseller', '16');
    customerType.set('Satnet', '24');

var roleType = new Map();
    roleType.set('Satnet', '33');
    roleType.set('Reseller', '34');
var userTypeId = $("#hdnUserTypeId").val();
var roleName = $("#hdnUserRoleName").val();
$(document).ready(function () {

});
$(function () {
    // for edit user scenario
    if (typeof userTypeId !== 'undefined') {
        if (userTypeId == customerType.get('Satnet')) {//satnet user type
            $("#CustomerId").prop("disabled", true);
            $("#CustomerId").val('');
        }
        //else if (userTypeId == customerType.get('Customer')) {//direct user type
        //    $("#RoleName").prop("disabled", true);
        //}
    }
    //if (typeof roleName != 'undefined') {


    //    $("#RoleName").val(roleName);
    //}

    $("#UserTypeId").change(function () {
        var type = $(this).val();
        console.log(customerType.get('Customer'));
        if (type === customerType.get('Customer')) {
            //alert(customerType.get('Customer'));
            $("#CustomerId").prop("disabled", false);
            $("#CustomerId").empty();
            getCustomers();
            $("#RoleName").prop("disabled", false);
            $("#RoleName").empty();
            getRoles(roleType.get('Reseller'));//reseller role
        } else if (type === customerType.get('Satnet')) {
            //alert(customerType.get('Satnet'));
            $("#CustomerId").empty();
            $("#CustomerId").prop("disabled", true);
            $("#RoleName").prop("disabled", false);
            $("#RoleName").empty();
            getRoles(roleType.get('Satnet'));//satnet role
        }

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
        items += "<option value=''>-Select Customer-</option>";
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
        items += "<option value=''>-Select Role-</option>";
        $.each(result, function (i, plan) {
            items += "<option value='" + plan.text + "'>" + plan.text + "</option>";
        });
        $("#RoleName").empty();
        $("#RoleName").html(items);
        $("#RoleName").prop("disabled", false);
    });
}