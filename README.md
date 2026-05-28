# Coffee Machine API

## Run

dotnet run --project CoffeeMachineAPI

## Test

dotnet test

## Endpoint

GET /brew-coffee

## Features

- Returns 200 OK with coffee message and prepared timestamp
- Every fifth request returns 503 Service Unavailable with an empty body
- April 1st returns 418 I'm a teapot with an empty body
