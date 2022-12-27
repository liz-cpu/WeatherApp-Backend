# WeatherApp Backend

This API provides a web server built in VB.NET that can be used to handle HTTP requests from a Kotlin application.
## Requirements

    .NET Framework 4.8 or higher
    Visual Studio 2020 or later

## Getting Started

    Clone or download the repository to your local machine
    Open the repository in Visual Studio
    Build the repository
    Run the application

## Endpoints

The API has the following endpoints:

    GET /: Returns a simple message to confirm the server is running
    POST /login: login
    POST /register: register
    POST /add: add a new favorite location
    POST /delete: delete a favorite location
    POST /all: get all favorite locations
