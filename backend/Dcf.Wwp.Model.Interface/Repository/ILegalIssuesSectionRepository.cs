namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ILegalIssuesSectionRepository
    {
        ILegalIssuesSection NewLegalIssuesSection(IParticipant parentParticipant, string user);
    }
}
