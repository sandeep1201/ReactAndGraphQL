//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Dcf.Wwp.Api.Library;
//using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
//using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
//using Dcf.Wwp.ViewModel;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
//{
//    [TestClass]
//    public class MilitarySectionViewModelTests : BaseUnitTest
//    {
//        [TestMethod]
//        public void UpsertMilitaryTrainingSection_NoMilitiaryTrainingSectionData_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
//            //BaseViewModel.InjectDependencies(repo);

//            // Act - Simuate API POST call with JSON data binding
//            var msd = new MilitarySectionContract();
//            var version = new byte[2] { 1, 2 };
//            msd.RowVersion = version;

//            msd.DoesHaveTraining = true;

//            msd.BranchId = 5;
//            msd.RankId = 9;
//            for (int i = 0; i < 999; i++)
//            {
//                msd.Rate += "x";
//            }

//            msd.Years = 32;
//            msd.DischargeTypeId = 2;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.SkillsAndTraining += "x";
//            }

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Notes += "x";
//            }
//            // Act - Controller
//            var vm = new MilitarySectionViewModel(repo);
//            vm.PostData("123", msd,"NG");


//            // Assert
//            var data1 = (from mts in Db.MilitaryTrainingSections where mts.Id == 1 select mts).SingleOrDefault();
//            Assert.IsNotNull(data1);
//            var test = data1.SkillsAndTraining.Length == 999;
//            Assert.IsNotNull(data1.Notes.Length != 999);
//            Assert.IsNotNull(data1.Rate.Length != 999);
//            Assert.IsNotNull(data1.SkillsAndTraining.Length != 999);
//        }

//        [TestMethod]
//        public void UpsertMilitaryTrainingSection_ExistingMilitiaryTrainingSectionData_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.IaWithMilitarySectionHavingMilitaryTrainingData(Db);
//            IRepository repo = new Repository(Db);
           

//            // Act - Simuate API POST call with JSON data binding
//            var msd = new MilitarySectionContract();
//            var version = new byte[2] { 1, 2 };
//            msd.RowVersion = version;
//            msd.DoesHaveTraining = true;
//            msd.BranchId = 5;
//            msd.RankId = 9;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Rate += "x";
//            }

//            msd.Years = 32;
//            msd.DischargeTypeId = 2;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.SkillsAndTraining += "x";
//            }

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Notes += "x";
//            }

//            // Act - Controller
//            var vm = new MilitarySectionViewModel(repo);
//            vm.PostData("123", msd,"NG");

//            // Assert
//            var data1 = (from mts in Db.MilitaryTrainingSections where mts.Id == 1 select mts).SingleOrDefault();
//            Assert.IsNotNull(data1);
//            Assert.IsNotNull(data1.SkillsAndTraining);
//            Assert.IsNotNull(data1.Notes);
//            Assert.IsNotNull(data1.Rate);
//            Assert.IsNotNull(data1.Notes.Length != 999);
//            Assert.IsNotNull(data1.Rate.Length != 999);
//            Assert.IsNotNull(data1.SkillsAndTraining.Length != 999);

//        }
//        [TestMethod]
//        public void UpsertMilitaryTrainingSection_WithMiltarySectionHavingNoTraining_SuccessfulSave()
//        {
//            // Arrange
//            VMTest.IaWithMilitarySectionHavingNoMilitaryTrainingData(Db);
//            IRepository repo = new Repository(Db);
           
//            // Act - Simulate API POST call with JSON data binding
//            var msd = new MilitarySectionContract();
//            var version = new byte[2] { 1, 2 };
//            msd.RowVersion = version;
//            msd.DoesHaveTraining = false;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Notes += "x";
//            }

//            // Act - Controller
//            var vm = new MilitarySectionViewModel(repo);
//            vm.PostData("123", msd,"NG3");

//            // Assert
//            var data1 = (from mts in Db.MilitaryTrainingSections where mts.Id == 1 select mts).SingleOrDefault();
//            Assert.IsNotNull(data1);
//            Assert.IsNull(data1.SkillsAndTraining);
//            Assert.IsNotNull(data1.Notes);
//            Assert.IsNull(data1.Rate);
//        }

//        [TestMethod]
//        public void UpsertMilitarySection_InvalidPin_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "PIN not valid.";

//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);

//            // Act - Simuate API POST call with JSON data binding
//            var msd = new MilitarySectionContract();
//            var version = new byte[2] { 1, 2 };
//            msd.RowVersion = version;

//            msd.DoesHaveTraining = true;

