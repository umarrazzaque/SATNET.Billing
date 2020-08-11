var unitGb;
var unitMb;
var _customerId;

$(function () {
    unitGb = $("#hdnUnitGb").val();
    unitMb = $("#hdnUnitMb").val();
    _customerId = $("#hdnCustomerId").val();

    hideAllSections();
    if (_customerId != 0) {
        $(".select-customer").hide();
    }
    $("#RequestTypeId").change(function () {
        var requestTypeId = $("#RequestTypeId").val();
        hideAllSections();
        resetFields();
        switch (requestTypeId) {
            case '1'://Activation
                $(".new-site").show();
                $(".hardware").show();
                $(".site-name").show();
                break
            case '2':// Termination
                $(".select-site").show();
                break
            case '32'://Re-Activation
                $(".select-site").show();
                $(".new-site").show();
                $(".site-name").hide();
                break
            case '3'://Upgrade
                $(".upgrade").show();
                $(".select-site").show();
                break
            case '4'://Downgrade
                $(".downgrade").show();
                $(".select-site").show();
                break
            case '5':// Token Top up
                $(".token").show();
                $(".select-site").show();
                break
            case '6':// lock
                $(".select-site").show();
                break
            case '7':// Unlock
                $(".select-site").show();
                break
            case '8':// Other
                $(".other").show();
                $(".select-site").show();
                break
            default:
        }
    });
    $("#RequestTypeId").change(function () {
        var requestTypeId = $("#RequestTypeId").val();
        var customerId = getCustomerId();
        if (requestTypeId != '1' && customerId > 0) {
            getSites(requestTypeId, customerId);
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
        var customerDDLId = $(this).val();
        var requestTypeId = $("#RequestTypeId").val();
        //if (_customerId == 0 && customerDDLId > 0) {//satnet user
        //    var url = '/Order/GetProposedSiteName';
        //    $.getJSON(url, { customerId: $(this).val() }, function (result) {
        //        $("#SiteName").val(result.siteName);
        //    });
        //}
        if (customerDDLId > 0 && requestTypeId > 0 && requestTypeId != '1') {
            getSites(requestTypeId, customerDDLId);
        }
    });
    $("#SiteId").change(function () {
        if ($(this).val() > 0) {
            var url = '/Order/GetSiteDetails';
            $.getJSON(url, { siteId: $(this).val() }, function (result) {
                if (result != null) {
                    $("#SiteCity").val(result.city);
                    $("#SiteArea").val(result.area);
                    $("#ServicePlanTypeId").val(result.servicePlanTypeId);
                    if (result.servicePlanTypeId > 0) {
                        populateServicePlan(result.servicePlanTypeId, result.servicePlanId, $("#RequestTypeId").val());
                    }
                    $("#IPId").val(result.ipId);
                    $("#SubscriberName").val(result.subscriberName);
                    $("#SubscriberCity").val(result.subscriberCity);
                    $("#SubscriberEmail").val(result.subscriberEmail);
                    $("#SubscriberArea").val(result.subscriberArea);
                    $("#SubscriberNotes").val(result.subscriberNotes);
                }
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
            removeDowngradeItem(type);
        }
        else if (section == "upgrade") {
            $("#UpgradeFromId").empty();
            $("#UpgradeToId").empty();
            $("#UpgradeFromId").html(items);
            $("#UpgradeToId").html(items);
            $("#UpgradeFromId").prop("disabled", false);
            $("#UpgradeToId").prop("disabled", false);
            setPlanUnit($(".unit-upgrade"), type);
            removeUpgradeItem(type);
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

getSites = function (requestTypeId, customerId) {
    var statusIds = [];
    switch (requestTypeId) {
        case '2':// Termination
        case '3'://Upgrade
        case '4'://Downgrade
        case '5':// Token Top up
        case '6':// lock
            //list all active sites
            statusIds.push(17);
            break
        case '7': // Unlock
            //list all locked sites
            statusIds.push(18);
            break
        case '32'://Re-Activation
            //list all terminated sites
            statusIds.push(19);
            break
        default:
    }
    $.ajax({
        url: '/Order/GetSites',
        data: { customerId: customerId, statusIds: statusIds },
        traditional: true,
        success: function (result) {
            var items = '';
            $.each(result, function (i, plan) {
                items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
            });
            $("#SiteId").empty();
            $("#SiteId").html(items);
        }
    });
}

getCustomerId = function () {
    var customerId = 0;
    if (_customerId == 0 && $("#CustomerId").val() > 0) {
        customerId = $("#CustomerId").val();
    }
    else if (_customerId > 0) {
        customerId = _customerId; 
    }
    return customerId;
}

populateServicePlan = function (servicePlanType, servicePlanId, requestTypeId) {
    var url = '/Order/GetServicePlansByType';
    $.getJSON(url, { servicePlanTypeId: servicePlanType }, function (result) {
        var items = '';
        $.each(result, function (i, plan) {
            items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
        });
        if (requestTypeId == "1" || requestTypeId == "32") {
            $("#ServicePlanId").empty();
            $("#ServicePlanId").html(items);
            $("#ServicePlanId").prop("disabled", false);
            setPlanUnit($(".unit-serviceplan"), servicePlanType);
            $("#ServicePlanId").val(servicePlanId);
        }
        //else if (requestTypeId == "4") {
        //    $("#DowngradeFromId").empty();
        //    $("#DowngradeToId").empty();
        //    $("#DowngradeFromId").html(items);
        //    $("#DowngradeToId").html(items);
        //    $("#DowngradeFromId").prop("disabled", false);
        //    $("#DowngradeToId").prop("disabled", false);
        //    setPlanUnit($(".unit-downgrade"), servicePlanType);
        //    $("#ServicePlanTypeId").val(servicePlanType);
        //    $("#DowngradeFromId").val(servicePlanId);
        //    removeHighestValue(type);
        //}
        //else if (requestTypeId == "3") {
        //    $("#UpgradeFromId").empty();
        //    $("#UpgradeToId").empty();
        //    $("#UpgradeFromId").html(items);
        //    $("#UpgradeToId").html(items);
        //    $("#UpgradeFromId").prop("disabled", false);
        //    $("#UpgradeToId").prop("disabled", false);
        //    setPlanUnit($(".unit-upgrade"), servicePlanType);
        //    $("#ServicePlanTypeId").val(servicePlanType);
        //    $("#UpgradeFromId").val(servicePlanId);
        //    removeLowestValue(type);
        //}
    });

}

resetFields = function () {
    $("#SiteCity").val('');
    $("#SiteArea").val('');
    $(".service-plantype").val(0);
    $(".service-plan").val(0);
    $(".service-plan").empty();
    $("#IPId").val(0);
    $("#SubscriberName").val('');
    $("#SubscriberCity").val('');
    $("#SubscriberEmail").val('');
    $("#SubscriberArea").val('');
    $("#SubscriberNotes").val('');
}

removeDowngradeItem = function (servicePlanType) {
    if (servicePlanType == 12) {//quota
        $("#DowngradeToId option:contains(1000)").remove();
        $("#DowngradeFromId option:contains(15)").remove();
    }
    else if (servicePlanType == 13) {//unlimited
        $("#DowngradeToId option:contains(Unlimited 20)").remove();
        $("#DowngradeFromId option:contains(Unlimited 5)").remove();
    }
}

removeUpgradeItem = function (servicePlanType) {
    if (servicePlanType == 12) {//quota
        $("#UpgradeToId option:contains(15)").remove();
        $("#UpgradeFromId option:contains(1000)").remove();
    }
    else if (servicePlanType == 13) {//unlimited
        $("#UpgradeToId option:contains(Unlimited 5)").remove();
        $("#UpgradeFromId option:contains(Unlimited 30)").remove();
    }
}



