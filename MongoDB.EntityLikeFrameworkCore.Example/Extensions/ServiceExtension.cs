using Microsoft.Extensions.DependencyInjection;
using MongoDB.EntityLikeFrameworkCore.Example.Core;
using MongoDB.EntityLikeFrameworkCore.Example.Repositories;

namespace MongoDB.EntityLikeFrameworkCore.Example.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
