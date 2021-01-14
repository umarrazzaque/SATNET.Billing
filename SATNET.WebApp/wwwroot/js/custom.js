/**
 * Theme Custom JS
 * ------------------
 * Use this file to add theme specific js 
 */

var PLAN_TYPE = new Map();
PLAN_TYPE.set('Quota', '12');
PLAN_TYPE.set('Unlimited', '13');
PLAN_TYPE.set('Dedicated', '14');
PLAN_TYPE.set('Hybrid', '53');

var PLAN_VALIDITY = 'GB/Mth';

var HARD_TYPE = new Map();
HARD_TYPE.set('Spare', '83');
function Layout() {
}

function init_table_pagination(tableName) {
    var table = $('#' + tableName).DataTable({

        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "columnDefs": [
            { "orderable": false, "targets": 0 }
        ]
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    return table;
}

(function ($) {
    $("ul").on("click",".metismenu li a", function (e) {
   
        console.log('menu item clicked');
        //removing the previous selected menu state
        $('.metismenu li').find('a.active').removeClass('active');
        //adding the state for this parent menu
        $(this).addClass('active');
    });
    $(document).ready(function () {
        $(document).ready(function () {
            //localStorage.clear();   // Uncomment to clear ALL storage.
            // Timer needed because of Bootstrap's animation delay.
            var timer;
            $("ul").on("click", function (e) {
                console.log("Click!");

                // Clear previous timer if any.
                clearTimeout(timer);
                timer = setTimeout(function () {

                    // Get expanded states for each ul.
                    var expanded = [];
                    $("ul").each(function () {
                        var thisExpanded = $(this).attr("aria-expanded");
                        console.log(thisExpanded);

                        if (typeof (thisExpanded) != "undefined") {
                            expanded.push(thisExpanded);
                        } else {
                            expanded.push("undefined");
                        }
                    });
                    var expandedString = JSON.stringify(expanded);
                    console.log(expandedString);
                    localStorage.setItem("ULexpanded", expandedString);
                }, 600);
            });

            // On load, set ul to previous state.
            console.log("---- On Load.");

            // Parse the string back to an array.
            var previousState = JSON.parse(localStorage.getItem("ULexpanded"));
            console.log(previousState);

            // If there is data in locaStorage.
            if (previousState != null) {
                console.log("Setting ul states on...");

                $("ul").each(function (index) {

                    // If the ul was expanded.
                    if (previousState[index] == "true") {
                        console.log("Index #" + index);
                        $(this).addClass("show").attr("aria-expanded", previousState[index]);
                    }
                });
            }

        });
        
        if (localStorage.getItem("TransactionStatus")) {
            var jsonObj = JSON.parse(localStorage.getItem("TransactionStatusMessage"));
            Layout.ShowMessage(jsonObj.text, jsonObj.title, jsonObj.type);
            localStorage.clear();
        }
        else {
            console.log('Custom.js - Local Storage Keys not found');
        }

    });

    'use strict'

    //Load right content area
    Layout.LoadContent = function (url, actionModel) {
        //Layout.Loading(true);
        //$("#MainContents").load(window.location.origin + url, function (response, status, xhr) {
        //    if (status === "error") {
        //        window.location.href = window.location.origin + "/Auth/Login";
        //    }
        //    //Layout.Loading(false);
        //    if (actionModel !== null) {
        //        //Layout.ErrorMsgModel(actionModel);
        //    }
        //});

        //window.location.href = 'https://localhost:44367/Customer';
        //$("#MainContents").html(data.html);
        console.log(window.location.origin + url);
        if (url !== null) {
            window.location.href = window.location.origin + url;
        }
    };

    Layout.ShowMessage = function (text, title, type) {
        if (type === 1) {
            toastr.success(text, title, {
                "closeButton": true,
                "progressBar": true,
            });
        } else if (type === 2) {
            toastr.error(text, title, {
                "closeButton": true,
                "progressBar": true
            });
        }
    };

    Layout.ProcessTransactionMessage = function (isSuccess, errorCode) {
        var jsonObj = {
            "text": "",
            "title": "",
            "type": -1
        };
        localStorage.setItem("TransactionStatus", isSuccess);
        console.log(localStorage.getItem("TransactionStatus"));
        if (isSuccess === true) {
            jsonObj = { text: errorCode, title: "Success Message", type: 1 };
        }
        else {
            jsonObj = { text: errorCode, title: "Error Message", type: 2 };
        }
        localStorage.setItem("TransactionStatusMessage", JSON.stringify(jsonObj));
    };

    Layout.Add = function () {
        console.log('Layout is added');
    };

    //-------------------------Export---------------------------
    $('#excel').click(function () {
        var table_header = [];
        var dt_col_start_index = -1; var dt_col_end_index = -1;
        var table_name = "grid_table";
        var datatable = $('#' + table_name).DataTable();
        var table_rows_array = datatable.rows({ search: 'applied' }).data().toArray();
        var table_column_count = datatable.columns().data().toArray().length;
        var ret_table_rows = new Array();
        var menu_name = $(location).attr('pathname').replace('/','');

        $('#' + table_name + ' > thead > tr > th').each(function () {
            table_header.push($.trim($(this).text()));
        });
        //Set stating and closing index
        if (table_header.length !== 0) {
            //set table column start index
            dt_col_start_index = table_header[0] == '#' ? 1 : 0;
            //set table column end index
            let table_header_col_c = table_header.length;
            dt_col_end_index = table_header[table_header_col_c - 1].toUpperCase() == 'Actions'.toUpperCase() ||
                table_header[table_header_col_c - 1] == '' ? table_header_col_c - 2 : table_header_col_c - 1;
        }
        else {
            //safe values
            dt_col_start_index = 1; dt_col_end_index = table_column_count - 2;
        }
        //Add table header
        let t_col_header = [];
        for (var i = dt_col_start_index, y = 1; i <= dt_col_end_index; i++, y++) {
            t_col_header.push(table_header[i]);
        }
        var table_header_str = t_col_header.join(",");
        //Add table rows
        $.each(table_rows_array, function (index, value) {
            let data = {};
            var propertyName = "Property";
            for (var i = dt_col_start_index, y = 1; i <= dt_col_end_index; i++, y++) {
                let cProp = propertyName + y;
                let res = value[i];
                $("<div></div>").html(value[i]).find("a").each(function (l) {
                    res = $(this).text();
                });
                data[cProp] = res;
            }
            ret_table_rows.push(data);
        });
        //console.log('table header:' + table_header);
        //console.log('table col count:' + table_column_count);
        //console.log('col start index:' + dt_col_start_index);
        //console.log('col end index:' + dt_col_end_index);
        //console.log(ret_table_rows);
        $.ajax({
            ContentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            url: '/Export/ExcelExport',
            data: { "records": ret_table_rows, "header": table_header_str, "menu": menu_name },
            success: function (response) {
                console.log(response);
                if (response.isSuccess === true) {
                    Layout.ShowMessage(response.errorCode, 'Success Message', 1);
                }
                else {
                    Layout.ShowMessage(response.errorCode, 'Error Message', 2);
                }

            },
            error: function (response) {
                Layout.ShowMessage('Something went wrong during the operation!', 'Error Message', 2);
            }
        });
    });
    $('#pdf').click(function () {
        console.log('Click PDF');
        var tab = $('#grid_table').DataTable();
        var dd = tab.rows({ search: 'applied' }).data().toArray();
        var retArray = new Array();
        $.each(dd, function (index, value) {

            let data = {};
            data["Property1"] = value[1];
            data["Property2"] = value[2];
            data["Property3"] = value[3];
            retArray.push(data);
        });
        console.log(retArray);

        $.ajax({
            ContentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            url: '/Export/PDFExport',
            data: { "records": retArray },
            success: function (response) {
                console.log(response);
                if (response.isSuccess === true) {
                    Layout.ShowMessage(response.errorCode, 'Success Message', 1);
                }
                else {
                    Layout.ShowMessage(response.errorCode, 'Error Message', 2);
                }
            },
            error: function (response) {
                Layout.ShowMessage('Something went wrong during the operation!', 'Error Message', 2);
            }
        });
    });
    //-------------------------Export---------------------------

    $(document).on("click", 'a.right-pan', function (e) {
        e.preventDefault();
        var href = $(this).attr("href");
        Layout.LoadContent(href, null);
        return true;
    });

    $(document).on("click", 'a.edit-pan', function (e) {
        e.preventDefault();
        //$.ajax(
        //    {
        //        url: $(this).attr("href"),
        //        type: 'get',
        //        dataType: "json",
        //        success: function (data) {
        //            console.log(data);
        //            $("#MainContents").html(data.html);
        //        },
        //        error: function () {
        //            alert('Error on clicking edit pan');
        //        }
        //    });
    });

    $(document).on("submit", '#formAdd', function (e) {
        console.log('There');
        e.preventDefault();
        $.post($(this).attr("action"),
            $(this).serialize(),
            function (data) {
                Layout.LoadContent(data.responseUrl, null);
                Layout.ProcessTransactionMessage(data.isSuccess, data.errorCode);
            });
    });

    $(document).on("click", 'a.modal-pan', function (e) {
        e.preventDefault();
        console.log('Delete Here');
        $("#modal-deleteConfirm").attr("href", $(this).attr("href"));
        return true;
    });

    $(document).on("click", '#deleteConfirmSubmit', function (e) {
        e.preventDefault();
        var href = $("#modal-deleteConfirm").attr("href");
        $.ajax(
            {
                url: href,
                type: 'get',
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data.isSuccess === true) {
                        Layout.ShowMessage(data.errorCode, 'Success Message', 1);
                    }
                    else {
                        Layout.ShowMessage(data.errorCode, 'Error Message', 2);
                    }
                    if (data.isReload === true) {
                        location.reload();
                    }
                    $("#MainContents").html(data.html);
                },
                error: function () {
                    alert('Error on clicking delete confirmation');
                }
            });

        return false;
    });
    
})(jQuery)
