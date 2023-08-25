using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Bills.Core.Persistence;
using Shared.Config;
using Shared.Serialization;
using System.Text.Json;

namespace Bills.Core.Config;

internal static class DbConfig
{
    internal static CosmosDb GetCosmosBillsDb(IConfiguration cosmosConfig)
    {
        var databaseName = cosmosConfig.GetSection("databaseName").Value;
        var containerName = cosmosConfig.GetSection("containerName").Value;
        var account = cosmosConfig.GetSection("account").Value;
        var key = cosmosConfig.GetSection("key").Value;
        var converters = SerializationConfig.GetSerializationConverters<IBillsMarker>();
        var serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = new PrivateConstructorContractResolver()

        };

        foreach (var converter in converters) 
        {
            serializerOptions.Converters.Add(converter);
        }
        var serializer = new CosmosSystemTextJsonSerializer(serializerOptions);
        var client = new CosmosClient(account, key, new CosmosClientOptions
        {
            HttpClientFactory = () =>
            {
                HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                return new HttpClient(httpMessageHandler);
            },
            ConnectionMode = ConnectionMode.Gateway,
            Serializer = serializer,
            
            
        });
        var database = client.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult().Database;
        var container = database.CreateContainerIfNotExistsAsync(containerName, "/id").GetAwaiter().GetResult();

        return new CosmosDb(container.Container);
    }
}
