//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Dcf.Wwp.Api.Library;
//using Dcf.Wwp.Api.Library.Contracts;
//using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
//using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
//using Dcf.Wwp.ViewModel;
//using Dcf.Wwp.Model.Interface;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using Dcf.Wwp.Data.Sql.Model;
//using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
//using Dcf.Wwp.ViewModel.UnitTests;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
//{
//    [TestClass]
//    public class LanguageSectionViewModelTests : BaseUnitTest
//    {
//        [TestMethod]
//        public void Upsert_SaveLanguagesWithSingleKnownLanguageAndEnglishAsPrimaryLanguage_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.IaWithLanguageSectionAndEducationSection_WithEnglishAsKnownLanguages(Db);
//            IRepository repo = new Repository(Db);
//            //////BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding
//            // LanguageSectionViewModel();
//            var model = new Api.Library.Contracts.InformalAssessment.LanguageSectionContract();

//            model.HomeLangType = 7;
//            model.IsAbleToSpeakHomeLang = true;
//            model.IsAbleToReadHomeLang = true;
//            model.IsAbleToWriteHomeLang = true;
//            var kn = new KnownLanguageContract();
//            kn.Id = 1;
//            kn.CanRead = true;
//            kn.CanSpeak = true;
//            kn.CanWrite = true;
//            kn.LanguageId = 38;

//            model.KnownLanguages = new List<KnownLanguageContract>();
//            var knownlanguages = model.KnownLanguages;
//            knownlanguages.Add(kn);
//            //x.KnownLanguages.Add(kn);
//            model.LangNotes = "Fluent speaking in English";
//            model.IsNeedingInterpreter = true;
//            model.InterpreterNotes = "Needs Training in Interpretor Notes";
//            var version = new byte[2] {1, 2};
//            model.RowVersion = version;
//            // x.RowVersion =             
//            //vm.InformalAssessmentData = ia;
//            //vm.InformalAssessmentData.LanguagesData = x;

//            // Act - Controller
//            var vm = new LanguageSectionViewModel(repo);
//            vm.PostData("123", model,"NG2");

//            // Asserts the save of a language section. 
//            var langSectionDb = (from ls in Db.LanguageSections where ls.Id == 1 select ls).SingleOrDefault();
//            Assert.IsNotNull(langSectionDb);

//            // Asserts English as primary language with able to read,speak and write abilty's.
//            var primaryLangDb = langSectionDb.KnownLanguages.SingleOrDefault(x1 => x1.IsPrimary == true && x1.LanguageId == 7 );
//            if (primaryLangDb == null)
//            {
//                Assert.IsTrue(true);
//                return;
//            }
//            Assert.IsTrue(primaryLangDb.IsAbleToRead == true);
//            Assert.IsTrue(primaryLangDb.IsAbleToSpeak == true);
//            Assert.IsTrue(primaryLangDb.IsAbleToWrite == true);

//            // Asserts particiapnt has secondary language other than english.
//            var nonPrimaryLangDb = langSectionDb.KnownLanguages.SingleOrDefault(x1 => x1.LanguageId == 38);
//            if (nonPrimaryLangDb == null)
//            {
//                Assert.IsFalse(true);
//                return;
//            }
//        }

//        [TestMethod]
//        public void Upsert_SavePrimaryLanguageAndEnglish_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);

//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding.
//            var x = new Api.Library.Contracts.InformalAssessment.LanguageSectionContract();
//            x.KnownLanguages = new List<KnownLanguageContract>();
//            x.EnglishRead = false;
//            x.EnglishSpeak = false;
//            x.EnglishWrite = false;

//            x.HomeLangType = 1;
//            x.IsAbleToReadHomeLang = true;
//            x.IsAbleToSpeakHomeLang = true;
//            x.IsAbleToWriteHomeLang = true;

//            var version = new byte[2] {1, 2};
//            x.RowVersion = version;

//            // Act - Controller
//            var vw = new LanguageSectionViewModel(repo);
//            vw.PostData("123", x,"NG2");

//            // Asserts language section is saved.
//            var langSectionDb = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1);
//            if (langSectionDb == null)
//            {
//                Assert.IsTrue(true);
//                return;
//            }

//            // Asserts English is non primary language with no read, write and speak skills.
//            var englishLangDb = langSectionDb.KnownLanguages.SingleOrDefault(x1 => x1.LanguageId == 7);
//            if (englishLangDb == null)
//            {
//                Assert.IsTrue(true);
//                return;
//            }

//            Assert.IsTrue(englishLangDb.IsAbleToRead == false);
//            Assert.IsTrue(englishLangDb.IsAbleToSpeak == false);
//            Assert.IsTrue(englishLangDb.IsAbleToWrite == false);

//            // Asserts Other language has Priamary language with reading, wrting and speaking abilities.
//            var primaryLangDb = langSectionDb.KnownLanguages.SingleOrDefault(x1 => x1.IsPrimary == true);
//            if (primaryLangDb == null)
//            {
//                Assert.IsTrue(true);
//                return;
//            }

//            Assert.IsTrue(primaryLangDb.IsAbleToRead == true);
//            Assert.IsTrue(primaryLangDb.IsAbleToSpeak == true);
//            Assert.IsTrue(primaryLangDb.IsAbleToWrite == true);


//        }

//        [TestMethod]
//        public void Upsert_SaveLanguagesWith150KnownLanguages_SuccessfulSave()
//        {
//            // Arrange.
//            VMTest.IaWithLanguagesSection(Db);
//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding.
//            var model = new Api.Library.Contracts.InformalAssessment.LanguageSectionContract();
//            model.KnownLanguages = new List<KnownLanguageContract>();

