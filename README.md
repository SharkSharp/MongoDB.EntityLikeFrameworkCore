# MongoDB.EntityLikeFrameworkCore
Library designed to facilitate the use of MongoDb in ASP.NET Core applications, based on the use of EntityFramework.


## What is MongoDB.EntityLikeFrameworkCore?
The purpose of the library is not to be an ORM, but to provide an interface between the MondoDB.Driver and the ASP.NET Core application, providing a Context that manages the driver, offering a CodeFirst interface and automatically initializing its Collections, leaving the developer only the create, annotate its Context and add it to the dependency injector.
