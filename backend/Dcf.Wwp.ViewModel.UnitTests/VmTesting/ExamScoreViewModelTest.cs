//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dcf.Wwp.Api.Library;
//using Dcf.Wwp.Api.Library.Contracts;
//using Dcf.Wwp.Api.Library.Extensions;
//using Dcf.Wwp.Api.Library.ViewModels.TestScoresApp;
//using Dcf.Wwp.Data.Sql.Model;
//using Dcf.Wwp.Model.Interface;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


//namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
//{

//    [TestClass]
//    public class ExamScoreViewModelTest : BaseUnitTest
//    {
//        private enum SubjectType
//        {
//            English = 1,
//            Language,
//            ReadingWriting,
//            Reading,
//            Writing,
//            SpeakingListening,
//            Speaking,
//            Listening,
//            LifeAndWork,
//            Mathematics,
//            SocialStudies,
//            Science,
//            Civics,
//            Health
//        }

//        private enum ExamType
//        {
//            Tabe = 1,
//            Casas,
//            Gain,
//            Mapt,
//            GedHsed,
//            TabeClassE,
//            BestLiteracyPlus,
//            CasasEll
//        }
//        #region //Get Data
//        [TestMethod]
//        public void GetExamScore_WithSingleExamScoreAndHavingMultipleSujects_SuccessfulGet()
//        {
//            // Arrange.
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           
//            var vm = new ExamScoresViewModel(repo);
//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            string id = "1";
//            //var mockRepo = new Mock<IRepository>();

//            vm.InitializeFromPin(pin);
//            // Act- Controller
//            var data = vm.GetData(pin, id);

//            //Assert

//            Assert.IsTrue(data.Count == 1);

//            foreach (var x in data)
//            {
//                var data1 = (from ext in Db.EducationExams where ext.Id == 1 select ext).SingleOrDefault();
//                Assert.IsTrue(x.DateTaken == data1.DateTaken.ToStringMonthDayYear());
//                Assert.IsTrue(x.ExamTypeId == 1);
//                foreach (var y in x.Subjects)
//                {
//                    Assert.IsTrue(y.SubjectTypeId == 5);
//                    Assert.IsTrue(y.ExamLevelType == 1);
//                    Assert.IsTrue(y.NRSTypeId == 3);
//                    // Assert.IsTrue(y.SPLTypeId == 4);
//                    // Assert.IsTrue(y.SubjectType == "abc2");
//                    Assert.IsTrue(y.ExamPassTypeId == 1);
//                }
//                Assert.IsTrue(x.Subjects.Count == 10);
//            }
//        }

//        [TestMethod]
//        public void GetExamScore_WithMultipleExamScoresAndHavingMultipleSujects_SuccessfulGet()
//        {
//            // Arrange.
//            VMTest.ParticipantWithMultipleExamScoresAndHavingMultipleSubjects(Db);
//            IRepository repo = new Repository(Db);
           
//            var vm = new ExamScoresViewModel(repo);
//            //var mockRepo = new Mock<IRepository>();

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            string id = "x";

//            vm.InitializeFromPin(pin);
//            // Act- Controller
//            var data = vm.GetData(pin, id);

//            //Assert

//            Assert.IsTrue(data.Count == 10);
//            foreach (var x in data)
//            {
//                //Assert.IsTrue(x.DateTaken.ToDateTimeMonthDayYear() == DateTime.Now);
//                Assert.IsTrue(x.ExamTypeId == 1);
//                foreach (var y in x.Subjects)
//                {
//                    Assert.IsTrue(y.SubjectTypeId == 5);
//                    Assert.IsTrue(y.ExamLevelType == 1);
//                    Assert.IsTrue(y.NRSTypeId == 3);
//                    //Assert.IsTrue(y.SPLTypeId == 4);
//                    Assert.IsTrue(y.ExamPassTypeId == 1);
//                    //Assert.IsTrue(y.SubjectType == "GED");
//                }
//                Assert.IsTrue(x.Subjects.Count == 10);
//            }
//        }

//        [TestMethod]
//        public void GetExamScore_WitNoExamScoreAndIdInputZero_NoData()
//        {
//            // Arrange.
//            VMTest.ParticipantWithNoExamScores(Db);
//            IRepository repo = new Repository(Db);
           
//            var vm = new ExamScoresViewModel(repo);
//            //var mockRepo = new Mock<IRepository>();

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            string id = "0";

//            vm.InitializeFromPin(pin);
//            // Act- Controller
//            var data = vm.GetData(pin, id);

//            //Assert

//            Assert.IsTrue(data.Count == 0);

//        }

