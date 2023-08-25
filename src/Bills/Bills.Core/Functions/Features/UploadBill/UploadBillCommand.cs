using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using Azure.Storage.Blobs.Models;
using SixLabors.ImageSharp.Formats;
using LanguageExt.Common;

namespace Bills.Core.Functions.Features.UploadBill;

internal sealed class UploadBillCommand : IRequest<Result<bool>>, IDisposable
{
    internal UploadBillCommand(HttpRequestData data)
    {
        File = data.Body;
    }
    public Stream File { get; } = default!;
    public IImageFormat? Format { get; set; }

    public void Dispose()
    {
        File?.Dispose();
    }
}
