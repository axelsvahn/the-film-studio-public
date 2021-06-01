"use strict";

//for global variables
let app = {
    globaltoken: "",
};

//gör i local storage

// (async function main() {
// })();

async function showAllFilms() {
    let filmsResponse = await fetch("/api/v1/films/", {
        method: "GET",
        headers: {
            "Content-type": "application/json; charset=UTF-8",
            Authorization: `Bearer ${localStorage.getItem("filmToken")}`,
        },
    });

    let films = await filmsResponse.json();

    let filmlistDiv = document.querySelector("#filmListDiv");
    let output = "";
    for (let i = 0; i < films.length; i++) {
        output += `<div id="filmItem${i + 1}" class="filmItem">
       <ul class="filmListUl">
       <li class="filmName">${films[i].name}</li>

       <li>Id: ${films[i].filmId}</li>
       <li>Land: ${films[i].country}</li>
       <li>Regissör: ${films[i].director}</li>
       <li>År: ${films[i].releaseYear}</li>
       <li>Kopior tillgängliga: ${films[i].copiesForRent}</li>
       </ul>
       </div>`;
    }
    filmlistDiv.innerHTML = output;
}

async function showRentedFilms() {
    let rentedfilmsResponse = await fetch("/api/v1/rentedfilms/", {
        method: "GET",
        headers: {
            "Content-type": "application/json; charset=UTF-8",
            Authorization: `Bearer ${localStorage.getItem("filmToken")}`,
        },
    });

    let rentedfilms = await rentedfilmsResponse.json();

    let rentedFilmListDiv = document.querySelector("#rentedFilmListDiv");
    let output = "";
    for (let i = 0; i < rentedfilms.length; i++) {
        output += `<div id= "filmItem${i + 1}">
       <ul>
       <li>${i + 1}: ${rentedfilms[i].sourceFilmName}</li>
       <li>Filmens id: ${rentedfilms[i].sourceFilmId}</li>
       </ul>
       </div>`;
    }
    rentedFilmListDiv.innerHTML = output;
}

async function loginUser() {
    console.log("loginform active");

    let loginForm = document.getElementById("loginForm");

    let onsubmit2 = async (e) => {
        e.preventDefault();
        console.log("submit login");

        let email2 = document.getElementById("email2").value;

        let password2 = document.getElementById("inputPassword2").value;

        let response = await fetch("/api/v1/authenticate", {
            method: "POST",
            body: JSON.stringify({
                Email: email2,
                Password: password2,
            }),
            headers: {
                "Content-type": "application/json; charset=UTF-8",
            },
        });

        let result = await response.json();
        app.globaltoken = result.token;
        console.log("global", app.globaltoken);

        storeLogin();
        checkLogin();

        //sudda gamla innehållet i input fields

        document.getElementById("email2").value = "";

        document.getElementById("inputPassword2").value = "";
    };

    loginForm.addEventListener("submit", onsubmit2);
}

async function storeLogin() {
    //matar in token i localStorage
    localStorage.setItem("filmToken", app.globaltoken);
}

async function checkLogin() {
    console.log("checklogin active");
    console.log("token is", localStorage.getItem("filmToken"));
    if (localStorage.getItem("filmToken") !== null) {
        document.getElementById("loginMessage").innerHTML = "Du är inloggad.";
        let logout = document.createElement("div");
        logout.innerHTML =
            '<div id="logoutButtondiv"><button type="button" class="btn btn-dark" id="logoutBut">logga ut</button></div>';
        document.querySelector("#loginMessage").insertAdjacentElement("afterend", logout);
        createLogoutButton();
    }
}

async function createLogoutButton() {
    console.log("createlogoutbutton activated");
    let logoutBut = document.querySelector("#logoutBut");
    logoutBut.addEventListener("click", () => {
        if (app.globaltoken !== "" || localStorage.getItem("filmToken") !== null) {
            app.globaltoken = "";
            localStorage.removeItem("filmToken");
            let main = document.querySelector("#main");
            main.innerHTML =
                "<div> Du har nu loggat ut. Glöm inte att stänga webbläsaren. </div>";
        }
    });
}

async function registerStudio() {
    console.log("registerstudio active");
    let registerUserForm = document.getElementById("registerStudioForm");

    let onsubmit3 = async (e) => {
        e.preventDefault();
        console.log("submit register studio");

        let email = document.getElementById("email").value;

        let password = document.getElementById("password").value;

        let name = document.getElementById("name").value;

        let location = document.getElementById("location").value;

        let presidentName = document.getElementById("presidentName").value;

        let presidentEmail = document.getElementById("presidentEmail").value;

        let phone = document.getElementById("phone").value;

        let response = await fetch("/api/v1/register/filmstudio", {
            method: "POST",
            body: JSON.stringify({
                Email: email,
                Password: password,
                Name: name,
                Location: location,
                PresidentName: presidentName,
                PresidentEmail: presidentEmail,
                PresidentPhoneNumber: phone,
            }),
            headers: {
                "Content-type": "application/json; charset=UTF-8",
                Authorization: `Bearer ${localStorage.getItem("filmToken")}`,
            },
        });

        let result = await response.json();
        console.log(result);

        document.getElementById("email").value = "";

        document.getElementById("password").value = "";

        document.getElementById("name").value = "";

        document.getElementById("location").value = "";

        document.getElementById("presidentName").value = "";

        document.getElementById("presidentEmail").value = "";

        document.getElementById("phone").value = "";
    };

    registerUserForm.addEventListener("submit", onsubmit3);
}

async function rentFilm() {
    let rentForm = document.getElementById("rentForm");

    let onsubmitr = async (e) => {
        e.preventDefault();
        console.log("submit rent");

        let id = document.getElementById("filmid1").value;

        let response = await fetch(`/api/v1/films/${id}/rent`, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8",
                Authorization: `Bearer ${localStorage.getItem("filmToken")}`,
            },
        });

        let result = await response.json();
        console.log("rentresponse:", result);

        document.getElementById("filmid1").value = "";
        showAllFilms();
        showRentedFilms();
    };

    rentForm.addEventListener("submit", onsubmitr);
}

async function returnFilm() {
    let returnForm = document.getElementById("returnForm");

    let onsubmitr = async (e) => {
        e.preventDefault();
        console.log("submit return");

        let id = document.getElementById("filmid2").value;

        let response = await fetch(`/api/v1/films/${id}/return`, {
            method: "PUT",
            headers: {
                "Content-type": "application/json; charset=UTF-8",
                Authorization: `Bearer ${localStorage.getItem("filmToken")}`,
            },
        });

        document.getElementById("filmid2").value = "";
        showAllFilms();
        showRentedFilms();
    };
    returnForm.addEventListener("submit", onsubmitr);
}
