using Microsoft.Extensions.DependencyInjection;
using Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Config;

public static class SerializationConfig
{
    private static JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
    public static IEnumerable<JsonConverter> GetSerializationConverters<Tassembly>()
    {
        var factory = new SingleValueObjectConverterFactory();



        var converterTypesToRegister = Assembly
            .GetAssembly(typeof(Tassembly))!
            .GetTypes()
            .Union(
                Assembly
                .GetAssembly(typeof(ISharedMarker))
                .GetTypes())
            .Where(type =>  
                !type.IsAbstract 
                && !type.IsInterface
                && (type.BaseType?.IsGenericType ?? false)
                && type.BaseType.GetGenericTypeDefinition() == typeof(SingleValueObject<>));

        foreach(var type in converterTypesToRegister)
        {
            yield return factory.CreateConverter(type, options);
        }
    }
}
