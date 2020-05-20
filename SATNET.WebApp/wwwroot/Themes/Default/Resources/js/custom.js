/**
 * AdminLTE Demo Menu
 * ------------------
 * You should not use this file in production.
 * This file is for demo purposes only.
 */
(function ($) {

    
  'use strict'

    function ShowMessage(text, title, type) {
        toastr.success(text, title, {
            "closeButton": true,
            "progressBar": true
        });
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
                ShowMessage('Are you the 6 fingered man?', 'Success Message',1);
                $("#MainContents").html(data.html);
            });
    });
})(jQuery)
