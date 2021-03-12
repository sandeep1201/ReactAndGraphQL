using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWageHourHistoryWageTypeBridgeRepository
    {
        public IWageHourHistoryWageTypeBridge NewWageHourHistoryWageTypeBridge(IWageHourHistory parentObject, string user)
        {
			IWageHourHistoryWageTypeBridge whhwtb = new WageHourHistoryWageTypeBridge();
			whhwtb.WageHourHistory = parentObject;
			whhwtb.ModifiedBy = user;
			whhwtb.IsDeleted = false;
			_db.WageHourHistoryWageTypeBridges.Add((WageHourHistoryWageTypeBridge)whhwtb);
			return whhwtb;
		}
	}
}