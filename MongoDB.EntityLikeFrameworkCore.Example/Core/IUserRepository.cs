using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.EntityLikeFrameworkCore.Example.Models;

namespace MongoDB.EntityLikeFrameworkCore.Example.Core
{
    public interface IUserRepository
    {
        Task<bool> DeleteUser(string id);
        Task<User> FindOne(string id);
        Task<IEnumerable<User>> GetAll();
        Task<User> InertUser(User newUser);
        Task<User> UpdateUser(string id, User user);
    }
}