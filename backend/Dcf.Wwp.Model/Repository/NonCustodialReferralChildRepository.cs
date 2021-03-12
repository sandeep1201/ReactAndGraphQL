using System;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{

    public partial class Repository : INonCustodialReferralChildRepository
    {
        public INonCustodialReferralChild NewNonCustodialReferralChild(INonCustodialReferralParent parent, string user)
        {
            var ncrc = new NonCustodialReferralChild();
            ncrc.NonCustodialReferralParent = parent as NonCustodialReferralParent;
            ncrc.ModifiedBy = user;
            ncrc.ModifiedDate = DateTime.Now;
            _db.NonCustodialReferralChilds.Add(ncrc);

            return ncrc;
        }

        public INonCustodialReferralChild GetNonCustodialReferralChildById(int id)
        {
            return _db.NonCustodialReferralChilds.SingleOrDefault(x => x.Id == id);
        }

        public INonCustodialReferralChild GetNonCustodialReferralChild(int id)
        {
            var r = _db.NonCustodialReferralChilds.FirstOrDefault(i => i.Id == id);

            return (r);
        }

        // written this way (with a lambda it's really the only method needed ;) )
        public INonCustodialReferralChild GetNonCustodialReferralChild(Expression<Func<INonCustodialReferralChild, bool>> clause)
        {
            var r = _db.NonCustodialReferralChilds.FirstOrDefault(clause);

            return (r);
        }

    }
}