namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialReferralParentRepository
    {
        INonCustodialReferralParent NewNonCustodialReferralParent(INonCustodialParentsReferralSection section, string user);
    }
}