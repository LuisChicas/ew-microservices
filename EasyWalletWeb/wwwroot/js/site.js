function openNav() {
    document.getElementById("navbar-mobile").style.height = "11rem";
    document.getElementById("navbar-mobile").style.marginTop = "-11rem";
    document.getElementById("navbar").style.marginBottom = "11rem";
    document.getElementById("menu-btn").style.opacity = "0";
}

function closeNav() {
    document.getElementsById("navbar-mobile").style.height = "0";
    document.getElementsById("navbar-mobile").style.marginTop = "0";
    document.getElementById("navbar").style.marginBottom = "0";
    document.getElementById("menu-btn").style.opacity = "1";
}