using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IFamilyBarriersDetailRepository
    {
        public IFamilyBarriersDetail NewFamilyBarriersDetail(IFamilyBarriersSection parentObject, string user)
        {
            IFamilyBarriersDetail fbd = new FamilyBarriersDetail();
            fbd.ModifiedBy = user;
            fbd.ModifiedDate = DateTime.Now;
            _db.FamilyBarriersDetails.Add((FamilyBarriersDetail)fbd);

            return fbd;
        }

        public IFamilyBarriersDetail FamilBarrierDetailsByDetailId(int? id)
        {
            return _db.FamilyBarriersDetails.SingleOrDefault(x => x.Id == id);
        }
    }
}
