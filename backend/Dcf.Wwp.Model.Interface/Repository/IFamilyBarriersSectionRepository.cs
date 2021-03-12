namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IFamilyBarriersSectionRepository
    {
        IFamilyBarriersSection NewFamilyBarriersSection(IParticipant participant, string user);
    }
}
