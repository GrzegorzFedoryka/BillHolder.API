using Azure;
using Azure.Storage.Blobs;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Features.UploadBill;

internal sealed class UploadBillCommandHandler : IRequestHandler<UploadBillCommand, Result<bool>>
{
    private readonly BlobServiceClient _client;

    public UploadBillCommandHandler(BlobServiceClient client)
    {
        _client = client;
    }

    public async Task<Result<bool>> Handle(UploadBillCommand request, CancellationToken cancellationToken)
    {
        var containerClient = _client.GetBlobContainerClient("name"); // todo replace name with tenant blob id
        try
        {
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }
        catch(RequestFailedException ex)
        {
            // todo handle exception
            return new Result<bool>(ex);
        }
        
        var blobClient = containerClient.GetBlobClient("name2222"); // todo replace name2222 with unique guid connected with db id
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