var unitGb;
var unitMb;
var _customerId;

$(function () {

    unitGb = $("#hdnUnitGb").val();
    unitMb = $("#hdnUnitMb").val();
    _customerId = $("#hdnCustomerId").val();

    var dtToday = new Date();
    var maxDate = new Date();
    maxDate.setDate(maxDate.getDate() + 5);
    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();

    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var minDate = year + '-' + month + '-' + day;
    console.log(maxDate);
    $('#InstallationDate').attr('min', minDate);
    $('#InstallationDate').attr('max', maxDate);

    hideAllSections();
    if (_customerId != 0) {
        $(".select-customer").hide();
        //Standard User
        //if (_customerId > 0) {
        //    var url = '/Order/GetProposedSiteName';
        //    $.getJSON(url, { customerId: _customerId }, function (result) {
        //        $("#SiteName").val(result.siteName);
        //    });
        //}
    }
    $("#RequestTypeId").change(function () {
        // on/off validation rules according to business logic
        $('#SiteId').rules('add', {
            required: true   // overwrite an existing rule
        });
        var requestTypeId = $("#RequestTypeId").val();
        hideAllSections();
        resetFields();
        switch (requestTypeId) {
            case '1'://Activation
                $(".new-site").show();
                $(".hardware").show();
                $(".site-name").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58);
                $("#ScheduleDateId").prop("disabled", true);
                break
            case '2':// Termination
                $(".select-site").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58);
                $("#ScheduleDateId").prop("disabled", true);
                break
            case '32'://Re-Activation
                $(".select-site").show();
                $(".new-site").show();
                $(".hardware").show();
                $("#MacAirNoId").prop("disabled", true);
                $("#HardwareConditionId").prop("disabled", true);
                $("#PromotionId").prop("disabled", true);
                $(".site-name").hide();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58);
                $("#ScheduleDateId").prop("disabled", true);
                break
            case '3'://Upgrade
                $(".upgrade").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(0);
                $("#ScheduleDateId").prop("disabled", false);
                break
            case '4'://Downgrade
                $(".downgrade").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(59);
                $("#hdnScheduleDateId").val(59);
                $("#ScheduleDateId").prop("disabled", true);
                $("#downgrade-note").show();
                break
            case '5':// Token Top up
                $(".token").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58);
                $("#ScheduleDateId").prop("disabled", true);
                break
            case '6':// lock
                $(".select-site").show();
                break
            case '7':// Unlock
                $(".select-site").show();
                $("#ScheduleDateId").val(0);
                $("#ScheduleDateId").prop("disabled", false);
                break
            case '8':// Other
                $(".other").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58);
                $("#ScheduleDateId").prop("disabled", true);
                // on/off validation rules according to business logic
                $('#SiteId').rules('add', {
                    required: false   // overwrite an existing rule
                });
                break
            case '68':// Change IP
                $(".change-ip").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58); //schedule date now
                $("#ScheduleDateId").prop("disabled", true);
                break
            case '67':// Change Plan
                $(".change-serviceplan").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(59);
                $("#hdnScheduleDateId").val(59); //schedule date end of month
                $("#ScheduleDateId").prop("disabled", true);
                break
            case '69':// Modem Swap
                $(".modem-swap").show();
                $(".select-site").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58); //schedule date now
                $("#ScheduleDateId").prop("disabled", true);
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
        $('#txtProRataGB').text('');
        $('.pro-rata-gb').hide();
        var type = $(this).val();
        var section = $(this).data('section');
        if (type != "") {
            getServicePlansByType(type, section);
        }
    });
    $("#CustomerId").change(function () {
        console.log($('#CustomerId').val());
        var customerDDLId = $(this).val();
        var requestTypeId = $("#RequestTypeId").val();

        //Satnet User
        if (_customerId == 0 && customerDDLId > 0) {//satnet user
            var url = '/Order/GetProposedSiteName';
            $.getJSON(url, { customerId: $(this).val() }, function (result) {
                $("#SiteName").val(result.siteName);
            });
        }
        
        if (customerDDLId > 0 && requestTypeId > 0 && requestTypeId != '1') {
            getSites(requestTypeId, customerDDLId);
        }
    });
    $("#SiteId").change(function () {
        if ($(this).val() > 0) {
            var url = '/Order/GetSiteDetails';
            $.getJSON(url, { siteId: $(this).val() }, function (result) {
                if (result != null) {
                    var requestTypeId = $("#RequestTypeId").val();
                    $("#SiteCityId").val(result.cityId);
                    $("#SiteArea").val(result.area);
                    $("#ServicePlanTypeId").val(result.servicePlanTypeId);
                    if (result.servicePlanTypeId > 0) {
                        populateServicePlan(result.servicePlanTypeId, result.servicePlanId, requestTypeId);
                    }
                    $("#IPId").val(result.ipId);
                    $("#SubscriberName").val(result.subscriberName);
                    $("#MacAirNoId").val(result.macAirNoId);
                    $('#ipChangeTo').empty();
                    var $options = $("#IPId > option").clone();
                    $('#ipChangeTo').append($options);
                    $("#ipPlan").val(result.ipId);
                    $("#ipPlan").prop("disabled", true);
                    $("#ipChangeTo option[value=" + result.ipId + "]").remove();
                    $("#currentServicePlanType").val(result.servicePlanTypeId);
                    $("#currentServicePlanType").prop("disabled", true);
                    $("#currentMacAirNo").val(result.macAirNoId);
                    $("#currentMacAirNo").prop("disabled", true);
                }
            });
        }
    });
    $("#ServicePlanId").change(function () {
        var servicePlanType = $("#ServicePlanTypeId").val();
        if (servicePlanType == '14') {
            var dedicatedPlanValue = $(this).val();
            var dedicatedPlanText = $('#ServicePlanId option:selected').text();
            if (dedicatedPlanValue > 0) {
                if (dedicatedPlanText == 'Above 80') {
                    $(".custom-dedicated-serviceplan").show();
                    $(".dedicated-serviceplan").hide();
                }
                else {
                    $(".dedicated-serviceplan").show();
                    $(".custom-dedicated-serviceplan").hide();
                    populateCustomDedicatedPlans(dedicatedPlanText);
                }
            }
        }
        else if (servicePlanType == '12') {
            var quotaPlanValue = $(this).val();
            if (quotaPlanValue > 0) {
                var quotaPlanText = $('#ServicePlanId option:selected').text();
                var installationDate = $('#InstallationDate').val();
                if (installationDate != '') {
                    $('.pro-rata-gb').show();
                    showProRataGB(quotaPlanText, installationDate)
                }
                else {
                    //alert('Select plan installation date to see pro rata quota GB.');
                }
            }
        }
    });
    $("#InstallationDate").change(function () {
        var installationDate = $(this).val();
        var quotaPlanValue = $('#ServicePlanId').val();
        var quotaPlanText = $('#ServicePlanId option:selected').text();
        if (installationDate != '' && quotaPlanValue > 0) {
            $('.pro-rata-gb').show();
            showProRataGB(quotaPlanText, installationDate)
        }
    });
    $("#SiteCityId").change(function () {
        var cityId = $(this).val();
        if (cityId > 0) {
            getArea(cityId);
        }
    });
    $("#HardwareConditionId").change(function () {
        var conditionId = $(this).val();
        if (conditionId == 61) { //used
            $("#PromotionId option[value='2']").prop("disabled", true);
        }
        else {
            $("#PromotionId option[value='2']").prop("disabled", false);
        }
    });

});

