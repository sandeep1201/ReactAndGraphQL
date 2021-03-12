//using System;
//using Xunit;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
//using Dcf.Wwp.Web.ViewModels;
//using Dcf.Wwp.Web.ViewModels.InformalAssessment;
//using System.Linq;
//using Dcf.Wwp.Web.Extensions;


//namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
//{
//	public class PostSecondaryViewModelTests : BaseUnitTest
//	{
//		[Fact]
//		public void UpsertPostSecondaryEducationSection_NoCollegeAndLicenseCertificationData_SuccessfulSave()
//		{
//			// Arrange
//			SetupAPostSecondarySectionHelper.PostSecondaryEducationSectionData(Db);
//			IRepository repo = new Repository(Db);
//			BaseViewModel.InjectDependencies(repo);

//			// Act - Simuate API POST call with JSON data binding
//			var vm = new PostSecondarySectionViewModel();
//			var ia = new InformalAssessmentData();
//			ia.Pin = "123";

//			var psd = new PostSecondaryData();

//			psd.AttendedCollege = "No";
//			psd.LicenseCertified = "No";
//			var version = new byte[2] { 1, 2 };
//			psd.RowVersion = version;

//			ia.PostSecondaryData = psd;
//			vm.InformalAssessmentData = ia;

//			// Act - Controller
//			vm.Upsert("123", "test");

//			// Assert
//			var data1 = (from pse in Db.PostSecondaryEducationSections where pse.Id == 1 select pse).SingleOrDefault(); ;
//			Assert.NotNull(data1);
//		}

//		[Fact]
//		public void UpsertPostSecondaryEducationSection_WithCollegeAndNoLicenseCertificationData_SuccessfulSave()
//		{
//			// Arrange
//			SetupAPostSecondarySectionHelper.PostSecondaryEducationSectionData(Db);
//			IRepository repo = new Repository(Db);
//			BaseViewModel.InjectDependencies(repo);

//			// Act - Simuate API POST call with JSON data binding
//			var vm = new PostSecondarySectionViewModel();
//			var ia = new InformalAssessmentData();
//			ia.Pin = "123";

//			var psd = new PostSecondaryData();

//			psd.AttendedCollege = "Yes";
//			//College
//			psd.Location1 = "Madison,WI";
//			psd.Name1 = "University of Madison";
//			psd.HasGraduation1 = "Yes";
//			psd.LicenseAttainedYear1 = "2011";
//			psd.Semesters1 = 4;
//			psd.Details1 = "Graduated in Sciences";

//			//Degree
//			psd.Degree = "Yes";
//			psd.DegreeName1 = "Mastes in Information Systems";
//			psd.DegreeType1 = 1;
//			psd.DegreeLocation1 = "Madison,WI";
//			psd.DegreeAttainedYear1 = 2011;

//			//Liscence & Certification
//			psd.LicenseCertified = "No";

//			var version = new byte[2] { 1, 2 };
//			psd.RowVersion = version;

//			ia.PostSecondaryData = psd;
//			vm.InformalAssessmentData = ia;
//			psd.PostSecondaryNotes = "testing";

//			// Act - Controller
//			vm.Upsert("123", "test");

//			// Assert
//			var data1 = (from pse in Db.PostSecondaryEducationSections where pse.Id == 1 select pse).SingleOrDefault(); ;
//			Assert.NotNull(data1);
//		}

//		[Fact]
//		public void UpsertPostSecondaryEducationSection_WithCollegeAndNotExpiredandInProgressLicenseCertificationData_SuccessfulSave()
//		{
//			// Arrange
//			SetupAPostSecondarySectionHelper.PostSecondaryEducationSectionData(Db);
//			IRepository repo = new Repository(Db);
//			BaseViewModel.InjectDependencies(repo);

//			// Act - Simuate API POST call with JSON data binding
//			var vm = new PostSecondarySectionViewModel();
//			var ia = new InformalAssessmentData();
//			ia.Pin = "123";

//			var psd = new PostSecondaryData();

//			psd.AttendedCollege = "Yes";
//			//College
//			psd.Location1 = "Madison,WI";
//			psd.Name1 = "University of Madison";
//			psd.HasGraduation1 = "Yes";
//			psd.LicenseAttainedYear1 = "2011";
//			psd.Semesters1 = 4;
//			psd.Details1 = "Graduated in Sciences";

//			//Degree
//			psd.Degree = "Yes";
//			psd.DegreeName1 = "Mastes in Information Systems";
//			psd.DegreeType1 = 1;
//			psd.DegreeLocation1 = "Madison,WI";
//			psd.DegreeAttainedYear1 = 2011;

//			//Liscence & Certification
//			psd.CertificateType1 = "License";
//			psd.CertificationName1 = "SqlCertification";
//			psd.ValidLicense1 = "yes";
//			psd.LicenseIssuer1 = "Oracle";
//			psd.InProgress = "Yes";
//			psd.DoesExpired1 = "No";

//			var version = new byte[2] { 1, 2 };
//			psd.RowVersion = version;

//			ia.PostSecondaryData = psd;
//			vm.InformalAssessmentData = ia;
//			psd.PostSecondaryNotes = "testing";

//			// Act - Controller
//			vm.Upsert("123", "test");

//			// Assert
//			var data1 = (from pse in Db.PostSecondaryEducationSections where pse.Id == 1 select pse).SingleOrDefault(); ;
//			Assert.NotNull(data1);
//		}


//	}
//}
