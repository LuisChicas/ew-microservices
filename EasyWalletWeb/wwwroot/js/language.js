$(document).ready(function () {
    $("#lang-en").click(function (e) {
        e.preventDefault();
        setLanguage($(this).attr("href"), "en");
    });

    $("#lang-es").click(function (e) {
        e.preventDefault();
        setLanguage($(this).attr("href"), "es");
    });
});

function setLanguage(path, language) {
    var url = path + "?language=" + language + "&returnUrl=" + window.location.pathname;

    $("<form>")
        .attr("method", "post")
        .attr("action", url)
        .appendTo(document.body)
        .submit();
}