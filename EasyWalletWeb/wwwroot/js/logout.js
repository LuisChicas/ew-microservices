$(document).ready(function () {

    $("#logout").click(function (e) {
        e.preventDefault();

        var $this = $(this);
        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .appendTo(document.body)
            .submit();
    });
});