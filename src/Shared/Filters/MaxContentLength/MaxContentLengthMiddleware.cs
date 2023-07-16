using Azure.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Hosting;
using Shared.Constants;
using Shared.Helpers;
using System.Net;
using System.Reflection;

namespace Shared.Filters.MaxContentLength;

internal class MaxContentLengthMiddleware : IFunctionsWorkerMiddleware
{
    // Todo caching using distributed cache (Redis probably)
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {

        if (context.TryGetCustomCalledFunctionAttribute<MaxContentLengthAttribute>(out var attribute))
        {
            var (headerExists, contentLength) = await context.GetContentLengthAsync();

            if (!headerExists)
            {
                await context.CreateResponseAsync<object?>(null,
                HttpStatusCode.LengthRequired);

                return;
            }
            if (contentLength > attribute!.Length)
            {
                await context.CreateResponseAsync(new
                {
                    Error = $"Max content length is: {attribute!.Length}",
                    attribute!.Length
                },
                HttpStatusCode.RequestEntityTooLarge);

                return;
            }

            await next.Invoke(context);
        }
    }
}

public static class MaxContextLengthMiddlewareRegistration
{
    public static IFunctionsWorkerApplicationBuilder UseMaxContentLengthAttribute(this IFunctionsWorkerApplicationBuilder app)
    {
        app.UseMiddleware<MaxContentLengthMiddleware>();

        return app;
    }
}
