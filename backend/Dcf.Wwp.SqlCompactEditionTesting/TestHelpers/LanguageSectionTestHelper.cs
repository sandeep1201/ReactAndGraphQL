//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Dcf.Wwp.Data.Sql.Model;

//namespace Dcf.Wwp.SqlCompactEditionTesting.TestHelpers
//{
//	public class LanguageSectionTestHelper
//	{
//		//public static void SetupANewKnownLanguageWithLanguage(Wwp.Data.WwpContext Db)
//		//{
//		//	var la = new Data.Model.LanguageSection();
//		//	la.ModifiedDate = DateTime.Now;
//		//	Db.LanguageSections.Add(la);

//		//	var l = new Data.Model.Language();
//		//	l.Name = "English";
//		//	Db.Languages.Add(l);

//		//	//var ea = new Data.Model.EducationSection();
//		//	//ea.ModifiedDate = DateTime.Now;
//		//	//Db.EducationSections.Add(ea);

//		//	var kl = new Data.Model.KnownLanguage();
//		//	kl.LanguageSection = la;
//		//	kl.Language = l;
//		//	kl.ModifiedDate = DateTime.Now;
//		//	kl.LanguageId = 1;
//		//	Db.KnownLanguages.Add(kl);

//		//	Db.SaveChanges();
//		//}
//		public static void SetupANewInformalAssesmentAndLanguage(WwpEntities Db)
//		{
//			try
//			{
//                var p = new Participant();
//                Db.Participants.Add(p);
//                var ia = new InformalAssessment();
//                ia.Participant = p;
//               // ia.ParticipantId = 1;
//                Db.InformalAssessments.Add(ia);
//				var la = new Data.Sql.Model.LanguageSection();
//				la.ModifiedDate = DateTime.Now;
//				la.ModifiedBy = "unit test";
//				Db.LanguageSections.Add(la);

//				var l = new Data.Sql.Model.Language();
//				l.Name = "English";
//				l.ModifiedBy = "unit test";
//				Db.Languages.Add(l);

//				//var ea = new Data.Model.EducationSection();
//				//ea.ModifiedDate = DateTime.Now;
//				//Db.EducationSections.Add(ea);

//				var kl = new Data.Sql.Model.KnownLanguage();
//				kl.LanguageSection = la;
//				kl.Language = l;
//				kl.ModifiedDate = DateTime.Now;
//				kl.LanguageId = 1;
//				kl.ModifiedBy = "unit test";
//				Db.KnownLanguages.Add(kl);

//				Db.SaveChanges();
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine(ex.Message);
//				throw;
//			}
//		}


//	}
//}
