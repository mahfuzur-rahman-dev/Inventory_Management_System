using Inventory.DataAccess.UnitOfWork;
using Inventory.Service.Features.Services;
using Inventory.Service.Features.Services.IServices;

namespace Inventory.Presentation.Extension
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterWebServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryManagementService, CategoryManagementService>();
            services.AddScoped<IProductManagementService, ProductManagementService>();
            services.AddScoped<IOrderDetailManagementService, OrderDetailManagementService>();

            return services;
        }
    }
}
