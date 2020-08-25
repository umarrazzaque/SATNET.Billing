/**
 * Theme Custom JS
 * ------------------
 * Use this file to add theme specific js 
 */
function Layout() {
}

(function ($) {
    $(document).ready(function () {
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
        window.location.href = window.location.origin + url;
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
