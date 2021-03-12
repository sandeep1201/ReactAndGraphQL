namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEligibilityRepository
    {
        IEligibilityByFPL EligibilityByFPL(int householdSize);
    }
}
