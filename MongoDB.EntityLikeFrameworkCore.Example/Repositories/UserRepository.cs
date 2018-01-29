using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Example.Core;
using MongoDB.EntityLikeFrameworkCore.Example.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDB.EntityLikeFrameworkCore.Example.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExampleContext context;

        public UserRepository(ExampleContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var result = await context.Users
                   .FindAsync(x => true);

                return result.ToEnumerable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> FindOne(string id)
        {
            try
            {
                var result = await context.Users
                   .FindAsync(x => x.Id == id);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> InertUser(User newUser)
        {
            try
            {
                await context.Users.InsertOneAsync(newUser);
                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                var result = await context.Users.DeleteOneAsync(x => x.Id == id);

                return result.IsAcknowledged &&
                       result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> UpdateUser(string id, User user)
        {
            try
            {
                var result = await context.Users
                .FindOneAndUpdateAsync<User>(x => x.Id == id,
                                            Builders<User>.Update
                                            .Set(x => x.Name, user.Name)
                                            .Set(x => x.Email, user.Email)
                                            .Set(x => x.BirthDate, user.BirthDate)
                                            .Set(x => x.Password, user.Password),
                                            new FindOneAndUpdateOptions<User>()
                                            {
                                                ReturnDocument = ReturnDocument.After
                                            });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
