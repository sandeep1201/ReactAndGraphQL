using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ILanguageRepository
    {
        IEnumerable<ILanguage> Languages();
        IEnumerable<ILanguage> AllLanguages();
    }
}