//        [TestMethod]
//        public void GetExamScore_WitNoExamScoreaAndIdInputOne_NoData()
//        {
//            // Arrange.
//            VMTest.ParticipantWithNoExamScores(Db);
//            IRepository repo = new Repository(Db);
           
//            var vm = new ExamScoresViewModel(repo);
//            //var mockRepo = new Mock<IRepository>();

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            string id = "1";

//            vm.InitializeFromPin(pin);
//            // Act- Controller
//            var data = vm.GetData(pin, id);

//            //Assert

//            Assert.IsTrue(data.Count == 0);

//        }

//        [TestMethod]
//        public void GetExamScores_InvalidPin_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "Pin not valid.";

//            // Arrange informal assessment with no sections.
//            VMTest.ParticipantWithNoExamScores(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulating Intialization of pin
//            string pin = "1234";
//            string id = "1";
//            try
//            {

//                var vm = new ExamScoresViewModel(repo);
//                // vm.InitializeFromPin(pin);
//                var data = vm.GetData(pin, id);
//            }
//            catch (InvalidOperationException ex)
//            {
//                expectedException = ex;
//            }

//            // Assert
//            Assert.IsNotNull(expectedException);
//            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
//        }
//        #endregion


//        [TestMethod]
//        public void UpdateExistingExamScoreWithSingleSubject_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.Tabe;//ExamType Mapt;
//            es.Id = 1;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            var esd = new SubjectContract();
//            esd.ExamLevelType = 1;
//            esd.Id = 1;
//            esd.ExamPassTypeId = 6;
//            esd.SubjectTypeId = (int)SubjectType.Reading;
//            esd.Score = 45;
//            esd.SPLTypeId = 4;
//            esd.NRSTypeId = 3;

//            es.Subjects = new List<SubjectContract>();
//            var subjects = es.Subjects;
//            subjects.Add(esd);
//            es.Subjects = subjects;

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);

//        }
//        [TestMethod]
//        public void UpdateExistingExamScoreWithNewSubject_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.Tabe;//ExamType Mapt;
//            es.Id = 1;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            var esd = new SubjectContract();
//            esd.ExamLevelType = 1;
//            esd.Id = 0;
//            esd.ExamPassTypeId = 6;
//            esd.SubjectTypeId = (int)SubjectType.Reading;
//            esd.Score = 45;
//            esd.SPLTypeId = 4;
//            esd.NRSTypeId = 3;

//            es.Subjects = new List<SubjectContract>();
//            var subjects = es.Subjects;
//            subjects.Add(esd);
//            es.Subjects = subjects;

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 11);

//        }

//        [TestMethod]
//        public void UpsertNewMaptExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.Mapt;//ExamType Mapt
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 9; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 0;
//                esd1.ExamPassTypeId = j;
//                esd1.SubjectTypeId = (int)SubjectType.Mathematics;
//                esd1.Score = 45;
//                esd1.SPLTypeId = j;
//                esd1.NRSTypeId = 1;
//                es.Subjects.Add(esd1);
//            }

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 10);
//        }

//        [TestMethod]
//        public void UpsertNewBestLiteracyPlusExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.BestLiteracyPlus;//ExamType BestLiteracyPlus
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 9; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 0;
//                esd1.ExamPassTypeId = j;
//                esd1.SubjectTypeId = (int)SubjectType.ReadingWriting;
//                esd1.Score = 45;
//                esd1.SPLTypeId = 1;
//                esd1.NRSTypeId = 1;
//                es.Subjects.Add(esd1);
//            }

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 10);
//        }

//        [TestMethod]
//        public void UpsertNewCasasExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.Casas;//ExamType Casas
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 9; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 0;
//                esd1.ExamPassTypeId = j;
//                esd1.SubjectTypeId = (int)SubjectType.Reading;
//                esd1.Score = 45;
//                esd1.SPLTypeId = j;
//                esd1.NRSTypeId = 1;
//                es.Subjects.Add(esd1);
//            }

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 10);
//        }

//        [TestMethod]
//        public void

//            UpsertNewGainExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.Gain;//ExamType Gain
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();

//            var esd1 = new SubjectContract();
//            esd1.ExamLevelType = 1;
//            esd1.Id = 0;
//            esd1.ExamPassTypeId = 1;
//            esd1.SubjectTypeId = (int)SubjectType.English;
//            esd1.Score = 45;
//            esd1.SPLTypeId = 1;
//            esd1.NRSTypeId = 1;
//            es.Subjects.Add(esd1);
//            var esd2 = new SubjectContract();
//            esd2.ExamLevelType = 1;
//            esd2.Id = 0;
//            esd2.ExamPassTypeId = 1;
//            esd2.SubjectTypeId = (int)SubjectType.Mathematics;
//            esd2.Score = 45;
//            esd2.SPLTypeId = 1;
//            esd2.NRSTypeId = 1;
//            es.Subjects.Add(esd2);

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 2 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 2);
//        }

