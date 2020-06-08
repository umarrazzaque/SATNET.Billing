/**
 * AdminLTE Demo Menu
 * ------------------
 * You should not use this file in production.
 * This file is for demo purposes only.
 */
(function ($) {

    'use strict'

    function ShowMessage(text, title, type) {
        if (type === 1) {
            toastr.success(text, title, {
                "closeButton": true,
                "progressBar": true
            });
        } else if (type === 2) {
            toastr.error(text, title, {
                "closeButton": true,
                "progressBar": true
            });
        }

    };

    $(document).on("click", 'a.right-pan', function (e) {
        e.preventDefault();
        $.ajax(
            {
                url: $(this).attr("href"),
                type: 'get',
                dataType: "json",
                success: function (data) {
                    $("#MainContents").html(data.html);
                },
                error: function () {
                    alert('Error');
                }
            });
    });

    $(document).on("click", 'a.edit-pan', function (e) {
        e.preventDefault();
        $.ajax(
            {
                url: $(this).attr("href"),
                type: 'get',
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    $("#MainContents").html(data.html);
                },
                error: function () {
                    alert('Error');
                }
            });
    });

    $(document).on("submit", '#formAdd', function (e) {
        e.preventDefault();
        $.post($(this).attr("action"),
            $(this).serialize(),
            function (data) {
                if (data.isSuccess === true) {
                    ShowMessage(data.errorCode, 'Success Message', 1);
                }
                else {
                    ShowMessage(data.errorCode, 'Error Message', 2);
                }
                $("#MainContents").html(data.html);
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
                    $("#MainContents").html(data.html);
                },
                error: function () {
                    alert('Error');
                }
            });

        return false;
    });
})(jQuery)
