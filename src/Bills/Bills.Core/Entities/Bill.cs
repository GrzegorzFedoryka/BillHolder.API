using Bills.Core.ValueObjects;
using Newtonsoft.Json;
using Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Entities;

internal class Bill
{
    [JsonProperty(PropertyName = "id")]
    public BillId Id { get; private set; }
    public UserId UserId { get; private set; }
    public string? Url { get; private set; }
    public bool IsUploaded => Url is not null;
    private Bill()
    {

    }

    public static Bill Create(UserId id)
    {
        return new Bill()
        {
            Id = Guid.NewGuid(),
            UserId = id
        };
    }
}
