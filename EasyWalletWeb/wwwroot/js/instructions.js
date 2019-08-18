$(document).ready(function () {
    var instructionsCookiePrefix = "ins_";

    $(".alert-warning a").click(function () {
        var instructionName = $(".alert-warning").data("instruction");
        deleteCookie(instructionsCookiePrefix + instructionName);

        var redirect = $(this).data("redirect");
        if (redirect != undefined) {
            window.location.href = redirect;
        }
        else {
            $(".alert-warning").hide();
        }
    });

    var deleteCookie = function (name) {
        document.cookie = name + '=; path=/; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    };
});