//        [TestMethod]
//        public void UpsertNewGedHsedExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.GedHsed;//ExamType GedHsed
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 9; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 0;
//                esd1.ExamPassTypeId = 1;
//                esd1.SubjectTypeId = (int)SubjectType.SocialStudies;
//                esd1.Score = 45;
//                esd1.SPLTypeId = 1;
//                esd1.NRSTypeId = 1;
//                es.Subjects.Add(esd1);
//            }

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 10);
//        }

//        [TestMethod]
//        public void UpsertNewTabeClassEExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           
//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.TabeClassE;//ExamType TabeClassE
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 9; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 0;
//                esd1.ExamPassTypeId = j;
//                esd1.SubjectTypeId = (int)SubjectType.SpeakingListening;
//                esd1.Score = 45;
//                esd1.SPLTypeId = 1;
//                esd1.NRSTypeId = 1;
//                es.Subjects.Add(esd1);
//            }

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 10);
//        }

//        [TestMethod]
//        public void UpsertNewCasasEllExamScoreWithMultipleSubjects_WithParticipantHavingExamSectionHavingMultipleSubjects_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = (int)ExamType.CasasEll;//ExamType CasasEll
//            es.Id = 0;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 9; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 0;
//                esd1.ExamPassTypeId = j;
//                esd1.SubjectTypeId = (int)SubjectType.Listening;
//                esd1.Score = 45;
//                esd1.SPLTypeId = 1;
//                esd1.NRSTypeId = 1;
//                es.Subjects.Add(esd1);
//            }

//            // Act - Controller
//            var vm = new ExamScoresViewModel(repo);
//            vm.InitializeFromPin("123");
//            var data = vm.PostData(es, "Ahsan");
//            // Assert
//            var data2 = (from eds in Db.EducationExams where eds.Id == 1 select eds).SingleOrDefault();
//            Assert.IsNotNull(data2);
//            Assert.IsTrue(data2.ExamResults.Count == 10);
//        }

//        [TestMethod]
//        public void UpdateParticipantExamScores_InvalidPin_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "Pin not valid.";

//            // Arrange informal assessment with no sections.
//            VMTest.ParticipantWithMultipleExamScoresAndHavingMultipleSubjects(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simulate API POST call with JSON data binding
//            var es = new ExamScoreContract();
//            es.DateTaken = "05/19/1986";
//            es.ExamTypeId = 1;
//            es.Id = 1;
//            var version = new byte[2] { 1, 2 };
//            es.RowVersion = version;
//            es.Subjects = new List<SubjectContract>();
//            for (var j = 0; j <= 10; j++)
//            {
//                var esd1 = new SubjectContract();
//                esd1.ExamLevelType = 1;
//                esd1.Id = 1;
//                esd1.ExamPassTypeId = j;
//                esd1.SubjectTypeId = j;
//                esd1.Score = 45;
//                esd1.SPLTypeId = 4;
//                esd1.NRSTypeId = 3;
//                es.Subjects.Add(esd1);
//            }

//            var vm = new ExamScoresViewModel(repo);

//            // Act- Controller

//            try
//            {
//                vm.InitializeFromPin("1234");
//                vm.PostData(es, "Ahsan");
//            }
//            catch (InvalidOperationException ex)
//            {
//                expectedException = ex;
//            }

//            // Assert
//            Assert.IsNotNull(expectedException);
//            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
//        }
//        [TestMethod]
//        public void UpsertParticipantWithExamScore_NullyfyingExamScoreData_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "Exam Score data is missing.";

//            // Arrange informal assessment with no sections.
//            VMTest.ParticipantWithSingleExamScoreAndHavingMultipleSujects(Db);

//            IRepository repo = new Repository(Db);
           

//            // Act - Simuate API POST call with JSON data binding.
//            var model = new ExamScoreContract();
//            model = null;

//            // Act- Controller
//            try
//            {
//                model = null;
//                var vm = new ExamScoresViewModel(repo);
//                vm.InitializeFromPin("123");
//                var data = vm.PostData(model, "Ahsan");
//            }
//            catch (InvalidOperationException ex)
//            {
//                expectedException = ex;
//            }
//            // Assert
//            Assert.IsNotNull(expectedException);
//            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
//        }


//    }
//}

