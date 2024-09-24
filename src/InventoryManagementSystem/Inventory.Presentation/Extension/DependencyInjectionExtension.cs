using Inventory.DataAccess.UnitOfWork;

namespace Inventory.Presentation.Extension
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterWebServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
