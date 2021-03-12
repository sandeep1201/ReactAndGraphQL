namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialParentReferralSectionRepository
    {
        INonCustodialParentsReferralSection NewNonCustodialParentsReferralSection(int participantId, string user);
    }
}