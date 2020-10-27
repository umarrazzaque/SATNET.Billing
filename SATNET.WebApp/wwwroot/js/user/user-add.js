
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
        refreshSelectPicker();
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

refreshSelectPicker = function () {
    var mySelect = $('#CustomerId').selectpicker({

        // text for no selection
        noneSelectedText: 'Nothing selected',

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
}