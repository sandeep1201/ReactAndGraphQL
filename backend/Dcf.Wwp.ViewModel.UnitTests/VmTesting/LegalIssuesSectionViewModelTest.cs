using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
{
    class LegalIssuesSectionViewModelTest: BaseUnitTest
    {
        private Mock<IRepository> mockRepository;
        private Mock<IAuthUser> mockAuthUser;
        private IRepository repo;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRepository>();
            mockAuthUser= new Mock<IAuthUser>();
        }

        [TestMethod]
        public void UpsertData_WithAllRepeatersSetToNo_SuccessfulSave()
        {
            var testClass = new LegalIssueSectionViewModel(mockRepository.Object, mockAuthUser.Object);

            var c = new LegalIssuesSectionContract();

            c.IsConvictedOfCrime = false;
            c.IsUnderCommunitySupervision = false;
            c.IsPending = false;
            c.HasFamilyLegalIssues = false;
            c.HasUpcomingCourtDates = false;

            //testClass.UpsertData("123", c, "user1" );

        }
    }
}
