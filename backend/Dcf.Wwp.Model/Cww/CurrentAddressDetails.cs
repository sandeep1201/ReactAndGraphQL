using Dcf.Wwp.Model.Interface.Cww;


namespace Dcf.Wwp.Model.Cww
{
	public class CurrentAddressDetails : ICurrentAddressDetails
	{
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public bool IsSubsidized { get; set; }
		public decimal? ShelterAmount { get; set; }
	}
}