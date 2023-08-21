using Shared.Exceptions;
using Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ValueObjects;

public class UserId : SingleValueObject<Guid>
{
    public UserId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidUserIdException();
        }
    }

    public static implicit operator Guid(UserId id) => id.Value;
    public static implicit operator UserId(Guid id) => new(id);
}