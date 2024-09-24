using Inventory.DataAccess.Context;
using Inventory.DataAccess.IdentityManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseConfig(this IServiceCollection services,
         IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                dbContextOptions => dbContextOptions
                    .UseSqlServer(configuration.GetConnectionString("DatabaseConnectionString")));

            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddIdentity<ApplicationIdentityUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}
