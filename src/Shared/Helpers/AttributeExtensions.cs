using Microsoft.Azure.Functions.Worker;
using Shared.Filters.MaxContentLength;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;

public static class AttributeExtensions
{
    public static bool TryGetCustomCalledFunctionAttribute<Tattribute>(
        this FunctionContext context,
        out Tattribute? attribute) where Tattribute : Attribute
    {
        // attribute extraction from here https://github.com/Azure/azure-functions-dotnet-worker/issues/938
        // module assembly
        var workerAssembly = Assembly.GetEntryAssembly()!;

        // entryPoint is namespace + type + method, we need to isolate the namespace+type and method separately
        var entryPointParts = context.FunctionDefinition.EntryPoint.Split(".");

        // after splitting on "." the typename should be every part except that last
        var workerTypeName = string.Join(".", entryPointParts[..^1]);
        // which is the method name
        var workerFunctionName = entryPointParts[^1];

        // use reflection to get the MethodInfo
        var workerType = workerAssembly!.GetType(workerTypeName);
        var workerFunction = workerType!.GetMethod(workerFunctionName);

        attribute = workerFunction!.GetCustomAttribute<Tattribute>();

        return attribute is not null;
    }
}
