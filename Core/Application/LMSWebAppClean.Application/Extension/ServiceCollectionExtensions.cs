using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.Behavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LMSWebAppClean.Application.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationValidation(this IServiceCollection services, Assembly assembly)
        {
            // Register the validation behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Auto-register all validators
            services.AddValidators(assembly);

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
        {
            // Find all validator implementations
            var validatorTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
                .ToList();

            foreach (var validatorType in validatorTypes)
            {
                // Get the IValidator<T> interface
                var validatorInterface = validatorType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                // Register as transient
                services.AddTransient(validatorInterface, validatorType);
            }

            return services;
        }

        public static IServiceCollection AddApplicationValidationFromAssemblyContaining<T>(this IServiceCollection services)
        {
            return services.AddApplicationValidation(typeof(T).Assembly);
        }
    }
}