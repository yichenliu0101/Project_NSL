function openNav() {
    document.getElementById("mySidenav").style.width = "15%";
    document.getElementById("main").style.paddingLeft = "15%";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main").style.paddingLeft = "0";
}

let navbarEvent = document.getElementById("navbarOpen");
let navbarStatus = document.getElementById("mySidenav");

navbarEvent.addEventListener("click", function(){
    if(navbarStatus.style.width == "0px"){
        openNav();
    }
    else{
        closeNav();
    }
})
