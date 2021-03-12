using Dcf.Wwp.Model.Interface.Cww;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWorkProgramSectionRepository
    {
        IWorkProgramSection NewWorkProgramSection(IParticipant parentParticipant, string user);

        IFsetStatus CwwFsetStatus(string pin);
    }
}
