using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using Dcf.Wwp.Web.ViewModels;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Web.ViewModels.Contracts;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
	public class WorkProgramsViewModelTests : BaseUnitTest
	{
		[Fact]
		public void UpsertWorkPorgramSection_WithData_SuccessfulSave()
		{

			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var wpd = new WorkProgramData();
			var workPrograms = new List<WorkProgramContract>();
			wpd.WorkPrograms = workPrograms;
			ia.WorkProgramData = wpd;

			var version = new byte[2] { 1, 2 };
			wpd.RowVersion = version;


			const int workProgramCount = 15;

			wpd.IsInOtherPrograms = true;
			for (int i = 0; i < workProgramCount; i++)
			{
			
				var workprogram = new WorkProgramContract();

				workprogram.Location = "Madison, WI, United States";

				if (i % 2 == 0)
				{
					workprogram.Details = "details";
				}
				else
				{
					workprogram.Details = "ALTdetails";
				}

				workPrograms.Add(workprogram);


			}

			// Act - Controller
			WorkProgramSectionViewModel.UpsertData(repo, "123", ia.WorkProgramData, "test");



			// Assert
			var data = (from wp in Db.WorkProgramSections where wp.Id == 1 select wp).SingleOrDefault();

			Assert.True(data.IsInOtherPrograms);
			Assert.True(data.InvolvedWorkPrograms.Count == workProgramCount);
			Assert.True(data.InvolvedWorkPrograms.ElementAt(1).City.Name == "Madison");
			Assert.True(data.InvolvedWorkPrograms.ElementAt(1).City.State.Code == "WI");
			Assert.True(data.InvolvedWorkPrograms.ElementAt(1).City.State.Country.Name == "United States");

		}

		[Fact]
		public void UpsertWorkPorgramSection_InvolvedProgramsData_SuccessfulSaveWithRestore()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithWorkProgramsSection(Db, true);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var wpd = new WorkProgramData();
			var workPrograms = new List<WorkProgramContract>();
			wpd.WorkPrograms = workPrograms;
			ia.WorkProgramData = wpd;

			var version = new byte[2] { 1, 2 };
			wpd.RowVersion = version;

			wpd.IsInOtherPrograms = true;
			var workprogram = new WorkProgramContract();

			workprogram.Details = "Test";

			workPrograms.Add(workprogram);

		

			IWorkProgramSection preSaveData = (from wp in Db.WorkProgramSections where wp.Id == 1 select wp).SingleOrDefault();


			Assert.True(preSaveData.AllInvolvedWorkPrograms.ElementAt(0).IsDeleted == true);

			// Act - Controller
			WorkProgramSectionViewModel.UpsertData(repo, "123", ia.WorkProgramData, "test");

			var data = (from wp in Db.WorkProgramSections where wp.Id == 1 select wp).SingleOrDefault();

			Assert.True(data.InvolvedWorkPrograms.Count == 1);
			Assert.True(data.InvolvedWorkPrograms.ElementAt(0).IsDeleted == false);
		}
	}

}





