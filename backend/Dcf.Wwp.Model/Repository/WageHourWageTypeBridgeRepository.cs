using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWageHourWageTypeBridgeRepository
    {
		public IWageHourWageTypeBridge NewWageHourWageTypeBridge(IWageHour parentObject, string user)
		{
			IWageHourWageTypeBridge wb = new WageHourWageTypeBridge();
			wb.WageHour = parentObject;
			//liab.ModifiedBy = user;
			wb.IsDeleted = false;
			_db.WageHourWageTypeBridges.Add((WageHourWageTypeBridge)wb);
			return wb;
		}
	}
}