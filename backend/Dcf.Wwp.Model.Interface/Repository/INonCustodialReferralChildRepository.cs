using System;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialReferralChildRepository
    {
        INonCustodialReferralChild NewNonCustodialReferralChild(INonCustodialReferralParent parent, string user);
        INonCustodialReferralChild GetNonCustodialReferralChildById(int id);

        INonCustodialReferralChild GetNonCustodialReferralChild(int id);
        INonCustodialReferralChild GetNonCustodialReferralChild(Expression<Func<INonCustodialReferralChild, bool>> clause);
    }
}