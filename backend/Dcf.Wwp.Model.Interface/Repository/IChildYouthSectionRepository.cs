namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IChildYouthSectionRepository
    {
        IChildYouthSection NewChildYouthSection(int participantId, string user);
    }
}