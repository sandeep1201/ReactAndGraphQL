namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEducationSectionRepository
    {
        IEducationSection NewEducationSection(IParticipant parentParticipant, string user);

        bool HasEducationSectionChanged(IEducationSection educationSection);
    }
}
