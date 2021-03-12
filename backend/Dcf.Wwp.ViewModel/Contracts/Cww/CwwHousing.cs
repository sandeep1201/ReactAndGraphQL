using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts.Cww
{
	[DataContract]
	public class CwwHousing
    {
		[DataMember(Name = "address")]
		public string Address { get; set; }

		[DataMember(Name = "city")]
		public string City { get; set; }

		[DataMember(Name = "state")]
		public string State { get; set; }

		[DataMember(Name = "zip")]
		public string Zip { get; set; }

		[DataMember(Name = "subsidized")]
		public string Subsidized { get; set; }

		[DataMember(Name = "rentObligation")]
		public string RentObligation { get; set; }


	}
}
