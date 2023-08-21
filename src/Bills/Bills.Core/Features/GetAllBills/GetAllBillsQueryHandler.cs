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

namespace Bills.Core.Features.GetAllBills
{
    internal class GetAllBillsQueryHandler : IRequestHandler<GetAllBillsQuery, Result<IEnumerable<BillReadModel>>>
    {
        private readonly Container _container;

        public GetAllBillsQueryHandler(Container container)
        {
            _container = container;
        }
        public async Task<Result<IEnumerable<BillReadModel>>> Handle(GetAllBillsQuery request, CancellationToken cancellationToken)
        {
            return await _container
                .GetItemLinqQueryable<BillReadModel>()
                .ToFeedIterator()
                .ToListAsync(cancellationToken);
        }
    }
}
