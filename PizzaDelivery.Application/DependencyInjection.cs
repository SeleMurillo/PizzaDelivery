using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PizzaDelivery.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registrar MediatR (todos los handlers de esta assembly)
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Registrar FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registrar AutoMapper
            // Provide an explicit configuration delegate to disambiguate overloads.
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            return services;
        }
    }  
}
