$(document).ready(function () {

    // Index

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

    // Form

    $(".new-category-tag").not(".tag-template").find("a").each(function (index) {
        $(this).click(function (e) {
            e.preventDefault();
            deleteTag(index);
        });
    });

    var tagsInput = document.getElementById("tags-input");
    var existingTags = $(".new-category-tag").not(".tag-template").length;
    if (tagsInput != null && existingTags == 0) {
        var message = $("#tags-input").data("empty-message");
        tagsInput.setCustomValidity(message);
    }

    $("#add-tag-btn").click(function (e) {
        e.preventDefault();
        addTag($("#tags-input").val());
    });

    $("#tags-input")
        .keyup(function (e) {
            if ($("#tags-input").val().trim().length > 0)
                $("#add-tag-btn").prop("disabled", false);
            else
                $("#add-tag-btn").prop("disabled", true);
        })
        .keydown(function (e) {
            if (e.which != 13)
                return;

            e.preventDefault();
            addTag($("#tags-input").val());
        });

    function addTag(name) {

        var duplicatedTag = getDuplicatedTag(name);
        if (duplicatedTag.length > 0) {
            alert($("#tags-container").data("duplicated-message"));

            $("#tags-container")
                .find("input")
                .filter(function (i, e) { return e.getAttribute("value") == name; })
                .addClass("duplicated-tag");

            return;
        }

        tagsInput.setCustomValidity('');

        var newIndex = $("#tags-container").find("li").length - 1;

        $newTag = $("#tags-container")
            .find(".tag-template")
            .clone()
            .removeClass("tag-template")
            .addClass("tag-" + newIndex)
            .find("span").text(name).end()
            .find("#new-tag-name-input").val(name).attr("name", "Tags[" + newIndex + "].Name").end();

        $newTag.appendTo($("#tags-container"));

        $newTag.find("a").click(function (e) {
            e.preventDefault();
            deleteTag(newIndex);
        });

        $("#tags-input").val("");
        $("#add-tag-btn").prop("disabled", true);
    }

    function deleteTag(index) {
        $(".tag-" + index)
            .find("#new-tag-name-input").val("").end()
            .hide();
    }

    function getDuplicatedTag(tagName) {
        $tag = $("#tags-container")
            .find("input")
            .filter(function (i, e) { return e.getAttribute("value") == tagName; })
            .first();

        return $tag;
    }
});