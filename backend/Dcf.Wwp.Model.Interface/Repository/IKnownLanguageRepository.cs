using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
	public interface IKnownLanguageRepository
	{
		IKnownLanguage NewKnownLanguage(ILanguageSection parentSection);

		void DeleteKnownLanguageById(Int32 id);
	}
}