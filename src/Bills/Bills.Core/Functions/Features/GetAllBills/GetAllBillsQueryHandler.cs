using Bills.Core.Persistence;
using Bills.Core.ReadModels;
using LanguageExt.Common;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Functions.Features.GetAllBills
{
    internal class GetAllBillsQueryHandler : IRequestHandler<GetAllBillsQuery, Result<IEnumerable<BillReadModel>>>
    {
        private readonly CosmosDb _db;

        public GetAllBillsQueryHandler(CosmosDb db)
        {
            _db = db;
        }
        public async Task<Result<IEnumerable<BillReadModel>>> Handle(GetAllBillsQuery request, CancellationToken cancellationToken)
        {
            var x = await _db.Container
                .GetItemLinqQueryable<BillReadModel>()
                .ToFeedIterator()
                .ToListAsync(cancellationToken);

            return x;
        }
    }
}
