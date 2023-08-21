using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Persistence;

internal class BillCosmosSerializer : CosmosSerializer
{
    private readonly JsonSerializer _serializer;
    public BillCosmosSerializer()
    {
        _serializer = new JsonSerializer()
        {

        };
    }
    public override T FromStream<T>(Stream stream)
    {
        throw new NotImplementedException();
    }

    public override Stream ToStream<T>(T input)
    {
        throw new NotImplementedException();
    }
}
