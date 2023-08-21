using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions;

public class BillHolderExceptionBase : ApplicationException
{
	public BillHolderExceptionBase() : base() { }
	public BillHolderExceptionBase(string message) : base(message) { }
	public BillHolderExceptionBase(string message, Exception? innerException) : base(message, innerException) { }
	protected BillHolderExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }


}
