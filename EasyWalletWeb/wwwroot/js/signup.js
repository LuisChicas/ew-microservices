$(document).ready(function (e) {
    var password = document.getElementById("Password");
    var repeat_password = document.getElementById("RepeatPassword");

    function validateMinLength() {
        if (password.value.length < 6) {
            password.setCustomValidity("Password must have at least 6 characters");
        }
        else {
            password.setCustomValidity('');
        }
    }

    function validateMatching() {
        if (password.value != repeat_password.value) {
            repeat_password.setCustomValidity("Passwords don't match");
        } else {
            repeat_password.setCustomValidity('');
        }
    }

    password.onkeyup = validateMinLength;
    password.onchange = validateMatching;
    repeat_password.onkeyup = validateMatching;
});