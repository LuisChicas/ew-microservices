$(document).ready(function () {

    $("a[data-delete-message]").click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var message = $this.data("delete-message");

        if (message && !confirm(message))
            return;

        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .appendTo(document.body)
            .submit();
    });
});