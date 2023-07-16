using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared;

public abstract class ModuleBase
{
    protected IMediator Mediator { get; }

    protected ModuleBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}
