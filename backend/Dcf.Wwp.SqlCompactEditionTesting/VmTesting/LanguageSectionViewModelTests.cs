using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Dcf.Wwp.Web.ViewModels;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using Dcf.Wwp.Web.Controllers;
using Dcf.Wwp.Web.ViewModels.Contracts;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
using System.Web.Http.SelfHost;
using System.Web.Http;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{

	public class LanguageSectionViewModelTests : BaseUnitTest
	{
		[Fact]
		public void Upsert_SaveLanguagesWithSingleKnownLanguageAndEnglishAsPrimaryLanguage_SuccessfulSave()
		{
			// Arrange
			VMTest.LanguageAndEducationSection_withKnownLanguage_Data(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			// LanguageSectionViewModel();
			var x = new LanguagesData();

			// var ls = new LanguageSection();
			var ia = new InformalAssessmentData();

			x.HomeLangId = 40;
			x.IsAbleToSpeakHomeLang = true;
			x.IsAbleToReadHomeLang = true;
			x.IsAbleToWriteHomeLang = true;
			var kn = new KnownLanguageContract();
			kn.Id = 1;
			kn.CanRead = true;
			kn.CanSpeak = true;
			kn.CanWrite = true;
			kn.LanguageId = 38;

			x.KnownLanguages = new List<KnownLanguageContract>();
			var knownlanguages = x.KnownLanguages;
			knownlanguages.Add(kn);
			//x.KnownLanguages.Add(kn);
			x.LangNotes = "Fluent speaking in English";
			x.IsNeedingInterpreter = true;
			x.InterpreterNotes = "Needs Training in Interpretor Notes";
			var version = new byte[2] { 1, 2 };
			x.RowVersion = version;
			// x.RowVersion =             
			//vm.InformalAssessmentData = ia;
			//vm.InformalAssessmentData.LanguagesData = x;
			ia.LanguagesData = x;
			var model = ia.LanguagesData;

			// Act - Controller
			LanguageSectionViewModel.UpsertData(repo, "123", model, "test");

			// Assert
			var data2 = (from ls in Db.LanguageSections where ls.Id == 1 select ls).SingleOrDefault();
			Assert.NotNull(data2);

		}

		[Fact]
		public void Upsert_SavePrimaryLanguageAndEnglish_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);

			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var model = new InformalAssessmentData();
			var x = new LanguagesData();
			x.KnownLanguages = new List<KnownLanguageContract>();
			model.LanguagesData = x;

			x.EnglishRead = false;
			x.EnglishSpeak = false;
			x.EnglishWrite = false;

			x.HomeLang = 1;
			x.IsAbleToReadHomeLang = true;
			x.IsAbleToSpeakHomeLang = true;
			x.IsAbleToWriteHomeLang = true;

			var version = new byte[2] { 1, 2 };
			x.RowVersion = version;

			// Act - Controller
			LanguageSectionViewModel.UpsertData(repo, "123", model.LanguagesData, "test");

			// Asserts.
			var langSectionDb = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1);
			if(langSectionDb == null)
			{
				Assert.False(true);
				return;
			}


			var englishLangDb = langSectionDb.KnownLanguages.SingleOrDefault(x1 => x1.LanguageId == 40);
			if (englishLangDb == null)
			{
				Assert.False(true);
				return;
			}

			Assert.True(englishLangDb.IsAbleToRead == false);
			Assert.True(englishLangDb.IsAbleToSpeak == false);
			Assert.True(englishLangDb.IsAbleToWrite == false);


			var primaryLangDb = langSectionDb.KnownLanguages.SingleOrDefault(x1 => x1.IsPrimary == true);
			if (primaryLangDb == null)
			{
				Assert.False(true);
				return;
			}

			Assert.True(primaryLangDb.IsAbleToRead == true);
			Assert.True(primaryLangDb.IsAbleToSpeak == true);
			Assert.True(primaryLangDb.IsAbleToWrite == true);

			
		}

		[Fact]
		public void Upsert_SaveLanguagesWith150KnownLanguages_SuccessfulSave()
		{
			// Arrange.
			VMTest.IaWithLanguagesSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var model = new InformalAssessmentData();
			var x = new LanguagesData();
			model.LanguagesData = x;
			x.KnownLanguages = new List<KnownLanguageContract>();

			for (int i = 0; i < 150; i++)
			{
				var lang = new KnownLanguageContract();
				lang.LanguageId = i;
				x.KnownLanguages.Add(lang);
			}


			// This row version matches DB.
			var version = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1).RowVersion;
			x.RowVersion = version;

			// Act - Controller
			LanguageSectionViewModel.UpsertData(repo, "123", model.LanguagesData, "test");

			// Assert
			var test = Db.KnownLanguages.Count();
			Assert.True(Db.KnownLanguages.Count() == x.KnownLanguages.Count());
		}

		[Fact]
		public void Upsert_SaveLanguagesWith150KnownLanguagesWithNoLanguageSectionInDB_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);

			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var model = new InformalAssessmentData();
			var x = new LanguagesData();

			x.KnownLanguages = new List<KnownLanguageContract>();

			for (int i = 0; i < 150; i++)
			{
				var lang = new KnownLanguageContract();
				lang.LanguageId = i;
				x.KnownLanguages.Add(lang);
			}

			model.LanguagesData = x;

			var version = new byte[2] { 1, 2 };
			x.RowVersion = version;

			// Act - Controller
			LanguageSectionViewModel.UpsertData(repo, "123", model.LanguagesData, "test");

			Assert.True(Db.KnownLanguages.Count() == x.KnownLanguages.Count());

		}


		[Fact]
		public void Upsert_AssignRowVersion_SuccessfulRowVersionAssigned()
		{
			// Arrange.
			VMTest.IaWithLanguagesSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var model = new InformalAssessmentData();
			var x = new LanguagesData();
			model.LanguagesData = x;
			x.KnownLanguages = new List<KnownLanguageContract>();

			for (int i = 0; i < 2; i++)
			{
				var lang = new KnownLanguageContract();
				lang.LanguageId = i;
				x.KnownLanguages.Add(lang);
			}


			// This row version matches DB.
			var version = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1).RowVersion;
			x.RowVersion = version;

			// Act - Controller
			LanguageSectionViewModel.UpsertData(repo, "123", model.LanguagesData, "test");

			// Assert
			var test = Db.KnownLanguages.Count();
			Assert.True(Db.KnownLanguages.Count() == x.KnownLanguages.Count());
		}

	}
}
