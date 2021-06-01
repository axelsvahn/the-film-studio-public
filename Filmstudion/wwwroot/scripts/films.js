"use strict";
(function checkLoginFilmPage() {
    console.log("checklogin for filmpage active");
    console.log("token is", localStorage.getItem("filmToken"));
    if (localStorage.getItem("filmToken") !== null) {
        document.getElementById("loginMessage").innerHTML = "Du är inloggad."

        let logout = document.createElement("div");
        logout.innerHTML = "<div id=\"logoutButtondiv\"><button type=\"button\" class=\"btn btn-dark\" id=\"logoutBut\">logga ut</button></div>";
        document.querySelector("#loginMessage").insertAdjacentElement('afterend', logout);
        createLogoutButton();

        showAllFilms();
        showRentedFilms();
        rentFilm();
        returnFilm();

    }
    else {
        let main = document.querySelector("#main");
        main.innerHTML = "<div>Du måste logga in för att kunna använda denna sida.</div>"

    }
})();


