using System;

namespace Dcf.Wwp.Model.Interface.FileUpload
{
	public interface ICMServiceRequest
	{
	    string CMServiceURL { get; set; }
	    string CMServerName { get; set; }
	    string CMUserID     { get; set; }
	    string CMUserPwd    { get; set; }
	}
	public class CMServiceRequest : ICMServiceRequest
	{
		public string CMServiceURL { get; set; }
		public string CMServerName { get; set; }
		public string CMUserID     { get; set; }
		public string CMUserPwd    { get; set; }
	}
}