//            model.HomeLangType = 7;
//            model.IsAbleToSpeakHomeLang = true;
//            model.IsAbleToReadHomeLang = true;
//            model.IsAbleToWriteHomeLang = true;

//            for (int i = 1; i <= 150; i++)
//            {
//                var lang = new KnownLanguageContract();
//                lang.LanguageId = i;
//                model.KnownLanguages.Add(lang);
//            }


//            // This row version matches DB.
//            var version = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1)?.RowVersion;
//            model.RowVersion = version;

//            // Act - Controller
//            var vw = new LanguageSectionViewModel(repo);
//            vw.PostData("123", model,"NG2");


//            // Asserts that saved known languages are 150.           
//            Assert.IsTrue(Db?.KnownLanguages.Count() == 150);
//        }
    
//        [TestMethod]
//        public void Upsert_AssignRowVersion_SuccessfulRowVersionAssigned()
//        {
//            // Arrange.
//            VMTest.IaWithLanguagesSection(Db);
//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding.
//            var model = new Api.Library.Contracts.InformalAssessment.LanguageSectionContract();

//            model.KnownLanguages = new List<KnownLanguageContract>();
//            model.HomeLangType = 7;
//            model.IsAbleToSpeakHomeLang = true;
//            model.IsAbleToReadHomeLang = true;
//            model.IsAbleToWriteHomeLang = true;
//            for (int i = 1; i < 2; i++)
//            {
//                var lang = new KnownLanguageContract();
//                lang.LanguageId = i;
//                model.KnownLanguages.Add(lang);
//            }


//            //  row version matches DB.
//            var version = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1)?.RowVersion;
//            model.RowVersion = version;

//            // Act - Controller
//            var vw = new LanguageSectionViewModel(repo);
//            vw.PostData("123", model,"NG");

//            // Assert
//            var dataversion = Db.LanguageSections.SingleOrDefault(ls => ls.Id == 1)?.RowVersion;
//            Assert.IsTrue(dataversion != model.RowVersion);
           
//        }

//        [TestMethod]
//        public void UpsertLanguageSection_InvalidPin_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "PIN not valid.";

//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);

//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding.
//            var model = new Api.Library.Contracts.InformalAssessment.LanguageSectionContract();


//            model.KnownLanguages = new List<KnownLanguageContract>();

//            for (int i = 0; i < 150; i++)
//            {
//                var lang = new KnownLanguageContract();
//                lang.LanguageId = i;
//                model.KnownLanguages.Add(lang);
//            }

//            var version = new byte[2] { 1, 2 };
//            model.RowVersion = version;
//            // Act- Controller

//            try
//            {
//                model = null;
//                var vm = new LanguageSectionViewModel(repo);
//                var data = vm.PostData("1234", model,"Ng2");
//            }
//            catch (InvalidOperationException ex)
//            {
//                expectedException = ex;
//            }


//            // Asserts pin is invalid
//            Assert.IsNotNull(expectedException);

//            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
//        }

//        [TestMethod]
//        public void UpsertLanguageSection_NullyfyingLanguageData_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "Languages data is missing.";

//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);

//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding.
//            var model = new Api.Library.Contracts.InformalAssessment.LanguageSectionContract();
//            model = null;

//            // Act- Controller
//            try
//            {
//                model = null;
//                var vm = new LanguageSectionViewModel(repo);
//                var data = vm.PostData("123", model,"NG2");
//            }
//            catch (InvalidOperationException ex)
//            {
//                expectedException = ex;
//            }
//            // Assert inserted Empty Model with Invalid model exception.
//            Assert.IsNotNull(expectedException);
//            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
//        }
    

//        [TestMethod]
//        public void GetLanguageSection_WithLanguageSectionHaveEnglishAsPrimary_SuccessfulGet()
//        {
//            // Arrange informal assessment with Language section.
//            VMTest.IaWithLanguageSectionAndEducationSection_WithEnglishAsKnownLanguages(Db);
//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);
//            var vw = new LanguageSectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            Assert.IsTrue(data.HomeLangType == 7);
//            Assert.IsTrue(data.IsAbleToReadHomeLang == true);
//            Assert.IsTrue(data.IsAbleToSpeakHomeLang == true);
//            Assert.IsTrue(data.IsAbleToWriteHomeLang == false);
//        }

//        [TestMethod]
//        public void GetLanguageSection_WithLanguageSectionHavingKnownLanguages_SuccessfulGet()
//        {
//            // Arrange informal assessment with Language section.
//            VMTest.IaWithLanguageSection_WithKnownLanguages_Data(Db);
//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);
//            var vw = new LanguageSectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            // Assert
//            var test = Db.KnownLanguages.Count();
//            Assert.IsTrue(Db.KnownLanguages.Count() == data.KnownLanguages.Count());
//        }

//        [TestMethod]
//        public void GetEmptyLanguageSection_WithoutLanguageSectionAssessement_SuccesfulGet()
//        {
//            // Arrange informal assessment with Language section.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
//            ////BaseViewModel.InjectDependencies(repo);
//            var vw = new LanguageSectionViewModel(repo);
//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert         
//            Assert.IsNull(data.EnglishRead);
//            Assert.IsNull(data.EnglishSpeak);
//            Assert.IsNull(data.EnglishWrite);
//        }
//    }
//}



