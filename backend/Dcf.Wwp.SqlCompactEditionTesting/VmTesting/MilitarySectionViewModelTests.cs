using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using Dcf.Wwp.Web.ViewModels;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
	public class MilitarySectionViewModelTests : BaseUnitTest
	{
		[Fact]
		public void UpsertMilitaryTrainingSection_NoMilitiaryTrainingSectionData_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var msd = new MilitaryTrainingData();
			ia.MilitaryTrainingData = msd;


			var version = new byte[2] { 1, 2 };
			msd.RowVersion = version;

			msd.DoesHaveTraining = true;

			msd.MilitaryBranch = 5;
			msd.MilitaryRank = 9;
			for (int i = 0; i < 999; i++)
			{
				msd.MilitaryRate += "x";
			}

			msd.MilitaryYears = 32;
			msd.DischargeType = 2;

			for (int i = 0; i < 999; i++)
			{
				msd.SkillsAndTraining += "x";
			}

			for (int i = 0; i < 999; i++)
			{
				msd.MilitaryNotes += "x";
			}

			// Act - Controller
			MilitaryTrainingSectionViewModel.UpsertData(repo, "123", ia.MilitaryTrainingData, "test");


			// Assert
			var data1 = (from mts in Db.MilitaryTrainingSections where mts.Id == 1 select mts).SingleOrDefault(); 
			Assert.NotNull(data1);
			var test = data1.SkillsAndTraining.Length == 999;
			Assert.False(data1.Notes.Length != 999);
			Assert.False(data1.Rate.Length != 999);
			Assert.False(data1.SkillsAndTraining.Length != 999);
		}

		[Fact]
		public void UpsertMilitaryTrainingSection_ExistingMilitiaryTrainingSectionData_SuccessfulSave()
		{
			// Arrange
			VMTest.IaWithMilitarySection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var msd = new MilitaryTrainingData();
			ia.MilitaryTrainingData = msd;

	
			var version = new byte[2] { 1, 2 };
			msd.RowVersion = version;


			msd.DoesHaveTraining = true;

			msd.MilitaryBranch = 5;
			msd.MilitaryRank = 9;

			for (int i = 0; i < 999; i++)
			{
				msd.MilitaryRate += "x";
			}

			msd.MilitaryYears = 32;
			msd.DischargeType = 2;

			for (int i = 0; i < 999; i++)
			{
				msd.SkillsAndTraining += "x";
			}

			for (int i = 0; i < 999; i++)
			{
				msd.MilitaryNotes += "x";
			}

			// Act - Controller
			MilitaryTrainingSectionViewModel.UpsertData(repo, "123", ia.MilitaryTrainingData, "test");

			// Assert
			var data1 = (from mts in Db.MilitaryTrainingSections where mts.Id == 1 select mts).SingleOrDefault();
			Assert.NotNull(data1);
			Assert.NotNull(data1.SkillsAndTraining);
			Assert.NotNull(data1.Notes);
			Assert.NotNull(data1.Rate);
			Assert.False(data1.Notes.Length != 999);
			Assert.False(data1.Rate.Length != 999);
			Assert.False(data1.SkillsAndTraining.Length != 999);

		}
	}
}
