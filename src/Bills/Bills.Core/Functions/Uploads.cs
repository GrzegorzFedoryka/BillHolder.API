using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shared.Filters.MaxContentLength;
using Shared;
using Bills.Core.Features.UploadBill;

namespace Bills.Core.Functions;

public sealed class Uploads : ModuleBase
{
    private readonly ILogger _logger;

    public Uploads(
        ILoggerFactory loggerFactory,
        IMediator mediator) : base(mediator)
    {
        _logger = loggerFactory.CreateLogger<Uploads>();
    }

    [Function("uploads")]
    [MaxContentLength(25_000_000)]
    public async Task<HttpResponseData> UploadImage([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        using var command = new UploadBillCommand(req);

        var result = await Mediator.Send(command);

        var response = result.Match(
            success => req.CreateResponse(HttpStatusCode.OK),
            fail => {
                var r = req.CreateResponse(HttpStatusCode.BadRequest);
                r.WriteString(fail.Message);
                return r;
            });

        return response;
    }
}
