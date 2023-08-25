using Bills.Core.ReadModels;
using LanguageExt.Common;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Functions.Features.GetAllBills;

internal sealed class GetAllBillsQuery : IRequest<Result<IEnumerable<BillReadModel>>>
{
    internal GetAllBillsQuery(HttpRequestData data)
    {

    }
}