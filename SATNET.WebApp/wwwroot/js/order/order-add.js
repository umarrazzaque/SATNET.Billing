var unitGb;
var unitMb;
var _customerId;

$(function () {
    var mySelect = $('#CustomerId').selectpicker({

        // text for no selection
        noneSelectedText: 'Select',

        // text for no result
        noneResultsText: 'No results matched {0}',

        // Sets the format for the text displayed when selectedTextFormat is count or count > #. {0} is the selected amount. {1} is total available for selection.
        // When set to a function, the first parameter is the number of selected options, and the second is the total number of options. 
        // The function must return a string.
        countSelectedText: function (numSelected, numTotal) {
            return (numSelected == 1) ? "{0} item selected" : "{0} items selected";
        },

        // The text that is displayed when maxOptions is enabled and the maximum number of options for the given scenario have been selected.
        // If a function is used, it must return an array. array[0] is the text used when maxOptions is applied to the entire select element. array[1] is the text used when maxOptions is used on an optgroup. 
        // If a string is used, the same text is used for both the element and the optgroup.
        maxOptionsText: function (numAll, numGroup) {
            return [
                (numAll == 1) ? 'Limit reached ({n} item max)' : 'Limit reached ({n} items max)',
                (numGroup == 1) ? 'Group limit reached ({n} item max)' : 'Group limit reached ({n} items max)'
            ];
        },

        // Text for Select All button
        selectAllText: 'Select All',

        // Text for Deselect All button
        deselectAllText: 'Deselect All',

        // Shows done button
        doneButton: false,

        // Text for done button
        doneButtonText: 'Close',

        // custom separator
        multipleSeparator: ', ',

        // button styles
        styleBase: 'btn',
        style: 'btn-default',

        // dropdown size
        size: 'auto',

        // dropdown title
        title: null,

        // 'values' | 'static' | 'count' | 'count > x'
        selectedTextFormat: 'values',

        // dropdown width
        width: false,

        // e.g., container: 'body' | '.main-body'
        container: false,

        // hide disabled options
        hideDisabled: false,

        // shows sub text
        showSubtext: false,

        // shows icon
        showIcon: true,

        // shows content
        showContent: true,

        // auto dropup
        dropupAuto: true,

        // shows dropdown header
        header: false,

        // live search options
        liveSearch: true,
        liveSearchPlaceholder: null,
        liveSearchNormalize: false,
        liveSearchStyle: 'contains',

        // enables Select All / Deselect All box
        actionsBox: false,

        // icons
        iconBase: 'glyphicon',
        tickIcon: 'glyphicon-ok',

        // shows checkmark on selected option
        showTick: false,

        // custom template
        template: {
            caret: '<span class="caret"></span>'
        },

        // string | array | function
        maxOptions: false,

        // enables the device's native menu for select menus
        mobile: false,

        // treats the tab character like the enter or space characters within the selectpicker dropdown.
        selectOnTab: false,

        // Align the menu to the right instead of the left.
        dropdownAlignRight: false,

        // e.g. [top, right, bottom, left]
        windowPadding: 0

    });

    // Sets the selected value
    mySelect.selectpicker('val', 'JQuery');
    mySelect.selectpicker('val', ['jQuery', 'Script']);

    // Selects all items
    mySelect.selectpicker('selectAll');

    // Clears all
    mySelect.selectpicker('deselectAll');

    // Re-render
    mySelect.selectpicker('render');


    unitGb = $("#hdnUnitGb").val();
    unitMb = $("#hdnUnitMb").val();
    _customerId = $("#hdnCustomerId").val();

    var dtToday = new Date();
    var maxDate = new Date();
    maxDate.setDate(maxDate.getDate() + 5);
    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();

    var maxMonth = maxDate.getMonth() + 1;
    var maxDay = maxDate.getDate();
    var maxYear = maxDate.getFullYear();

    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    if (maxMonth < 10)
        maxMonth = '0' + maxMonth.toString();
    if (maxDay < 10)
        maxDay = '0' + maxDay.toString();

    var minDate = year + '-' + month + '-' + day;
    var maxDate = maxYear + '-' + maxMonth + '-' + maxDay;

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
                $("#AirMac").prop("disabled", false);
                $("#HardwareConditionId").prop("disabled", false);
                $("#PromotionId").prop("disabled", false);
                getModemModels();
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
                $("#AirMac").prop("disabled", true);
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
                $("#ScheduleDateId").val('');
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
                $("#ScheduleDateId").val('');
                $("#ScheduleDateId").prop("disabled", false);
                break
            case '7':// Unlock
                $(".select-site").show();
                $(".site-name").show();
                $("#ScheduleDateId").val(58);
                $("#hdnScheduleDateId").val(58);
                $("#ScheduleDateId").prop("disabled", true);
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
        $('#IsServicePlanFull').val('');
        $('#txtProRataGB').text('');
        $('.pro-rata-gb').hide();
        $('.fullOrProrata').hide();
        $('.dedicated-serviceplan').hide();
        $('.custom-dedicated-serviceplan').hide();
        var type = $(this).val();
        var section = $(this).data('section');
        if (type == "12" && section != "change-service-plan") {
            $('.fullOrProrata').show();
        }
        if (type != "" && section != 'downgrade' && section != 'upgrade') {
            getServicePlansByType(type, section);
        }
    });
    $("#CustomerId").change(function () {
        var customerDDLId = $(this).val();
        var requestTypeId = $("#RequestTypeId").val();
        
        if (customerDDLId > 0 && requestTypeId > 0 && requestTypeId != '1') {
            getSites(requestTypeId, customerDDLId);
        }
        var modemModelId = $("#selectModemModel").val();
        if (modemModelId > 0 && customerDDLId > 0) {
            getAIRMACs(customerDDLId, modemModelId, 'new-site');
        }
    });
    $("#SiteId").change(function () {
        if ($(this).val() > 0) {
            var url = '/Order/GetSiteDetails';
            $.getJSON(url, { siteId: $(this).val() }, function (result) {
                if (result != null) {
                    populateFormFields(result);
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
            $('#txtProRataGB').text('');
            var quotaPlanValue = $(this).val();
            if (quotaPlanValue > 0) {
                var quotaPlanText = $('#ServicePlanId option:selected').text();
                var installationDate = $('#InstallationDate').val();
                if (installationDate != '') {
                    //$('.pro-rata-gb').show();
                    showProRataGB(quotaPlanText, installationDate, '#txtProRataGB')
                }
                else {
                    //alert('Select plan installation date to see pro rata quota GB.');
                }
            }
        }
    });
    $("#InstallationDate").change(function () {
        var installationDate = $(this).val();
        var quotaPlanType = $('#ServicePlanTypeId').val();
        var quotaPlanValue = $('#ServicePlanId').val();
        var quotaPlanText = $('#ServicePlanId option:selected').text();
        if (installationDate != '' && quotaPlanValue > 0 && quotaPlanType == 12) {
            //$('.pro-rata-gb').show();
            showProRataGB(quotaPlanText, installationDate, '#txtProRataGB')
        }
    });
    $("#ScheduleDateId").change(function () {
        $('.upgrade-pro-rata-gb').hide();
        $('#txtUpgradeProRataGB').text('');
        var scheduleDate = $(this).val();
        var quotaPlanType = $('#upgradeServicePlanType').val();
        var quotaPlanValue = $('#UpgradeToId').val();
        var quotaPlanText = $('#UpgradeToId option:selected').text();
        if (scheduleDate == 58 && quotaPlanValue > 0 && quotaPlanType == 12) {// 58:now, 12:quota
            $('.upgrade-pro-rata-gb').show();
            showProRataGB(quotaPlanText, getDateToday(), '#txtUpgradeProRataGB')
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
    $("#UpgradeToId").change(function () {
        $('.upgrade-pro-rata-gb').hide();
        $('#txtUpgradeProRataGB').text('');
        var quotaPlanText = $('#UpgradeToId option:selected').text();
        if ($(this).val() != '' && $("#upgradeServicePlanType").val() == 12 && $("#ScheduleDateId").val()==58) { //for quota plan and schedule date now display pro-rata quota
            $('.upgrade-pro-rata-gb').show();
            showProRataGB(quotaPlanText, getDateToday(), '#txtUpgradeProRataGB');
        }
    });
    $("#IsServicePlanFull").change(function () {
        if ($(this).val() == "False") {
            $(".pro-rata-gb").show();
        }
        else if ($(this).val() == "True") {
            $(".pro-rata-gb").hide();
        }
    });
    $(".modem").change(function () {
        var section = $(this).data("section");
        var modemModelId = $(this).val();
        var customerId = getCustomerId();
        if (modemModelId > 0 && customerId > 0) {
            getAIRMACs(customerId, modemModelId, section);
        }
    });

});

getDateToday = function(){
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    today = mm + '/' + dd + '/' + yyyy;
    return today;
}
hideAllSections = function () {
    $(".new-site").hide();
    $(".dedicated-serviceplan").hide();
    $(".custom-dedicated-serviceplan").hide();
    $(".pro-rata-gb").hide();
    $(".fullOrProrata").hide();
    $(".upgrade-pro-rata-gb").hide();
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
        if (section == "new-site") {
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
        case '67':// change plan
        case '68'://change ip
            case '69': //modem swap
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
        items += "<option value=''>Select</option>";
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
        else if (requestTypeId == "67") {
            $("#currentServicePlanType").val(servicePlanType);
            $("#currentServicePlanType").prop("disabled", true);
            $("#selectCurrentServicePlan").empty();
            $("#selectCurrentServicePlan").html(items);
            $("#selectCurrentServicePlan").val(servicePlanId);
            $('#changeToServicePlanType').empty();
            var $options = $("#ServicePlanTypeId > option").clone();
            $('#changeToServicePlanType').append($options);
            $("#changeToServicePlanType option[value=" + servicePlanType + "]").remove();

            //setPlanUnit($(".unit-upgrade"), servicePlanType);
        }
    });

}

resetFields = function () {
    $("#CustomerId").val('');
    $('#CustomerId').selectpicker('refresh')
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
    $("#selectAirMac").val('');
    $("#selectAirMac").prop('disabled', false);
}

removeDowngradeItem = function (servicePlanType) {
    if (servicePlanType == 12) {//quota
        $("#DowngradeToId > option").each(function () {
            var currentText = this.text;
            var existingText = $("#DowngradeFromId option:selected").text();
            var currentNo = parseInt(currentText);
            var existingNo = parseInt(existingText);
            if (currentNo >= existingNo) {
                $('#DowngradeToId option:eq(' + this.index + ')').remove();
            }
        });
    }
    else if (servicePlanType == 13) {//unlimited
        var priceValue = "Valvet ($100)";
        console.log(/\d+/g.exec(priceValue)[0]);

        $("#DowngradeToId > option").each(function () {
            var currentText = /\d+/g.exec(this.text)[0];
            var existingText = /\d+/g.exec($("#DowngradeFromId option:selected").text())[0];
            var currentNo = parseInt(currentText);
            var existingNo = parseInt(existingText);
            if (currentNo >= existingNo) {
                $('#DowngradeToId option:eq(' + this.index + ')').remove();
            }
        });
    }
}

removeUpgradeItem = function (servicePlanType) {
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

showProRataGB = function (quotaPlanText, installationDate, textBox) {
    $.ajax({
        url: '/Order/GetProRataGB',
        data: { monthlyQuota: quotaPlanText, installationDate: installationDate },
        success: function (result) {
            if (result != null) {
                $(textBox).val(result.proRataQuotaGB);
            }
        }
    });
}

populateFormFields = function (result) {

    var requestTypeId = $("#RequestTypeId").val();
    $("#SiteCityId").val(result.cityId);
    $("#SiteArea").val(result.area);
    $("#ServicePlanTypeId").val(result.servicePlanTypeId);
    if (result.servicePlanTypeId > 0) {
        populateServicePlan(result.servicePlanTypeId, result.servicePlanId, requestTypeId);
    }
    $("#IPId").val(result.ipId);
    $("#SubscriberName").val(result.subscriberName);

    if (requestTypeId == 32) {
        $("#selectAirMac").empty();
        var airmacItem = "<option selected disabled>" + result.airMac + "</option>";
        $("#selectAirMac").html(airmacItem);
        $("#selectAirMac").prop("disabled", true);
        $("#selectModemModel").empty();
        var modemItem = "<option selected disabled>" + result.modemModel + "</option>";
        $("#selectModemModel").html(modemItem);
        $("#selectModemModel").prop("disabled", true);
    }
    else if (requestTypeId == 69) {
        $("#txtCurrentModem").val(result.modemModel);
        $("#txtCurrentAirmac").val(result.airMac);
    }
    $('#ipChangeTo').empty();
    var $options = $("#IPId > option").clone();
    $('#ipChangeTo').append($options);
    $("#ipPlan").val(result.ipId);
    $("#ipPlan").prop("disabled", true);
    $("#ipChangeTo option[value=" + result.ipId + "]").remove();
    var items = '';
    items += "<option>" + result.airMac + "</option>";
    $('#currentAirMac').empty();
    $("#currentAirMac").html(items);
    $("#currentAirMac").prop("disabled", true);
                    //$('#NewAirMac').empty();
                    //var $options = $("#AirMac > option").clone();
                    //$('#NewAirMac').append($options);
                    //$("#currentAirMac").val(result.airMac);
                    //$("#currentAirMac").prop("disabled", true);
                    //$("#NewAirMac option[value=" + result.airMac + "]").remove();
}

getModemModels = function () {

    $.ajax({
        url: '/Order/GetModemModels',
        type: 'get',
        dataType: 'json',
        success: function (result) {
            var items = '';
            $.each(result, function (i, plan) {
                items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
            });
            $("#selectModemModel").empty();
            $("#selectModemModel").html(items);
            $("#selectModemModel").prepend("<option value=''>Select</option>").val('');
            $("#selectModemModel").prop("disabled", false);
        }
    });

}
getAIRMACs = function (customerId, modemModelId, section) {

        $.ajax({
            url: '/Order/GetAIRMACs',
            data: { customerId: customerId, modemModelId: modemModelId },
            type: 'get',
            dataType: 'json',
            success: function (result) {
                var items = '';
                var select = '';
                $.each(result, function (i, plan) {
                    items += "<option value='" + plan.value + "'>" + plan.text + "</option>";
                });
                if (section == 'new-site') {
                    select = $("#selectAirMac");
                }
                else if (section=='modem-swap') {
                    select = $("#selectNewAirmac");
                }
                select.empty();
                select.html(items);
                select.prepend("<option value=''>Select</option>").val('');
                select.prop("disabled", false);
            }
        });

}






