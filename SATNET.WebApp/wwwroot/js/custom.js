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
HARD_TYPE.set('Spare', '77');
function Layout() {
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

                    // Show it in console.
                    var expandedString = JSON.stringify(expanded);
                    console.log(expandedString);

                    // Save it in Storage.
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
