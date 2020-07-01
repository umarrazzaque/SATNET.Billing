$(function () {
    //dropdown filters change events
    $("#UserTypeId").change(function () {
        getUsers();
    });
    $("#ddlOrderStatus").change(function () {
        getUsers();

    });

    //    var t = $("#grid_table").DataTable({

    //        "paging": true,
    //        "lengthChange": true,
    //        "searching": true,
    //        "ordering": true,
    //        "info": true,
    //        "autoWidth": false,
    //        "responsive": true,
    //        "columnDefs": [
    //            { "orderable": false, "targets": 3 },
    //            { "orderable": false, "targets": 5 }
    //        ],
    //        //"columnDefs": [
    //        //    {
    //        //        'targets': 0, // column index (start from 0)bb
    //        //        'orderable': false, // set orderable false for selected columns
    //        //    }],
    //    });
    //    t.on('order.dt search.dt', function () {
    //        t.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //            cell.innerHTML = i + 1;
    //        });
    //    }).draw();
});


getUsers = function () {
    var url = '/Order/GetUsers';
    $.getJSON(url, { requestTypeValue: $("#ddlRequestType").val(), statusValue: $("#ddlOrderStatus").val() }, function (result) {
        if (result.isValid === true && result.html !== null) {
            $("#grid_table").html(result.html);
        }
    });
}