hideAllSections = function () {
    $(".new-site").hide();
    $(".dedicated-serviceplan").hide();
    $(".custom-dedicated-serviceplan").hide();
    $(".pro-rata-gb").hide();
    $(".upgrade").hide();
    $(".downgrade").hide();
    $(".token").hide();
    $(".other").hide();
    $(".select-site").hide();
    $(".hardware").hide();
    $("#downgrade-note").hide();
    $(".change-ip").hide();
    $(".change-serviceplan").hide();
    $(".modem-swap").hide();
}

getServicePlansByType = function (type, section) {
    var url = '/Order/GetServicePlansByType';
    $.getJSON(url, { servicePlanTypeId: type }, function (result) {
        var items = '';
        items += "<option value=''>Select</option>";
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
        } else if (section == "change-service-plan") {
            $("#selectChangeServicePlan").empty();
            $("#selectChangeServicePlan").html(items);
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

getSites = function (requestTypeId, customerId) {
    var statusIds = [];
    switch (requestTypeId) {
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
        case '2':// Termination
            //list all active,locked sites
            statusIds.push(17);
            statusIds.push(18);
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
            $("#SiteId").prepend("<option value=''>Select</option>").val('');
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
        else if (requestTypeId == "4") {
            $("#DowngradeFromId").empty();
            $("#DowngradeToId").empty();
            $("#DowngradeFromId").html(items);
            $("#DowngradeToId").html(items);
            $("#DowngradeToId").prop("disabled", false);
            setPlanUnit($(".unit-downgrade"), servicePlanType);
            $("#downgradeServicePlanType").val(servicePlanType);
            $("#DowngradeFromId").val(servicePlanId);
            $("#downgradeServicePlanType").prop("disabled", true);
            $("#DowngradeFromId").prop("disabled", true);
            removeDowngradeItem(servicePlanType, servicePlanId);
        }
        else if (requestTypeId == "3") {
            $("#UpgradeFromId").empty();
            $("#UpgradeToId").empty();
            $("#UpgradeFromId").html(items);
            $("#UpgradeToId").html(items);
            $("#UpgradeToId").prop("disabled", false);
            setPlanUnit($(".unit-upgrade"), servicePlanType);
            $("#upgradeServicePlanType").val(servicePlanType);
            $("#UpgradeFromId").val(servicePlanId);
            $("#upgradeServicePlanType").prop("disabled", true);
            $("#UpgradeFromId").prop("disabled", true);
            removeUpgradeItem(servicePlanType, servicePlanId);
        }
    });

}

resetFields = function () {
    $("#CustomerId").val('');
    $("#SiteId").val('');
    $("#SiteCityId").val('');
    $("#SiteId").empty();
    $("#SiteCity").val('');
    $("#SiteArea").val('');
    $(".service-plantype").val('');
    $(".service-plan").val('');
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

removeUpgradeItem = function (servicePlanType, servicePlanValue) {
    if (servicePlanType == 12) {//quota
        $("#UpgradeToId > option").each(function () {
            var currentText = this.text;
            var existingText = $("#UpgradeFromId option:selected").text();
            var currentNo = parseInt(currentText);
            var existingNo = parseInt(existingText);
            if (currentNo <= existingNo) {
                $('#UpgradeToId option:eq(' + this.index + ')').remove();
            }
        });
    }
    else if (servicePlanType == 13) {//unlimited
        $("#UpgradeToId option:contains(Unlimited 5)").remove();
        $("#UpgradeFromId option:contains(Unlimited 30)").remove();
        var priceValue = "Valvet ($100)";
        console.log(/\d+/g.exec(priceValue)[0]);

        $("#UpgradeToId > option").each(function () {
            var currentText = /\d+/g.exec(this.text)[0];
            var existingText = /\d+/g.exec($("#UpgradeFromId option:selected").text())[0];
            var currentNo = parseInt(currentText);
            var existingNo = parseInt(existingText);
            if (currentNo <= existingNo) {
                $('#UpgradeToId option:eq(' + this.index + ')').remove();
            }
        });
    }
}

populateCustomDedicatedPlans = function (dedicatedPlanText) {
    var items = '';
    if (dedicatedPlanText == '2 to 9') {
        for (var i = 2; i <= 9; i++) {
            items += "<option value='" + i + "'>" + i + "</option>";
        }
    }
    else if (dedicatedPlanText == '10 to 19') {
        for (var i = 10; i <= 19; i++) {
            items += "<option value='" + i + "'>" + i + "</option>";
        }
    }
    else if (dedicatedPlanText == '20 to 39') {
        for (var i = 20; i <= 39; i++) {
            items += "<option value='" + i + "'>" + i + "</option>";
        }
    }
    else if (dedicatedPlanText == '40 to 79') {
        for (var i = 40; i <= 79; i++) {
            items += "<option value='" + i + "'>" + i + "</option>";
        }
    }
    $("#DedicatedServicePlanName").empty();
    $("#DedicatedServicePlanName").html(items);
}

getArea = function (cityId) {
    $.ajax({
        url: '/Order/GetCityArea',
        data: { cityId: cityId },
        success: function (result) {
            if (result != null) {
                $('#SiteArea').val(result.areaname);
            }
        }
    });
}

showProRataGB = function (quotaPlanText, installationDate) {
    $.ajax({
        url: '/Order/GetProRataGB',
        data: { monthlyQuota: quotaPlanText, installationDate: installationDate },
        success: function (result) {
            if (result != null) {
                $('#txtProRataGB').val(result.proRataQuotaGB);
            }
        }
    });
}






