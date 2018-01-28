using System.Collections.Generic;
using MongoDB.EntityLikeFrameworkCore.Example.Models;

namespace MongoDB.EntityLikeFrameworkCore.Example.Core
{
    public interface IUserRepository
    {
        bool DeleteUser(string id);
        User FindOne(string id);
        IEnumerable<User> GetAll();
        User InertUser(User newUser);
        User UpdateUser(string id, User user);
    }
}