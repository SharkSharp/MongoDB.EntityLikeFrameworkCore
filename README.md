# MongoDB.EntityLikeFrameworkCore
Library designed to facilitate the use of MongoDb in ASP.NET Core applications, based on the use of EntityFramework.


## What is MongoDB.EntityLikeFrameworkCore?
The purpose of the library is not to be an ORM, but to provide an interface between the MondoDB.Driver and the ASP.NET Core application, providing a Context that manages the driver, offering a CodeFirst interface and automatically initializing its Collections, leaving the developer only the create, annotate its Context and add it to the dependency injector.

## Get Started

Firstly create your models with the appropriate MongoDb.Driver annotations.

```cs
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime BirthDate { get; set; }
    }
```

Then create your context like in EntityFramework.

NOTE: We use the IMongoCollection from MongoDB.Driver instead of DbSet<>.

```cs
    [Database("NewDatabaseName")]
    public class ExampleContext : MongoContext
    {
        public ExampleContext(MongoDbContextOptions<ExampleContext> options)
            : base(options) { }

        [Collection("NewDatabaseCollection")]
        public IMongoCollection<User> Users { get; set; }
    }
```

At the beginning of the application Context will connect to the database, calculate the differences between the database and the code, and create the new collections.

Finally, register the context in dependency injection:

```cs
 public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddMongoDbContext(new MongoDbContextOptions<ExampleContext>("MONGODBCONNECTIONSTRING"));
            ...
        }
```

All set up.
Now just put the Context as dependency and use. The context automatically looks at the Collections added by the developer and automatically adds a reference to them.

## Next desired features:

* Add data annotations for Models to describe indexes.
* Optimize Context to allocate instances only on demand.
* Add a utils to help in some queries.
* Add appropriate support for MongoDB Options
