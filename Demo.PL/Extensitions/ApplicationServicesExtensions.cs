using Demo.BLL;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL.Extensitions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

    }
}
