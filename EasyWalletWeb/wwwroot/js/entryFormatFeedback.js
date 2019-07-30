$(document).ready(function () {
    if (isAndroid()) {
        $("#non-android-feedback").remove();
    }
    else {
        $("#android-feedback").remove();
    }

    $("#Entry").focus(function () {
        $(".entry-format-feedback")[0].style.visibility = "visible";
    });

    $("#Entry").blur(function () {
        $(".entry-format-feedback")[0].style.visibility = "hidden";
    });

    $("#Entry").on('keyup', function () {
        validate(this.value);
    });
});

var whiteSpacesRegex = new RegExp(/^\s+$/);
var amountRegex = new RegExp(/^[0-9]+((.|,)\d{1,2})?$/);

function validate(entry) {
    if (entry && entry !== '' && !whiteSpacesRegex.test(entry)) {
        $("#format-keyword").addClass("format-valid");
    }
    else {
        $("#format-keyword").removeClass("format-valid");
        $("#format-amount").removeClass("format-valid");
        $("#format-check").hide();
        return;
    }

    var entryParts = entry.split(" ");
    var filteredEntryParts = [];

    for (var i = 0; i < entryParts.length; i++) {
        if (entryParts[i] !== '') {
            filteredEntryParts.push(entryParts[i]);
        }
    }

    if (filteredEntryParts.length < 2) {
        $("#format-amount").removeClass("format-valid");
        $("#format-check").hide();
        return;
    }

    var amount = filteredEntryParts[filteredEntryParts.length - 1];

    if (amount.includes("$")) {
        amount = amount.replace("$", "");
    }

    if (amountRegex.test(amount)) {
        $("#format-amount").addClass("format-valid");
        $("#format-check").show();
    }
    else {
        $("#format-amount").removeClass("format-valid");
        $("#format-check").hide();
    }
}

function isAndroid() {
    var userAgent = navigator.userAgent || navigator.vendor || window.opera;

    // Windows Phone must come first because its UA also contains "Android"
    if (/windows phone/i.test(userAgent)) {
        return false;
    }

    if (/android/i.test(userAgent)) {
        return true;
    }

    return false;
}