using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWageHourWageTypeBridgeRepository
    {
		IWageHourWageTypeBridge NewWageHourWageTypeBridge(IWageHour parentObject, String user);
	}
}