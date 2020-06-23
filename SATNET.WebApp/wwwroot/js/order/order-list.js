$(function () {
    //var t = $("#grid_table").DataTable({

    //    "paging": true,
    //    "lengthChange": true,
    //    "searching": true,
    //    "ordering": true,
    //    "info": true,
    //    "autoWidth": false,
    //    "responsive": true,
    //    "columnDefs": [
    //        { "orderable": false, "targets": 6 }
    //    ],
    //    //"columnDefs": [
    //    //    {
    //    //        'targets': 0, // column index (start from 0)bb
    //    //        'orderable': false, // set orderable false for selected columns
    //    //    }],
    //});
    //t.on('order.dt search.dt', function () {
    //    t.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();

    //dropdown filters change events

    $("#ddlRequestType").change(function () {
        GetOrdersByDDLFilter();
    });
    $("#ddlOrderStatus").change(function () {
        GetOrdersByDDLFilter();

    });
});

GetOrdersByDDLFilter = function () {
    var url = '/Order/GetOrdersByDDLFilter';
    $.getJSON(url, { requestTypeValue: $("#ddlRequestType").val(), statusValue: $("#ddlOrderStatus").val() }, function (result) {
        if (result.isValid === true && result.html !== null) {
            $("#grid_table").html(result.html);
        }
    });
}
