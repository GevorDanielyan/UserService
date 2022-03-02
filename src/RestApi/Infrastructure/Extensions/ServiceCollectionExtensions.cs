using Domain.Services;
using DataLayer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApi.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace RestApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>(serviceProvider =>
            {
                IConfiguration config = serviceProvider.GetService<IConfiguration>();
                return new UserService(
                    config.GetConnectionString("DefaultConnection"),
                    serviceProvider.GetService<ILogger<UserService>>());
            });
            return services;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
