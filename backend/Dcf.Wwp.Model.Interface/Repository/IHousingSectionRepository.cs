namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IHousingSectionRepository
    {
        Cww.ICurrentAddressDetails CwwCurrentAddressDetails(string pin);
        IHousingSection NewHousingSection(IParticipant parentParticipant, string user);
    }
}