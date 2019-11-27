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
        public IMongoCollection<User> Users { get => GetCollection<User>(); }
    }
```

You now have the choice of creating a MongoDbBuilder, which will index and seed the collections.

```cs
public class ExampleDbBuilder : MongoDbBuilder<ExampleContext>
{
    private IHostingEnvironment hostingEnvironment;

    public CerberusDbBuilder(IHostingEnvironment hostingEnvironment)
        : base(hostingEnvironment.EnvironmentName == "Test")
    {
        this.hostingEnvironment = hostingEnvironment;
    }
    
    protected override void Index<ModelType>(IMongoCollection<ModelType> collection)
    {
        switch (collection)
        {
            case IMongoCollection<User> users:
                IndexUsers(users);
                return;
        }

        base.Index(collection);
    }
    
    private static void IndexUsers(IMongoCollection<User> users)
    {
        users.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending(x => x.Email),
                                new CreateIndexOptions() { Unique = true });
    }
    
    
    protected override void Seed<ModelType>(IMongoCollection<ModelType> collection)
    {
        switch (collection)
        {
            case IMongoCollection<User> users:
                SeedUsers(users);
                return;
        }
    }
    
    private static void SeedUsers(IMongoCollection<User> users)
    {
        users.InsertMany(new List<User>
        {
            new User()
            {
                Name = "Teste",
                Email = "teste@teste.com",
                Password = "teste123".Sha256(),
                BirthDate = DateTime.Now,
            }
        });
    }
 }
```

If any MongoDbBuilder were registered, at the beginning of the application, Context will connect to the database, calculate the differences between the database and the code and build the new collections.

Finally, register the context in dependency injection:

```cs
 public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddMongoDbContext<ExampleContext>((config, ctxName) => {
                config.ConnectionString = "MONGODBCONNECTIONSTRING";
            })
            .AddMongoDbBuilder(new ExampleDbBuilder(HostingEnvironment));
            ...
        }
```

All set up.
Now just put the Context as dependency and use. The context automatically looks at the Collections added by the developer and automatically adds a reference to them.

## Next desired features:

- [ ] Add data annotations for Models to describe indexes.
- [x] Optimize Context to allocate instances only on demand.
- [ ] Add appropriate support for MongoDB Options.
