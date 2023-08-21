using Bills.Core.Exceptions;
using Shared.Serialization;
using Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.ValueObjects;

internal class BillId : SingleValueObject<Guid>
{
	public BillId(Guid value) : base(value)
	{
        if (value == Guid.Empty)
        {
            throw new InvalidBillIdException();
        }
    }

    public static implicit operator Guid(BillId id) => id.Value;
    public static implicit operator BillId(Guid id) => new(id);
}