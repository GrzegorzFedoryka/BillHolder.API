using Microsoft.Azure.Functions.Worker;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;

public static class HeadersExtensions
{
    public static async Task<(bool HeaderExists, long? Length)> GetContentLengthAsync(this FunctionContext context)
    {
        var result = (false, (long?)null);
        var contextData = await context.GetHttpRequestDataAsync();

        if (contextData is null)
        {
            return result;
        }

        if (!contextData!.Headers.TryGetValues(HeaderNames.ContentLength, out var contentLength))
        {
            return result;
        }

        if (!long.TryParse(contentLength.First(), out long length))
        {
            return result;
        }

        return (true, length);
    }
}
