$(document).ready(function () {

    function logout(button) {
        $("<form>")
            .attr("method", "post")
            .attr("action", button.attr("href"))
            .appendTo(document.body)
            .submit();
    }

    $("#logout").click(function (e) {
        e.preventDefault();
        logout($(this));
    });
    
    $("#logout-mobile").click(function (e) {
        e.preventDefault();
        logout($(this));
    });
});