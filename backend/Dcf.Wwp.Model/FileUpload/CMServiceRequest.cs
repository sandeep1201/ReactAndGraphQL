using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.FileUpload;


namespace Dcf.Wwp.Model.FileUpload
{
	public class CMServiceRequest: ICMServiceRequest
	{
		public string CMServiceURL { get; set; }
		public string CMServerName { get; set; }
		public string CMUserID     { get; set; }
		public string CMUserPwd	   { get; set; }
	}
}