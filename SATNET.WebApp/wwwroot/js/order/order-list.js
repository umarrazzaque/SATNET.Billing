$(function () {
    var t = $("#grid_table").DataTable({

        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "columnDefs": [
            { "orderable": false, "targets": 0 }
        ],
        //"columnDefs": [
        //    {
        //        'targets': 0, // column index (start from 0)bb
        //        'orderable': false, // set orderable false for selected columns
        //    }],
    });
    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    //dropdown filters change events

    $('#ddlOrderStatus').val(20);

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
            //orderAction(orderId, 21, '');//accepted=21
        }
        else {
            $("#btnRejectOrder").data("id", orderId);
        }
    });

    $(document).on("click", 'a.rejected-order', function (e) {
        e.preventDefault();
        var reason = $(this).data("reason");
        $("#modal-rejectreason .modal-body p").html(reason);

    });


    $(document).on("click", '#btnRejectOrder', function (e) {
        e.preventDefault();
        var orderId = $(this).data("id");
        orderAction(orderId, 22, $("#txtRejectReason").val());//rejected=22

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
            data: { statusValue: $("#ddlOrderStatus").val()},
            type: 'get',
            dataType: "html",
            success: function (html) {
                if (html !== null) {
                    $("#BodyContent").html(html);
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


