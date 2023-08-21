using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.ReadModels;

internal record BillReadModel(
    Guid Id,
    string Name,
    string Url
    );