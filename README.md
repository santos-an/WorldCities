# WorldCities

![build](https://github.com/santos-an/WorldCities/actions/workflows/build.yml/badge.svg)
![test](https://github.com/santos-an/WorldCities/actions/workflows/test.yml/badge.svg)

## Introduction
This is a sample API, using **Clean Architecture**, **Authentication Authorization**, **CQRS** and **Microsoft .NET 7**. 

We are using some **Functional Programing** paradigms, concepts such as: 
- ``Maybe<T>`` - to avoid Nulls with the Maybe Type
- ``Result`` - to handle failures and Input Errors in a Functional Way

### Context
> The application is an API for a cities management system. We have several endpoints, that require different authentication policies for access (via JWT Bearer Token). 

### The task
> As a full-stack developer, you should implement an autocomplete component of cities (ordered asc), and data of the selected city (country, subcountry and geonameid)
• Use any UI libraries such as bootstrap / material etc. 
• The UI shouldn't be fancy, but a nice-looking UI will be appreciated. 
• The component should be written using React. 
• The server-side should be written in .Net core (3.1/5) 
• Bonus - add meaningful unit tests for your work For your convenience we have attached a sample csv file with cities. 

## Api layer

**Auth endpoint**

- POST `/Auth/Register` - Registers a new user. Returns a new access token
- POST `/Auth/Login` - Validates user credentials. Returns a new access token
- POST `/Auth/UpdateToken` - Validates the expired access token. Returns new access token 

**City endpoint**

- GET `/City/GetAll` - Returns all cities
- GET `/City/GetById` - Returns the city for the given Id
- GET `/City/GetByGeonameId` - Returns the city for the given GeonameId
- GET `/City/GetWithName` - Returns the list of cities, that contain the name in it
- GET `/City/GetWithCountry` - Returns the list of cities, that belongs to a country
- GET `/City/GetWithSubCountry` - Returns the list of cities, that belongs to a sub-country (district)
- POST `/City/Create` - Creates a new city


## Policies
We use policies to secure the endpoints:
- `Standard` - Only authenticated users can access the resource.
- `Administrator` - Only authenticated users, with the Admin role, can access the resource

## Endpoints and policies
- `Auth endpoint` (open to all users)
- `City endpoint` (only authenticated users are able to access this endpoint)

## Roles
We have 2 roles on the system:
- `Admin`
- `Standard`

Users with the *Admin role* can access:
- Standard policies
- Administrator policies

Users with the *Standard role* can access:
- Standard policies. 

When a new user is registered, we automatically assign the 'Standard role'.
To access other resources, we need to attach the 'Admin' role to the new registered user. Please use the `/Roles/AddRoleToUser` post endpoint. 

## Test Users
In order to `use the City endpoint`, you need to get an `access token`. You can either `register` a new user or `login` from an already existed one. 
During the first time, you run the project, we seed the database with random users. To get an access token, you can login with this users:
- `winifred.stoltenberg@yahoo.com`
- `tina5@hotmail.com`
- `erick.fisher78@gmail.com`

All users share the same password credentials: `D;|o)46s__/0$Eb.`

## Technologies
This demo application uses the following technologies:
 - .NET 7
 - C# 11
 - ASP.NET Core MVC 7.0
 - EF Core 7.0
 - Rider 2022
 - SQL Server 2022
 - Bogus 34.0
 - IdentityModel.Tokens.Jwt 6.25
 - Fluent Validation 11.2
