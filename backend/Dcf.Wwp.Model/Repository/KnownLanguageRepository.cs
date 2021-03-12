using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
	public partial class Repository : IKnownLanguageRepository
	{
		public IKnownLanguage NewKnownLanguage(ILanguageSection parentSection)
		{
			IKnownLanguage kl = new KnownLanguage();
			kl.LanguageSection = parentSection;
			kl.IsDeleted = false;
			_db.KnownLanguages.Add((KnownLanguage)kl);

			return kl;
		}


		//public ICollection<IKnownLanguage> KnownLanguageByLanguageSection(ILanguageSection parentSection)
		//{
		//	var langs = _db.KnownLanguages.Where(x => x.LanguageSectionId == parentSection.Id);
		//	return langs;


		//}

		public void DeleteKnownLanguageById(int id)
		{
			var lang = _db.KnownLanguages.FirstOrDefault(x => x.Id == id);
			lang.IsDeleted = true;
		}
	}
}