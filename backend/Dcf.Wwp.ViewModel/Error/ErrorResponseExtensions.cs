using System;

namespace Dcf.Wwp.Api.Library.Error
{
	public static class ErrorResponseExtensions
	{
		public static ErrorResponse ToErrorResponse(this Exception ex, ErrorResponseCodeEnum codeEnum, string message)
		{
			var er = new ErrorResponse();
			er.Code = (int)codeEnum;
			er.Message = message;
			er.TechnicalDetail = ex.Message;
			er.StackTrace = ex.StackTrace;

			return er;
		}
	}
}