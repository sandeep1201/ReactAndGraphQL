//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Dcf.Wwp.Api.Library;
//using Dcf.Wwp.Api.Library.Contracts;
//using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
//using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
//using Dcf.Wwp.Data.Sql.Model;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


//namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
//{
//    [TestClass]
//    public class WorkHistorySectionViewModelTest : BaseUnitTest
//    {
//        #region // UpsertTests
//        [TestMethod]
//        public void UpsertWorkHistorySectionWithFullTimeEmplyomentStatus_NoWorkHistorySection_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 1;

//            ////Act-Controller
//            var vm = new WorkHistorySectionViewModel(repo);
//            vm.PostData("123", whd, "NG");

//            ////Assert
//            var data = (from whds in Db.WorkHistorySections where whds.Id == 1 select whds).SingleOrDefault();
//            Assert.IsNotNull(data);
//            Assert.IsTrue(data.EmploymentStatusTypeId == 1);
//        }

//        [TestMethod]
//        public void UpsertWorkHistorySectionWithPartTimeEmplyomentStatus_NoWorkHistorySection_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 2;
//            var actionNeededTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//            var preventionFactor = new ActionNeededTypeContract();
//            preventionFactor.ActionNeededTypes = actionNeededTypes;
//            whd.PreventionFactors = preventionFactor;
//            whd.PreventionFactors.AssistDetails = "Test";
//            ////Act-Controller
//            var vm = new WorkHistorySectionViewModel(repo);
//            vm.PostData("123", whd, "NG");

//            ////Assert
//            var data = (from whds in Db.WorkHistorySections where whds.Id == 1 select whds).SingleOrDefault();
//            Assert.IsNotNull(data);
//            Assert.IsTrue(data.EmploymentStatusTypeId == 2);
//            Assert.IsTrue(data.EmploymentWorkHistoryBridges.Count() == 10);
//            Assert.IsTrue(data.Details == "Test");
//        }

//        [TestMethod]
//        public void UpsertWorkHistorySectionWithUnEmploymentStatus_NoWorkHistorySection_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 3;
//            var actionNeededTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//            var preventionFactor = new ActionNeededTypeContract();
//            preventionFactor.ActionNeededTypes = actionNeededTypes;
//            whd.PreventionFactors = preventionFactor;
//            whd.PreventionFactors.AssistDetails = "Test";
//            ////Act-Controller
//            var vm = new WorkHistorySectionViewModel(repo);
//            vm.PostData("123", whd, "NG");

//            ////Assert
//            var data = (from whds in Db.WorkHistorySections where whds.Id == 1 select whds).SingleOrDefault();
//            Assert.IsNotNull(data);
//            Assert.IsTrue(data.EmploymentStatusTypeId == 3);
//            Assert.IsTrue(data.EmploymentWorkHistoryBridges.Count() == 10);
//            Assert.IsTrue(data.Details == "Test");
//        }


//        [TestMethod]
//        public void UpsertWorkHistorySectionWithFullTimeEmplyomentStatus_WithWorkHistorySection_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithWorkhistorySection(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 1;

//            ////Act-Controller
//            var vm = new WorkHistorySectionViewModel(repo);
//            vm.PostData("123", whd, "NG");

//            ////Assert
//            var data = (from whds in Db.WorkHistorySections where whds.Id == 1 select whds).SingleOrDefault();
//            Assert.IsNotNull(data);
//            Assert.IsTrue(data.EmploymentStatusTypeId == 1);
//        }

//        [TestMethod]
//        public void UpsertWorkHistorySectionWithPartTimeEmplyomentStatus_WithWorkHistorySection_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithWorkhistorySection(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 2;
//            var actionNeededTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//            var preventionFactor = new ActionNeededTypeContract();
//            preventionFactor.ActionNeededTypes = actionNeededTypes;
//            whd.PreventionFactors = preventionFactor;
//            whd.PreventionFactors.AssistDetails = "Test";
//            ////Act-Controller
//            var vm = new WorkHistorySectionViewModel(repo);
//            vm.PostData("123", whd, "NG");

//            ////Assert
//            var data = (from whds in Db.WorkHistorySections where whds.Id == 1 select whds).SingleOrDefault();
//            Assert.IsNotNull(data);
//            Assert.IsTrue(data.EmploymentStatusTypeId == 2);
//            Assert.IsTrue(data.EmploymentWorkHistoryBridges.Count() == 29);
//            Assert.IsTrue(data.Details == "Test");
//        }

//        [TestMethod]
//        public void UpsertWorkHistorySectionWithUnEmploymentStatus_WithWorkHistorySection_SuccessfulSave()
//        {
//            // Arrange informal assessment with no sections.
//            VMTest.IaWithWorkhistorySection(Db);
//            IRepository repo = new Repository(Db);
            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 3;
