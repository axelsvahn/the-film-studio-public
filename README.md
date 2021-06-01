# Filmstudion (The Film Studio)
CRUD application with RESTful API in ASP .NET Core (.NET 5), MSSQL database (EF Core), and Bootstrap/Vanilla JS frontend. Swagger UI also included.

## 1. Background

Built during spring 2021 as course assignment. Part of higher vocational education: Webbutvecklare .NET (Web Developer .NET), 2020-2022, Jönköping University, Sweden. Name of course: Dynamiska Webbapplikationer 2 (Dynamic Web Applications 2). 

## 2. Technologies/techniques used

* ASP.NET Core (.NET 5) 
* Entity Framework Core
* ASP.NET Core Identity
* Bootstrap/JS for frontend
* Swagger UI
* JSON Web Tokens
* Principles and practices for building a RESTful API

## 3. What I learned

I learned about principles of RESTful API design and how to implement an API using ASP.NET Core.

## 4. How to use (documentation in Swedish)

### 4.1. Starta projektet

Öppna projektet med .sln-filen och kör med 
Visual Studio 2019 eller motsvarande. Kräver .NET 5. 
Nagivera till http://localhost:6600 och registrera en användare
med testdatat (se under) för att testa applikationen.
För att testa funktionalitet som administratör, se undre lista 
på åtkomstpunkter.

### 4.2. Swagger UI

Nagivera till http://localhost:6600/swagger/index.html för
att se UI. 

### 4.3. Portnummer

Port är inställd som http://localhost:6600.
Starta servern i Visual Studio 2019 och gå in på denna sida i browsern för att testa klientgränssnittet utan CORS-problem. 
***

### 4.4. Testdata

Fullständiga "paths" ges i sektion 4.5: Åtkomstpunkter.

#### 1. Testdata för registering och autentisering som admin (POST /register/admin, POST /authenticate)

```
{
"Email": "admin@sff.se", 
"Password": "P@ssw0rd!4"
}
```
#### 2. Testdata för registering och autentisering som vanlig användare (POST /register/user, POST /authenticate)

```
{
"Email": "test@sff.se", 
"Password": "P@ssw0rd!5"
}
```
#### 3. Testdata för registering av studio (POST /register/filmstudio; denna studio är knuten till seedad RentedFilm nr. 2)

Registrerade admins och användare kan registrera en studio. 
Användare registrerar studio i samband med registrering av användaren.

```
{
"Name": "Teststads filmstudio",
"Location": "Testgatan 1, Teststad",
"PresidentName": "Test Testsson",
"PresidentEmail": "test@sff.se",
"PresidentPhoneNumber": 963062769
}
```
#### 4. Testdata för uppdaterande av seedad film som admin (PUT films/1)

ReleaseYear och CopiesForRent är ändrade. 

```
{
"FilmId": 1,   
"Name": "The Net",
"Country": "USA",
"Director": "Irwin Winkler",
"ReleaseYear": 1994, 
"CopiesForRent": 1 
}
```

#### 5. Testdata för skapande av ny film som admin (POST /films)

Bör fungera utan FilmId i body, men ska annars vara "FilmId": 6

```
{
"Name": "Programmet som inte ville kompilera",
"Country": "Sverige",
"Director": "Film Filmsson",
"ReleaseYear": 2021,
"CopiesForRent" : 7
}
```
***

### 4.5. Åtkomstpunkter

#### 4.5.1. Filmer

#### GET http://localhost:6600/api/v1/films/ (öppen åtkomstpunkt)

Listar alla filmer inklusive information om tillgängliga kopior om man är autentiserad.
Listar alla filmer minus information om tillgängliga kopior om man inte är autentiserad. 

#### GET http://localhost:6600/api/v1/films/{filmid} (öppen åtkomstpunkt)

Ger filmen med det angivna idnumret inklusive information om tillgängliga kopior om man är autentiserad.
Ger filmen med det angivna idnumret utan information om tillgängliga kopior om man inte är autentiserad. 

#### GET http://localhost:6600/api/v1/rentedfilms

Visar uthyrda filmer inklusive information om vem som hyrt dem. Endast en admin kan se alla uthyrda filmer. En användare är kopplad till den studio som skapades vid registrering av användaren.
Användaren kan endast se de uthyrda filmerna för den specifika studion. 

#### POST http://localhost:6600/api/v1/films/{filmid}/rent

Hyr den aktuella filmen om antalet utlåningsbara kopior överstiger noll och filmen inte redan är hyrd av studion i fråga. 

#### PUT http://localhost:6600/api/v1/films/{filmid}/return

Lämnar tillbaka den aktuella filmen (och ökar antalet uthyrningsbara exemplar med ett) om filmen är hyrd av studion i fråga.

#### POST http://localhost:6600/api/v1/films/

Skapar ny film. För att testa, kopiera data från en individuell film hämtad med GET, ändra namnet, ta bort FilmId eller sätt det till 6 (om du inte redan lagt till en film) och klistra in i body, ange som raw/JSON i Postman, sänd.

#### PUT http://localhost:6600/api/v1/films/{filmid}

Uppdaterar film. För att testa, kopiera data från en annan fil, ändra namnet och klistra in i body, ange som raw/JSON i Postman. 

#### DELETE http://localhost:6600/api/v1/films/{filmid}

Tar bort film om man är admin.

***

#### 4.5.2. Filmstudior

#### GET http://localhost:6600/api/v1/filmstudios/ (öppen åtkomstpunkt)

Listar alla filmstudios. 

#### GET http://localhost:6600/api/v1/filmstudios/{studioid} (öppen åtkomstpunkt)

Ger filmstudion med det angivna idnumret. 

#### GET http://localhost:6600/api/v1/filmstudios/{name} (öppen åtkomstpunkt)

Ger filmstudion med det angivna namnet. 

#### 3. Registering och autentisering

#### POST http://localhost:6600/api/v1/authenticate

Autentisera admins och användare genom att posta.

#### POST http://localhost:6600/api/v1/register/admin (öppen åtkomstpunkt)

Registera som admin, kan sedan autentisera sig (se testdata överst)  

#### POST http://localhost:6600/api/v1/register/filmstudio

Registera en studio. (kräver autentisering som admin eller görs för användare i samband med registrering som användare.) 

#### DELETE http://localhost:6600/api/v1/filmstudios/{studioid} 

Tar bort studio om man är admin. 