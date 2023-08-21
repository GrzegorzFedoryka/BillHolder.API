using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Config;

public static class ServicesConfig
{
    public static IServiceCollection RegisterSharedServices(
        this IServiceCollection services, 
        bool isDevelopment = false)
    {
        return isDevelopment 
            ? services.AddScoped<IUserService, DevUserService>()
            : services.AddScoped<IUserService, UserService>();
    }
}
