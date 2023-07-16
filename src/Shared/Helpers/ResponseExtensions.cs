using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;

public static class ResponseExtensions
{
    // taken from https://stackoverflow.com/questions/68350021/azure-function-middleware-how-to-return-a-custom-http-response
    public static async Task CreateResponseAsync<Tinstance>(this FunctionContext context, Tinstance message, HttpStatusCode statusCode)
    {
        // To access the RequestData
        var req = await context.GetHttpRequestDataAsync();

        // To set the ResponseData
        var res = req!.CreateResponse();
        await res.WriteAsJsonAsync(message, statusCode);
        context.GetInvocationResult().Value = res;
        return;
    }
}
