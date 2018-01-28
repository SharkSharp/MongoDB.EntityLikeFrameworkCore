using MongoDB.EntityLikeFrameworkCore.Example.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.EntityLikeFrameworkCore.Example.Core;

namespace MongoDB.EntityLikeFrameworkCore.Example.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExampleContext context;

        public UserRepository(ExampleContext context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users
                   .Find(x => true)
                   .ToEnumerable();
        }

        public User FindOne(string id)
        {
            return context.Users
                   .Find(x => x.Id == id)
                   .FirstOrDefault();
        }

        public User InertUser(User newUser)
        {
            context.Users.InsertOne(newUser);
            return newUser;
        }

        public bool DeleteUser(string id)
        {
           return context.Users.DeleteOne(x => x.Id == id)
                  .DeletedCount > 0;
        }

        public User UpdateUser(string id, User user)
        {
            return context.Users.FindOneAndUpdate<User>(x => x.Id == id,
                                                        Builders<User>.Update
                                                        .Set(x => x.Name, user.Name)
                                                        .Set(x => x.Email, user.Email)
                                                        .Set(x => x.BirthDate, user.BirthDate)
                                                        .Set(x => x.Password, user.Password),
                                                        new FindOneAndUpdateOptions<User>()
                                                        {
                                                            ReturnDocument = ReturnDocument.After
                                                        });
        }
    }
}
