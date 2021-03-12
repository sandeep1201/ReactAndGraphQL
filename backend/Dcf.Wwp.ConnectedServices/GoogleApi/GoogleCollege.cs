using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
	[DataContract]
	public class GoogleCollege
    {
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "streetAddress")]
		public string StreetAddress { get; set; }
	}
}

