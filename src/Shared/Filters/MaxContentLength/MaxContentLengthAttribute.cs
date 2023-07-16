using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Filters.MaxContentLength;

[AttributeUsage(AttributeTargets.Method)]
public class MaxContentLengthAttribute : Attribute
{
    public MaxContentLengthAttribute(long length)
    {
        Length = length;
    }

    public long Length { get; }
}
