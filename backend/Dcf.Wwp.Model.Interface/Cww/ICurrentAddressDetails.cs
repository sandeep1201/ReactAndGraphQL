using System;

namespace Dcf.Wwp.Model.Interface.Cww
{
	public interface ICurrentAddressDetails
	{
		String Address { get; set; }
		String City { get; set; }
		String State { get; set; }
		String Zip { get; set; }
		Boolean IsSubsidized { get; set; }
		Decimal? ShelterAmount { get; set; }
	}
}