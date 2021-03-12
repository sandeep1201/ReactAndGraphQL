using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWageHourHistoryWageTypeBridgeRepository
    {
        IWageHourHistoryWageTypeBridge NewWageHourHistoryWageTypeBridge(IWageHourHistory parentObject, String user);
    }
}