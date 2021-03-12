using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
	public interface IExceptionAudit
	{
		Int32 Id { get; set; }
		String CurrentPage { get; set; }
		DateTime? ExceptionTimestamp { get; set; }
		String PreviousPage { get; set; }
		String IPAddress { get; set; }
		String ServiceName { get; set; }
		String UserId { get; set; }
		String WAMSLoginId { get; set; }
		String ExceptionText { get; set; }
		String ServiceMessageStackText { get; set; }
		String ServerName { get; set; }
		Int32? ExceptionSeverity { get; set; }
	}
}
