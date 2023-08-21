using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.PipelineBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Config;

public static class MediatRConfig
{
    public static IServiceCollection AddValidation<T>(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        var validatorTypes = (Assembly.GetAssembly(typeof(T)) ?? Assembly.GetCallingAssembly())
            .GetTypes()
            .Where(a => !a.IsAbstract
                && !a.IsInterface
                && typeof(IValidator).IsAssignableFrom(a));

        foreach(var validatorType in validatorTypes)
        {
            foreach(var @interface in validatorType.GetInterfaces())
            {
                services.AddScoped(@interface, validatorType);
            }
        }
        return services;
    }

    public static MediatRServiceConfiguration AddHandlers<T>(this MediatRServiceConfiguration config)
    {
        config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(T)) ?? Assembly.GetCallingAssembly());

        return config;
    }

}
