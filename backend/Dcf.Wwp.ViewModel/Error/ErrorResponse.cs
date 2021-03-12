using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Error
{
	[DataContract]
	public class ErrorResponse
	{
		[DataMember(Name = "code")]
		public int Code { get; set; }

		[DataMember(Name = "message")]
		public string Message { get; set; }

		[DataMember(Name = "technicalDetail")]
		public string TechnicalDetail { get; set; }

		[DataMember(Name = "stackTrace")]
		public string StackTrace { get; set; }

		public static ErrorResponse Create(ErrorResponseCodeEnum codeEnum, string message, string technicalDetail = "")
		{
			var er = new ErrorResponse();
			er.Code = (int)codeEnum;
			er.Message = message;
			er.TechnicalDetail = technicalDetail;

			return er;
		}
	}
}