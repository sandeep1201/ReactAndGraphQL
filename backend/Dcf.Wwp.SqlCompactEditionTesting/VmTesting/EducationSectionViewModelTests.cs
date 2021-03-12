using System;
using Xunit;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using Dcf.Wwp.Web.ViewModels;
using System.Linq;
using Dcf.Wwp.Web.Extensions;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
//using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
	public class EducationSectionViewModelTests : BaseUnitTest
	{
		[Fact]
		public void SimpleUpsertEducationSection_SaveEducationData_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var sd = new SchoolData();
			ia.SchoolData = sd;


			sd.SchoolLocation = "abc, abc, abc";
			sd.LastYearAttended = 2000;
			sd.GooglePlaceId = "asfa242sdfa";
			sd.Diploma = 2;




			// Act - Controller
			EducationSectionViewModel.UpsertData(repo, "123", ia.SchoolData, "test");

			// Assert.
			var dbData = (from es in Db.EducationSections where es.Id == 1 select es).SingleOrDefault(); ;
			Assert.NotNull(dbData.SchoolCollegeEstablishment.City.GooglePlaceId);
		}


	}
}
