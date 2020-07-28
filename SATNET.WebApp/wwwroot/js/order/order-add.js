var unitGb;
var unitMb;
var customerId;

$(function () {
    unitGb = $("#hdnUnitGb").val();
    unitMb = $("#hdnUnitMb").val();
    customerId = $("#hdnCustomerId").val();

    hideAllSections();
    if (customerId != 0) {
        $(".select-customer").hide();
    }
    $("#RequestTypeId").change(function () {
        var requestTypeId = $("#RequestTypeId").val();
        switch (requestTypeId) {
            case '1'://Activation
                hideAllSections();
                $(".new-site").show();
                $(".hardware").show();
                $(".site-name").show();
                break
            case '2':// Termination
                hideAllSections();
                $(".select-site").show();
                break
            case '32'://Re-Activation
                hideAllSections();
                $(".select-site").show();
                $(".new-site").show();
                $(".site-name").hide();
                break
            case '3'://Upgrade
                hideAllSections();
                $(".upgrade").show();
                $(".select-site").show();
                break
            case '4'://Downgrade
                hideAllSections();
                $(".downgrade").show();
                $(".select-site").show();
                break
            case '5':// Token Top up
                hideAllSections();
                $(".token").show();
                $(".select-site").show();
                break
            case '6':// lock
                hideAllSections();
                $(".select-site").show();
                break
            case '7':// Unlock
                hideAllSections();
                $(".select-site").show();
                break
            case '8':// Other
                hideAllSections();
                $(".other").show();
                $(".select-site").show();
                break
            default:
                hideAllSections();
        }
    });
    $(".service-plantype").change(function () {
        var type = $(this).val();
        var section = $(this).data('section');
        if (type != "") {
            getServicePlansByType(type, section);
        }
    });
    $("#CustomerId").change(function () {
        if (customerId == 0 && $(this).val() > 0) {
            var url = '/Order/GetProposedSiteName';
            $.getJSON(url, { customerId: $(this).val() }, function (result) {
                $("#SiteName").val(result.siteName);
            });
        }
    });

});

hideAllSections = function () {
    $(".new-site").hide();
    $(".upgrade").hide();
    $(".downgrade").hide();
    $(".token").hide();
    $(".other").hide();
    $(".select-site").hide();
    $(".hardware").hide();
}

getServicePlansByType = function (type, section) {
    var url = '/Order/GetServicePlansByType';
    $.getJSON(url, { servicePlanTypeId: type }, function (result) {
        var items = '';
        $.each(result, function (i, plan) {
            items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
        });
        if (section == "newsite") {
            $("#ServicePlanId").empty();
            $("#ServicePlanId").html(items);
            $("#ServicePlanId").prop("disabled", false);
            setPlanUnit($(".unit-serviceplan"), type);
        }
        else if (section == "downgrade") {
            $("#DowngradeFromId").empty();
            $("#DowngradeToId").empty();
            $("#DowngradeFromId").html(items);
            $("#DowngradeToId").html(items);
            $("#DowngradeFromId").prop("disabled", false);
            $("#DowngradeToId").prop("disabled", false);
            setPlanUnit($(".unit-downgrade"), type);
        }
        else if (section == "upgrade") {
            $("#UpgradeFromId").empty();
            $("#UpgradeToId").empty();
            $("#UpgradeFromId").html(items);
            $("#UpgradeToId").html(items);
            $("#UpgradeFromId").prop("disabled", false);
            $("#UpgradeToId").prop("disabled", false);
            setPlanUnit($(".unit-upgrade"), type);
        }
    });
}

setPlanUnit = function (span, type) {
    if (type == 12) {//quota
        $(span).text(unitGb);
    }
    else if (type == 14) {//dedicated
        $(span).text(unitMb);
    }
    else {
        $(span).text('');
    }
}

