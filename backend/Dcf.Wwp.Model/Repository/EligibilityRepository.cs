using System;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEligibilityRepository
    {
        #region Methods

        public IEligibilityByFPL EligibilityByFPL(int householdSize)
        {
            //Do not use CDO Date as the table always looks for a current date
            return _db.EligibilityByFPLs.SingleOrDefault(i => i.GroupSize == householdSize && !i.IsDeleted && ((i.EndDate == null || i.EndDate >= DateTime.Today)
                                                                                                               && i.EffectiveDate <= DateTime.Today));
        }

        #endregion
    }
}
