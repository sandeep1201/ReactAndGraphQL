using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialReferralParentRepository
    {
        public INonCustodialReferralParent NewNonCustodialReferralParent(INonCustodialParentsReferralSection section, string user)
        {
            var ncrp = new NonCustodialReferralParent();
            ncrp.NonCustodialReferralParentsSection = section as NonCustodialParentsReferralSection;
            ncrp.ModifiedBy = user;
            ncrp.ModifiedDate = DateTime.Now;
            _db.NonCustodialReferralParents.Add(ncrp);

            return ncrp;
        }
    }
}