//            msd.BranchId = 5;
//            msd.RankId = 9;
//            for (int i = 0; i < 999; i++)
//            {
//                msd.Rate += "x";
//            }

//            msd.Years = 32;
//            msd.DischargeTypeId = 2;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.SkillsAndTraining += "x";
//            }

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Notes += "x";
//            }
//            var vm = new MilitarySectionViewModel(repo);

//            // Act- Controller

//            try
//            {

//                var data = vm.PostData("1234", msd,"NG");
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
//        public void UpsertMilitarySection_NullyfyingMilitaryData_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "Military Training data is missing.";

//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding
//            var msd = new MilitarySectionContract();

//            var vm = new MilitarySectionViewModel(repo);


//            // Act- Controller

//            try
//            {
//                msd = null;
//                var data = vm.PostData("123", msd,"NG");
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
//        public void Upsert_AssignRowVersion_SuccessfulRowVersionAssigned()
//        {
//            // Arrange.
//            VMTest.IaWithMilitarySectionHavingMilitaryTrainingData(Db);
//            IRepository repo = new Repository(Db);
            
//            // Act - Simuate API POST call with JSON data binding
//            var msd = new MilitarySectionContract();

//            msd.DoesHaveTraining = true;
//            msd.BranchId = 5;
//            msd.RankId = 9;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Rate += "x";
//            }

//            msd.Years = 32;
//            msd.DischargeTypeId = 2;

//            for (int i = 0; i < 999; i++)
//            {
//                msd.SkillsAndTraining += "x";
//            }

//            for (int i = 0; i < 999; i++)
//            {
//                msd.Notes += "x";
//            }

//            // This row version matches DB.
//            var version = Db.MilitaryTrainingSections.SingleOrDefault(ms => ms.Id == 1).RowVersion;
//            msd.RowVersion = version;

//            // Act - Controller
//            var vw = new MilitarySectionViewModel(repo);
//            var data = vw.PostData("123", msd,"NG");

//            // Assert
//            var data1 = (from mts in Db.MilitaryTrainingSections where mts.Id == 1 select mts).SingleOrDefault();
//            Assert.IsTrue(data1?.MilitaryBranchId == 5);
//        }

//        [TestMethod]
//        public void GetMilitarySection_WithMilitarySectionHavingTrainingData_SuccessfulGet()
//        {
//            // Arrange informal assessment with Military section.
//            VMTest.IaWithMilitarySectionHavingMilitaryTrainingData(Db);
//            IRepository repo = new Repository(Db);
//            var vw = new MilitarySectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            Assert.IsTrue(data.DoesHaveTraining == true);
//            Assert.IsTrue(data.BranchId == 1);
//            Assert.IsTrue(data.RankId == 1);
//            Assert.IsTrue(data.Rate == "50");
//            Assert.IsTrue(data.Years == 15);
//            Assert.IsTrue(data.DischargeTypeId == 1);
//            Assert.IsNotNull(data.SkillsAndTraining);
//            Assert.IsNotNull(data.Notes);
//        }
//        [TestMethod]
//        public void GetMilitarySection_WithMilitarySectionHavingNoTrainingData_SuccessfulGet()
//        {
//            // Arrange informal assessment with Military section.
//            VMTest.IaWithMilitarySectionHavingNoMilitaryTrainingData(Db);
//            IRepository repo = new Repository(Db);
//            var vw = new MilitarySectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            Assert.IsTrue(data.DoesHaveTraining == false);
//            Assert.IsNull(data.BranchId);
//            Assert.IsNull(data.RankId);
//            Assert.IsNull(data.Rate);
//            Assert.IsNull(data.Years);
//            Assert.IsNull(data.DischargeTypeId);
//            Assert.IsNull(data.SkillsAndTraining);
//            Assert.IsNotNull(data.Notes);
//        }

//        [TestMethod]
//        public void GetEmptyMilitarySection_WithoutMilitarySectionAssessement_SuccesfulGet()
//        {
//            // Arrange informal assessment with Military section.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
//            var vw = new MilitarySectionViewModel(repo);
//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            Assert.IsNull(data.DoesHaveTraining);
//            Assert.IsNull(data.BranchId);
//            Assert.IsNull(data.RankId);
//            Assert.IsNull(data.Rate);
//            Assert.IsNull(data.Years);
//            Assert.IsNull(data.DischargeTypeId);
//            Assert.IsNull(data.SkillsAndTraining);
//            Assert.IsNull(data.Notes);
//        }


//    }
//}
