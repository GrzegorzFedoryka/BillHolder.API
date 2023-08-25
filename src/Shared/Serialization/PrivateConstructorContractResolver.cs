using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Serialization;

public class PrivateConstructorContractResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && jsonTypeInfo.CreateObject is null)
        {
            if (jsonTypeInfo.Type
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(x => x.GetParameters().Length == 0))
            {
                // The type has parameterless non-public constructor
                jsonTypeInfo.CreateObject = () =>
                    Activator.CreateInstance(jsonTypeInfo.Type, true)!;
            }
        }

        return jsonTypeInfo;
    }
}