using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Dcf.Wwp.Api.Core.Http
{
	public class HttpConflictResult : StatusCodeResult
	{
		public HttpConflictResult() : base(StatusCodes.Status409Conflict)
		{
		}
	}
}