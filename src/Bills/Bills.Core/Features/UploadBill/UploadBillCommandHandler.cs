using Azure;
using Azure.Storage.Blobs;
using Bills.Core.Entities;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Persistence;
using Shared.Services;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ValueObjects;
using Bills.Core.Persistence;

namespace Bills.Core.Features.UploadBill;

internal sealed class UploadBillCommandHandler : IRequestHandler<UploadBillCommand, Result<bool>>
{
    private readonly BlobServiceClient _client;
    private readonly IUserService _userService;
    private readonly CosmosDb _db;

    public UploadBillCommandHandler(
        BlobServiceClient client,
        IUserService userService,
        CosmosDb db)
    {
        _client = client;
        _userService = userService;
        _db = db;
    }

    public async Task<Result<bool>> Handle(UploadBillCommand request, CancellationToken cancellationToken)
    {
        if (!_userService.TryGetUserId(out Guid? userId))
        {
            return new Result<bool>(new ClaimNotFoundException(nameof(userId)));
        }

        // todo add container caching
        var containerClient = _client.GetBlobContainerClient(userId.ToString());

        try
        {
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }
        catch(RequestFailedException ex)
        {
            // todo handle exception
            // todo add some retry strategy/queueing
            return new Result<bool>(ex);
        }

        var bill = Bill.Create(new UserId(userId!.Value));

        await _db.Container.CreateItemAsync(
            bill,
            cancellationToken : cancellationToken);

        var blobClient = containerClient.GetBlobClient(bill.Id.Value.ToString()); 
        try
        {
            await blobClient.UploadAsync(request.File, cancellationToken);
        }
        catch(RequestFailedException ex)
        {
            // todo handle exception
            return new Result<bool>(ex);
        }

        return true;
    }
}