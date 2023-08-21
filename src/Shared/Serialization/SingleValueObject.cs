using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Serialization;

public abstract class SingleValueObject<Tvalue>
{
    protected SingleValueObject(Tvalue value)
    {
        Value = value;
    }

    public Tvalue Value { get; }
}