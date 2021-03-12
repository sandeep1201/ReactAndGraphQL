namespace Dcf.Wwp.Model.Interface.Repository
{
   public interface IPostSecondaryEducationRepository
    {
        IPostSecondaryEducationSection NewPostSecondaryEducationSection(IParticipant parentParticipant, string user);
    }
}
