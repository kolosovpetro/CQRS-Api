# CQRS Api

ASP NET Core WEB API with CQRS Architecture.

## Online

- Github: https://github.com/kolosovpetro/CQRS-Api
- Heroku: https://cqrs-movies-api.herokuapp.com/swagger
- Docker: http://localhost:8000/swagger

## Docker commands

- docker-compose build
- docker-compose up
- docker-compose down

## How to run in Docker

- docker-compose buid
- docker-compose up
- cd CqrsApi.Data
- dotnet-ef database update
- http://localhost:8000/swagger
- enjoy

## To do

- Encapsulate responses to entites
- Validate input data
- Implement connection string from heroku
- Provide fake data to swagger for better testing
- Configure serialization to be snake case fields in responses
- Validate movie to be integer grater than first movie in the history