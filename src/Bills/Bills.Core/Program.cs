using Bills.Core;
using Bills.Core.Config;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Config;
using Shared.Filters.MaxContentLength;
using Shared.Helpers;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            //config.AddUserSecrets(Assembly.GetExecutingAssembly());
            config.AddJsonFile("local.settings.json", true);
        }
    })
    .ConfigureFunctionsWorkerDefaults(app =>
    {
        app.UseMaxContentLengthAttribute();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddMediatR(config =>
        {
            config.AddHandlers<IBillsMarker>();
        });

        services.AddValidation<IBillsMarker>();


        services.AddAzureClients(builder =>
        {
            var blobConnectionString = context.Configuration.GetValue<string>("billsStorage");
            builder.AddBlobServiceClient(blobConnectionString);
        });

        var cosmosConfig = context.Configuration.GetRequiredSection("billsCosmos");

        var container = DbConfig.GetCosmosBillsDb(cosmosConfig);

        services.AddSingleton(container);

        services.RegisterSharedServices(
            context.HostingEnvironment.IsDevelopment());
    })
    .Build();

host.Run();
