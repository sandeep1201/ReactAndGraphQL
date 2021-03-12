using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Cww;
using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using Dcf.Wwp.Web.Controllers;
using Dcf.Wwp.Web.ViewModels;
using Dcf.Wwp.Web.ViewModels.Contracts;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
    public class HousingSectionViewModelTests : BaseUnitTest
    {
		[Fact]
		public void UpsertHousingSectionWithTrueBoolsBigIntsLongStrings_NoHousingSectionData_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var ia = new InformalAssessmentData();
			var hd = new HousingData();
			var hhList = new List<HousingHistoryContract>();
			hd.Histories = hhList;
			ia.HousingData = hd;


			var version = new byte[2] { 1, 2 };
			hd.RowVersion = version;

			string longString = null;
			int bigNumber = 999999;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";
			}


			// Housing Situation
			hd.HousingCurrentType = 1;
			hd.HousingCurrentDuration = bigNumber;

			hd.HousingCurrentRent = bigNumber;
			hd.HousingCurrentEviction = true;
			hd.HousingActionUtilityNotes = longString;


			var hh = new HousingHistoryContract();
			hh.HistoryType = 1;
			hh.Duration = bigNumber;
			hh.Eviction = true;
			hh.Rent = bigNumber;
			hh.Details = longString;
			hhList.Add(hh);

			hd.HousingCurrentUtility = true;
			hd.HousingActionUtilityNotes = longString;

			hd.HousingCurrentParticipant = true;
			hd.HousingActionParticipateNotes = longString;

			var actionNeeded = new bool?[] {true, true, true, true, true,true,true,false};
			//hd.HousingActions = actionNeeded;
			hd.CurrentHousingNotes = longString;

			// Act - Controller.
			HousingSectionViewModel.UpsertData(repo, "123", ia.HousingData, "test");


			// Assert.
			var dataDb = (from hs in Db.HousingSections where hs.Id == 1 select hs).SingleOrDefault();
			Assert.NotNull(dataDb);
			Assert.True(dataDb.CurrentHousingDetails == longString);

			// Cant find this on page
			//Assert.True(dataDb.HousingAssistanceDetails == longString);
			//Assert.True(dataDb.RentPaid == bigNumber);
			Assert.True(dataDb.CurrentHousingMonths == bigNumber);
			Assert.True(dataDb.HousingHistories.First().RentPaid == bigNumber);
			Assert.True(dataDb.HousingHistories.First().YearsLivingInMonths == bigNumber);

			Assert.True(dataDb.HousingHistories.First().HousingSituationId == 1);
			Assert.True(dataDb.HousingHistories.First().HasEvicted == true);
			Assert.True(dataDb.HousingHistories.First().Details == longString);

			// Actions Needed bools.
			//Assert.True(dataDb.HasRefertoCommunityProgramOrShelter == true);
			//Assert.True(dataDb.HasReferToSubsidizedHousing == true);
			//Assert.True(dataDb.HasEnergyAssistance == true);
			//Assert.True(dataDb.HasEmergencyAssistance == true);
			//Assert.True(dataDb.HasEmergencyPayment == true);
			//Assert.True(dataDb.HasJobAccessLoan == true);
			//Assert.True(dataDb.HasReferToOtherAgency == true);
			//Assert.True(dataDb.HasNoAssistanceNeeded == false);

		}

		[Fact]
		public void UpsertHousingSectionWithTrueBoolsBigIntsLongStrings_WithHousingSectionData_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithHousingSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var ia = new InformalAssessmentData();
			var hd = new HousingData();
			var hhList = new List<HousingHistoryContract>();
			hd.Histories = hhList;
			ia.HousingData = hd;

			var version = Db.HousingSections.SingleOrDefault(hs => hs.Id == 1).RowVersion;
			hd.RowVersion = version;

			string longString = null;
			int bigNumber = 999999;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";
			}


			// Housing Situation
			hd.HousingCurrentType = 1;
			hd.HousingCurrentDuration = bigNumber;

			hd.HousingCurrentRent = bigNumber;
			hd.HousingCurrentEviction = true;
			hd.HousingActionUtilityNotes = longString;


			var hh = new HousingHistoryContract();
			hh.HistoryType = 1;
			hh.Duration = bigNumber;
			hh.Eviction = true;
			hh.Rent = bigNumber;
			hh.Details = longString;
			hhList.Add(hh);

			hd.HousingCurrentUtility = true;
			hd.HousingActionUtilityNotes = longString;

			hd.HousingCurrentParticipant = true;
			hd.HousingActionParticipateNotes = longString;

			var actionNeeded = new bool?[] { true, true, true, true, true, true, true, false };
			//hd.HousingActions = actionNeeded;
			hd.CurrentHousingNotes = longString;

			// Act - Controller.
			HousingSectionViewModel.UpsertData(repo, "123", ia.HousingData, "test");

			// Assert.
			var dataDb = (from hs in Db.HousingSections where hs.Id == 1 select hs).SingleOrDefault();
			Assert.NotNull(dataDb);
			Assert.True(dataDb.CurrentHousingDetails == longString);

			// Cant find this on page
			//Assert.True(dataDb.HousingAssistanceDetails == longString);
			//Assert.True(dataDb.RentPaid == bigNumber);
			Assert.True(dataDb.CurrentHousingMonths == bigNumber);
			Assert.True(dataDb.HousingHistories.First().RentPaid == bigNumber);
			Assert.True(dataDb.HousingHistories.First().YearsLivingInMonths == bigNumber);

			Assert.True(dataDb.HousingHistories.First().HousingSituationId == 1);
			Assert.True(dataDb.HousingHistories.First().HasEvicted == true);
			Assert.True(dataDb.HousingHistories.First().Details == longString);

			// Actions Needed bools.
			//Assert.True(dataDb.HasRefertoCommunityProgramOrShelter == true);
			//Assert.True(dataDb.HasReferToSubsidizedHousing == true);
			//Assert.True(dataDb.HasEnergyAssistance == true);
			//Assert.True(dataDb.HasEmergencyAssistance == true);
			//Assert.True(dataDb.HasEmergencyPayment == true);
			//Assert.True(dataDb.HasJobAccessLoan == true);
			//Assert.True(dataDb.HasReferToOtherAgency == true);
			//Assert.True(dataDb.HasNoAssistanceNeeded == false);

		}

		[Fact]
		public void UpsertHeavyHousingSection_WithHousingSectionData_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithHousingSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var ia = new InformalAssessmentData();
			var hd = new HousingData();
			var hhList = new List<HousingHistoryContract>();
			hd.Histories = hhList;
			ia.HousingData = hd;

			var version = Db.HousingSections.SingleOrDefault(hs => hs.Id == 1).RowVersion;
			hd.RowVersion = version;

			string longString = null;
			int bigNumber = 999999;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";
			}


			// Housing Situation
			hd.HousingCurrentType = 1;
			hd.HousingCurrentDuration = bigNumber;

			hd.HousingCurrentRent = bigNumber;
			hd.HousingCurrentEviction = true;
			hd.HousingActionUtilityNotes = longString;

			int historiesCount = 1000;

			for (int i = 0; i < historiesCount; i++)
			{
				var hh = new HousingHistoryContract();
				hh.HistoryType = 1;
				hh.Duration = bigNumber;
				if(i % 2 == 0)
				{
					hh.Eviction = true;
				}
				else
				{
					hh.Eviction = false;
				}

				hh.Rent = bigNumber;
				hh.Details = longString;
				hhList.Add(hh);
			}


			hd.HousingCurrentUtility = true;
			hd.HousingActionUtilityNotes = longString;

			hd.HousingCurrentParticipant = true;
			hd.HousingActionParticipateNotes = longString;

			var actionNeeded = new bool?[] { true, true, true, true, true, true, true, false };
			//hd.HousingActions = actionNeeded;
			hd.CurrentHousingNotes = longString;

			// Act - Controller.
			HousingSectionViewModel.UpsertData(repo, "123", ia.HousingData, "test");


			// Assert.
			var dataDb = (from hs in Db.HousingSections where hs.Id == 1 select hs).SingleOrDefault();
			Assert.NotNull(dataDb);
			Assert.True(dataDb.CurrentHousingDetails == longString);

			// Cant find this on page
			//Assert.True(dataDb.HousingAssistanceDetails == longString);
			//Assert.True(dataDb.RentPaid == bigNumber);
			Assert.True(dataDb.CurrentHousingMonths == bigNumber);
			Assert.True(dataDb.HousingHistories.Count() == historiesCount);
			Assert.True(dataDb.HousingHistories.First().RentPaid == bigNumber);
			Assert.True(dataDb.HousingHistories.First().YearsLivingInMonths == bigNumber);

			Assert.True(dataDb.HousingHistories.First().HousingSituationId == 1);
			Assert.True(dataDb.HousingHistories.First().HasEvicted == true);
			Assert.True(dataDb.HousingHistories.First().Details == longString);

			// Actions Needed bools.
			//Assert.True(dataDb.HasRefertoCommunityProgramOrShelter == true);
			//Assert.True(dataDb.HasReferToSubsidizedHousing == true);
			//Assert.True(dataDb.HasEnergyAssistance == true);
			//Assert.True(dataDb.HasEmergencyAssistance == true);
			//Assert.True(dataDb.HasEmergencyPayment == true);
			//Assert.True(dataDb.HasJobAccessLoan == true);
			//Assert.True(dataDb.HasReferToOtherAgency == true);
			//Assert.True(dataDb.HasNoAssistanceNeeded == false);

		}

		[Fact]
	    public void UpsertHeavyHousingSection_WithHousingSectionDataAndAgencyNotes_SuccessfulSave()
	    {
			// Arrange informal assessment with no sections.
			VMTest.IaWithHousingSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding.
			var ia = new InformalAssessmentData();
			var hd = new HousingData();
			var hhList = new List<HousingHistoryContract>();
			hd.Histories = hhList;
			ia.HousingData = hd;

			var version = Db.HousingSections.SingleOrDefault(hs => hs.Id == 1).RowVersion;
			hd.RowVersion = version;

			string longString = null;
			int bigNumber = 999999;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";
			}


			// Housing Situation
			hd.HousingCurrentType = 1;
			hd.HousingCurrentDuration = bigNumber;

			hd.HousingCurrentRent = bigNumber;
			hd.HousingCurrentEviction = true;
			hd.HousingActionUtilityNotes = longString;

			int historiesCount = 1000;

			for (int i = 0; i < historiesCount; i++)
			{
				var hh = new HousingHistoryContract();
				hh.HistoryType = 1;
				hh.Duration = bigNumber;
				if (i % 2 == 0)
				{
					hh.Eviction = true;
				}
				else
				{
					hh.Eviction = false;
				}

				hh.Rent = bigNumber;
				hh.Details = longString;
				hhList.Add(hh);
			}


			hd.HousingCurrentUtility = true;
			hd.HousingActionUtilityNotes = longString;

			hd.HousingCurrentParticipant = true;
			hd.HousingActionParticipateNotes = longString;

			var actionNeeded = new bool?[] { true, true, true, true, true, true, true, false };
			//hd.HousingActions = actionNeeded;
			hd.CurrentHousingNotes = longString;
			hd.OtherAgencyDetails = longString;

			// Act - Controller.
			HousingSectionViewModel.UpsertData(repo, "123", ia.HousingData, "test");


			// Assert.
			var dataDb = (from hs in Db.HousingSections where hs.Id == 1 select hs).SingleOrDefault();
			Assert.NotNull(dataDb);
			Assert.True(dataDb.CurrentHousingDetails == longString);

			// Cant find this on page
			//Assert.True(dataDb.HousingAssistanceDetails == longString);
			//Assert.True(dataDb.RentPaid == bigNumber);
			Assert.True(dataDb.CurrentHousingMonths == bigNumber);
			Assert.True(dataDb.HousingHistories.Count() == historiesCount);
			Assert.True(dataDb.HousingHistories.First().RentPaid == bigNumber);
			Assert.True(dataDb.HousingHistories.First().YearsLivingInMonths == bigNumber);

			Assert.True(dataDb.HousingHistories.First().HousingSituationId == 1);
			Assert.True(dataDb.HousingHistories.First().HasEvicted == true);
			Assert.True(dataDb.HousingHistories.First().Details == longString);

			// Actions Needed bools.
			//Assert.True(dataDb.HasRefertoCommunityProgramOrShelter == true);
			//Assert.True(dataDb.HasReferToSubsidizedHousing == true);
			//Assert.True(dataDb.HasEnergyAssistance == true);
			//Assert.True(dataDb.HasEmergencyAssistance == true);
			//Assert.True(dataDb.HasEmergencyPayment == true);
			//Assert.True(dataDb.HasJobAccessLoan == true);
			//Assert.True(dataDb.HasReferToOtherAgency == true);
			//Assert.True(dataDb.HasNoAssistanceNeeded == false);

			//Assert.True(dataDb.ReferToOtherAgencyNotes == longString);
		}

	    [Fact]
	    public void GetHeavyWithTruesSectionData_WithHousingSectionData_SuccessfulGet()
	    {
			// Arrange
			var mockRepo = new Mock<IRepository>();

		    mockRepo.Setup(x => x.CwwCurrentAddressDetails("123", "300")).Returns(new CurrentAddressDetails()
		    {
				Address = "123 Main st.",
				State = "Wisconsin",
				City = "Madison",
				IsSubsidized = true,
				ShelterAmount = 32m, 
				Zip = "53711"
		    });


			VMTest.IaWithHeavyHousingSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			ILogger<ApiController> logger = null;


		    var ia = repo.InformalAssessmentById(1);


			// Act
			var response = HousingSectionViewModel.GetData(ia, mockRepo.Object, "123", 300m);


			// Assert
			string longString = null;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";

			}


			// CWW Data.
			Assert.True(response.CwwHousing[0].Address == "123 Main st.");
			Assert.True(response.CwwHousing[0].State == "Wisconsin");
			Assert.True(response.CwwHousing[0].City == "Madison");
			Assert.True(response.CwwHousing[0].Subsidized == "Yes");
			Assert.True(response.CwwHousing[0].RentObligation == "$32");
			Assert.True(response.CwwHousing[0].Zip == "53711");

			Assert.True(response.HousingCurrentType == 1);

			Assert.True(response.HousingCurrentDuration == 13);

			Assert.True(response.Histories.Count == 20);

		    foreach (var x in response.Histories)
		    {
				x.Details = longString;
				x.Eviction = true;
				x.HistoryType = 1;
				x.Duration = 999;
				x.Rent = 32;

			}

			Assert.True(response.HousingCurrentRent == 12);
			Assert.True(response.HousingCurrentEviction == true);
			Assert.True(response.CurrentHousingNotes == longString);

			Assert.True(response.HousingActionUtilityNotes == longString);
			Assert.True(response.HousingCurrentUtility == true);

			Assert.True(response.HousingCurrentParticipant == true);
			Assert.True(response.HousingActionParticipateNotes == longString);

			Assert.True(response.CurrentHousingNotes == longString);

			// Radio buttons
			var actions = new bool?[] {true,true,true,true,true,true,true, false};

		 //   for (int i = 0; i < 8; i++)
		 //   {
			//    if (response.HousingActions[i] != actions[i])
			//    {
			//	    Assert.False(true);
			//    }
			//}
	
			Assert.True(response.OtherAgencyDetails == longString);


		}

		[Fact]
		public void GetHeavyWithFalsesAndNullCWWSectionData_WithHousingSectionData_SuccessfulGet()
		{
			// Arrange
			var mockRepo = new Mock<IRepository>();

			mockRepo.Setup(x => x.CwwCurrentAddressDetails("123", "300")).Returns((CurrentAddressDetails) null);


			VMTest.IaHeavyWithFalsesHousingSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			ILogger<ApiController> logger = null;


			var ia = repo.InformalAssessmentById(1);


			// Act
			var response = HousingSectionViewModel.GetData(ia, mockRepo.Object, "123", 300m);


			// Assert
			string longString = null;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";

			}


			// CWW Data.
			Assert.True(response.CwwHousing == null);


			Assert.True(response.HousingCurrentType == 1);

			Assert.True(response.HousingCurrentDuration == 13);

			Assert.True(response.Histories.Count == 20);

			foreach (var x in response.Histories)
			{
				x.Details = longString;
				x.Eviction = true;
				x.HistoryType = 1;
				x.Duration = 999;
				x.Rent = 32;
			}

			Assert.True(response.HousingCurrentRent == 12);
			Assert.True(response.HousingCurrentEviction == false);
			Assert.True(response.CurrentHousingNotes == longString);

			Assert.True(response.HousingActionUtilityNotes == longString);
			Assert.True(response.HousingCurrentUtility == false);

			Assert.True(response.HousingCurrentParticipant == false);
			Assert.True(response.HousingActionParticipateNotes == longString);

			Assert.True(response.CurrentHousingNotes == longString);

			// Radio buttons
			var actions = new bool?[] { false, false, false, false, false, false, false, false };

			for (int i = 0; i < 8; i++)
			{
				//if (response.HousingActions[i] != actions[i])
				//{
				//	Assert.False(true);
				//}
			}

			Assert.True(response.OtherAgencyDetails == null);


		}
	}
}
