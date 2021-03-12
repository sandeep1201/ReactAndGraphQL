namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ILanguageSectionRepository
    {
        ILanguageSection NewLanguageSection(IParticipant parentParticipant, string user);
    }
}
