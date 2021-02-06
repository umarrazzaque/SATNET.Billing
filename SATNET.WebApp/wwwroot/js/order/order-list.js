$(function () {

    $(document).ready(function () {
        init_table_pagination('grid_table');
    });
    
    //dropdown filters change events

    $('#selectOrderStatus').val(20);

    $(".filters select").change(function () {
            GetOrdersByDDLFilter();
    });

    $(document).on("click", 'a.modal-pan', function (e) {
        e.preventDefault();
        $("#modal-deleteConfirm .modal-header h6").html('Order Cancel Confirmation');
        $("#modal-deleteConfirm .modal-body p").html('Are you sure you want to cancel the service order?');
    });

    //$(document).on("click", 'a.orderdetails', function (e) {
    //    e.preventDefault();
    //    var href = $(this).attr("href");
    //    $.ajax(
    //        {
    //            url: href,
    //            type: 'get',
    //            dataType: "json",
    //            success: function (data) {
    //                console.log(data);
    //                $('#pOrderNo').html(data.orderNumber);
    //                $('#pRequestType').html(data.requestTypeName);
    //                $('#pCustomerName').html(data.customerName);
    //                $('#pSiteName').html(data.siteName);
    //                //hideAllSections();
    //                //switch (data.requestTypeId) {
    //                //    case 1://Activation
    //                //        $("#divActivation").show();
    //                //        $('#pSiteName').html('<b>Name:</b> ' + data.siteName);
    //                //        $('#pSiteCity').html('<b>City:</b> ' + data.siteCity);
    //                //        $('#pSiteArea').html('<b>Area:</b> ' + data.siteArea);
    //                //        $('#pSiteInstallationDate').html('<b>Installation Date:</b> ' + data.installationDate);
    //                //        break
    //                //    case 32://Re-Activation
    //                //        $("#divReActivation").show();
    //                //        break
    //                //    case 3://Upgrade
    //                //        $("#divUpgrade").show();
    //                //        break
    //                //    case 4://Downgrade
    //                //        $("#divDowngrade").show();
    //                //        break
    //                //    default:
    //                //}
    //            },
    //            error: function () {
    //                alert('Error on clicking service order');
    //            }
    //        });
    //});

    $(document).on("click", 'a.order-action', function (e) {
        e.preventDefault();
        var orderId = $(this).data("id");
        var action = $(this).data("action");
        if (action == "Accept") {
            $("#btnAcceptOrder").data("id", orderId);
        }
        else if (action == "Cancel") {
            $("#btnRejectOrder").data("id", orderId);
            $("#btnRejectOrder").data("section", 111);//cancelled=111
            $('#modal-rejectOrderConfirm .modal-title').text('Cancel Service Order');
        }
        else if (action == "Reject") {
            $("#btnRejectOrder").data("id", orderId);
            $("#btnRejectOrder").data("section", 22);//rejected=22
        }
    });

    $(document).on("click", 'a.rejected-order', function (e) {
        e.preventDefault();
        var reason = $(this).data("reason");
        var action = $(this).data("action");
        $("#modal-rejectreason .modal-body p").html(reason);
        if (action == 'Cancel') {
            $('#modal-rejectreason .modal-title').text('Reason for Service Order Cancellation');
        }
    });


    $(document).on("click", '#btnRejectOrder', function (e) {
        e.preventDefault();
        var orderId = $(this).data("id");
        var sectionId = $(this).data("section");
        if (orderId > 0 && sectionId > 0) {
            orderAction(orderId, sectionId, $("#txtRejectReason").val());
        }
    });

    $(document).on("click", '#btnAcceptOrder', function (e) {
        e.preventDefault();
        var orderId = $(this).data("id");
        orderAction(orderId, 21, '');//accepted=21
    });

});

GetOrdersByDDLFilter = function () {
    $.ajax(
        {
            url: '/Order/GetOrdersByDDLFilter',
            data: { statusId: $("#selectOrderStatus").val(), scheduleDateId: $("#selectScheduleDate").val()},
            type: 'get',
            dataType: "html",
            success: function (html) {
                if (html !== null) {
                    $("#BodyContent").html(html);
                    init_table_pagination('grid_table');
                }
            },
            error: function () {
                alert('Error occurred while processing the request.');
            }
        });
}

orderAction = function (orderId, statusId, reason) {
    $.ajax(
        {
            url: '/Order/Action',
            data: { id: orderId, statusId: statusId, rejectReason: reason },
            type: 'get',
            dataType: "json",
            success: function (data) {
                if (data.isReload === true) {
                    location.reload();
                }
            },
            error: function () {
                alert('Error occurred while processing the request.');
            }
        });
}

hideAllSections = function () {
    $("#divActivation").hide();
    $("#divReActivation").hide();
    $("#divDowngrade").hide();
    $("#divUpgrade").hide();
}