//            var actionNeededTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//            var preventionFactor = new ActionNeededTypeContract();
//            preventionFactor.ActionNeededTypes = actionNeededTypes;
//            whd.PreventionFactors = preventionFactor;
//            whd.PreventionFactors.AssistDetails = "Test";
//            ////Act-Controller
//            var vm = new WorkHistorySectionViewModel(repo);
//            vm.PostData("123", whd, "NG");

//            ////Assert
//            var data = (from whds in Db.WorkHistorySections where whds.Id == 1 select whds).SingleOrDefault();
//            Assert.IsNotNull(data);
//            Assert.IsTrue(data.EmploymentStatusTypeId == 3);
//            Assert.IsTrue(data.EmploymentWorkHistoryBridges.Count() == 29);
//            Assert.IsTrue(data.Details == "Test");
//        }

//        [TestMethod]
//        public void UpsertWorkHistorySection_InvalidPin_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "PIN not valid.";

//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);           

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();
//            var version = new byte[2] { 1, 2 };
//            whd.RowVersion = version;
//            whd.EmploymentStatusTypeId = 3;
//            var actionNeededTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//            var preventionFactor = new ActionNeededTypeContract();
//            preventionFactor.ActionNeededTypes = actionNeededTypes;
//            whd.PreventionFactors = preventionFactor;
//            whd.PreventionFactors.AssistDetails = "Test";
//            try
//            {
//                var vm = new WorkHistorySectionViewModel(repo);
//                var data = vm.PostData("1234", whd, "user");
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
//        public void UpsertWorkHistorySectio_NullyfyingWorkHistoryData_ThrowsInvalidOperationException()
//        {
//            // Expectations
//            InvalidOperationException expectedException = null;
//            string expectedExceptionMessage = "Work History data is missing.";

//            // Arrange informal assessment with no sections.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);            

//            // Act - Simuate API POST call with JSON data binding            
//            var whd = new WorkHistorySectionContract();

//            try
//            {
//                whd = null;
//                var vm = new WorkHistorySectionViewModel(repo);
//                var data = vm.PostData("123", whd, "NG");
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
//        #region // GetTests for workHistoryView Model
//        [TestMethod]
//        public void GetWorkHistorySection_WithWorkHistorySectionWithFullTimeEmploymentStatus_SuccessfulGet()
//        {
//            // Arrange informal assessment with Military section.
//            VMTest.IaWithWorkhistorySectionWithFulltimeEmploymentStatus(Db);
//            IRepository repo = new Repository(Db);
            
//            var vw = new WorkHistorySectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();
            
//            //Assert
//            Assert.IsTrue(data.EmploymentStatusTypeName == "Full-Time");
//        }
//        #endregion
//        [TestMethod]
//        public void GetWorkHistorySection_WithWorkHistorySectionWithPartTimeEmploymentStatus_SuccessfulGet()
//        {
//            // Arrange informal assessment with Military section.
//            VMTest.IaWithWorkhistorySection(Db);
//            IRepository repo = new Repository(Db);
            
//            var vw = new WorkHistorySectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            Assert.IsTrue(data.EmploymentStatusTypeName == "Part-Time");
//            Assert.IsTrue(data.PreventionFactors.ActionNeededTypes.Count == 10);
//            foreach (var x in data.PreventionFactors.ActionNeededTypes)
//            {
//                Assert.IsTrue(x == 1);
//            }
//        }

//        [TestMethod]
//        public void GetWorkHistorySection_WithWorkHistorySectionWithUnEmploymentStatus_SuccessfulGet()
//        {
//            // Arrange informal assessment with Military section.
//            VMTest.IaWithWorkhistorySection_WithUnemploymentStatus(Db);
//            IRepository repo = new Repository(Db);            
//            var vw = new WorkHistorySectionViewModel(repo);

//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert            
//            Assert.IsTrue(data.EmploymentStatusTypeName == "Unemployed");
//            Assert.IsTrue(data.PreventionFactors.ActionNeededTypes.Count == 10);
//            foreach (var x in data.PreventionFactors.ActionNeededTypes)
//            {
//                Assert.IsTrue(x == 1);
//            }
//            Assert.IsTrue(data.HasVolunteered == true);
//        }

//        [TestMethod]
//        public void GetEmptyWorkHistorySectionSection_WithoutWorkHistoryAssessment_SuccesfulGet()
//        {
//            // Arrange informal assessment with workHistorySection.
//            VMTest.IaWithNoSections(Db);
//            IRepository repo = new Repository(Db);
            
//            var vw = new WorkHistorySectionViewModel(repo);
//            // Act - Simulating Intialization of pin
//            string pin = "123";
//            vw.InitializeFromPin(pin);

//            // Act- Controller
//            var data = vw.GetData();

//            //Assert
//            Assert.IsNull(data.HasVolunteered);
//            Assert.IsNull(data.EmploymentStatusTypeId);
//        }

//    }
//}
