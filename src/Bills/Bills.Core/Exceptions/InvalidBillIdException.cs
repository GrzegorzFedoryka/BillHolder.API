using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Exceptions;

internal class InvalidBillIdException : BillHolderExceptionBase
{
	public InvalidBillIdException()
	{

	}
}
