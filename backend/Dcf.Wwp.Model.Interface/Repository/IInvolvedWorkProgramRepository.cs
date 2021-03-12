namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IInvolvedWorkProgramRepository
    {
        IInvolvedWorkProgram NewInvolvedWorkProgram(IWorkProgramSection parentSection, string user);

        void DeleteWorkProgram(IInvolvedWorkProgram workProgram);
    }
}
