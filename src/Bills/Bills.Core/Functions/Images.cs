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
using Bills.Core.Features.GetAllBills;

namespace Bills.Core.Functions;

public sealed class Images : ModuleBase
{
    private readonly ILogger _logger;

    public Images(
        ILoggerFactory loggerFactory,
        IMediator mediator) : base(mediator)
    {
        _logger = loggerFactory.CreateLogger<Images>();
    }

    [Function("uploadImage")]
    [MaxContentLength(25_000_000)]
    public async Task<HttpResponseData> UploadImage([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "images")] HttpRequestData req)
    {
        using var command = new UploadBillCommand(req);

        var result = await Mediator.Send(command);

        var response = result.Match(
            success => req.CreateResponse(HttpStatusCode.OK),
            fail =>
            {
                var r = req.CreateResponse(HttpStatusCode.BadRequest);
                r.WriteString(fail.Message);
                return r;
            });

        return response;
    }

    [Function("getAllImages")]
    public async Task<HttpResponseData> GetAllImages([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images")] HttpRequestData req)
    {
        var query = new GetAllBillsQuery(req);

        var result = await Mediator.Send(query);

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
