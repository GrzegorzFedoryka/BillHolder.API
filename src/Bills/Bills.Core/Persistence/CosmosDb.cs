using Microsoft.Azure.Cosmos;
using Shared.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Persistence;

internal class CosmosDb
{
    public Container Container { get; }

    public CosmosDb(Container container)
	{
        Container = container;
    }
}
