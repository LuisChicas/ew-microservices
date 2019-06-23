function openNav() {
    document.getElementById("navbar-mobile-extra").style.height = "5rem";
    document.getElementById("navbar-mobile-extra").style.marginTop = "-5rem";
    document.getElementById("navbar").style.marginBottom = "5rem";
    document.getElementById("menu-btn").style.opacity = "0";
    document.getElementById("menu-btn").style.zIndex = "1";
    document.getElementById("menu-close-btn").style.opacity = "1";
    document.getElementById("menu-close-btn").style.zIndex = "2";
}

function closeNav() {
    document.getElementById("navbar-mobile-extra").style.height = "0";
    document.getElementById("navbar-mobile-extra").style.marginTop = "0";
    document.getElementById("navbar").style.marginBottom = "0";
    document.getElementById("menu-btn").style.opacity = "1";
    document.getElementById("menu-btn").style.zIndex = "2";
    document.getElementById("menu-close-btn").style.opacity = "0";
    document.getElementById("menu-close-btn").style.zIndex = "1";
}