using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using Dcf.Wwp.Web.Extensions;
using Dcf.Wwp.Web.ViewModels;
using Dcf.Wwp.Web.ViewModels.Contracts;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
    public class LegalIssuesViewModelTests : BaseUnitTest
	{
	    [Fact]
	    public void UpsertLegalIssuesSection_NoLegalIssuesSection_SuccessfulSave()
	    {
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var lid = new LegalIssuesData();
			ia.LegalIssueData = lid;

			var version = new byte[2] { 1, 2 };
			lid.RowVersion = version;

		    lid.IsConvictedOfCrime = true;

		    string longString = null;
		    for (int i = 0; i < 1000; i++)
		    {
			    longString += "x";
		    }

			var crimes = new List<ConvictionContract>();
		    lid.Convictions = crimes;
		    for (int i = 0; i < 11; i++)
		    {
			    var x = new ConvictionContract();
			    x.Type = 1;
			    x.Date = "02/2015";
			    x.Details = longString;
				crimes.Add(x);
		    }

			// Are you currently under community supervision?
			lid.IsParoled = true;
		    lid.ParoledDetails = "CommunitySupervisonDetails";

		    lid.IsPending = true;

			var pendings = new List<PendingContract>();
		    lid.Pendings = pendings;
		    for (int i = 0; i < 10; i++)
		    {
			    var x = new PendingContract();
			    x.Type = 1;
			    x.Date = "03/2015";
			    x.Details = longString;
				pendings.Add(x);
		    }

			// Do you have any immediate family members with legal issues?
			lid.HasFamilyLegalIssues = true;
		    lid.FamilyLegalIssueNotes = "FamilyLegalIssueNotes";


			// Do you currently have a child welfare worker?
		    lid.HasChildWelfareWorker = true;
		    lid.ChildWelfareNotes = longString;

			// Are you currently ordered to pay child support?
		    lid.HasChildSupport = true;
		    lid.ChildSupportAmount = "123";
		    lid.HasBackChildSupport = true;
		    lid.ChildSupportDetails = longString;

			// Have you been ordered to appear for any upcoming court dates ?
		    lid.HasUpcomingCourtDates = true;

			var courtDates = new List<CourtContract>();
			lid.CourtDates = courtDates;
			for (int i = 0; i < 10; i++)
			{
				var x = new CourtContract();
				x.Location = longString;
				x.Date = "03/01/2015";
				x.Details = longString;
				courtDates.Add(x);
			}

			lid.ActionNeededs = new List<int> { 1,2,3};
		    lid.ActionNeededDetails = longString;
		    lid.Notes = longString;

			// Act - Controller
			LegalIssuesViewModel.UpsertData(repo, "123", ia.LegalIssueData, "test");

			// Assert
			var data = (from lids in Db.LegalIssuesSections where lids.Id == 1 select lids).SingleOrDefault();

			Assert.NotNull(data);
			Assert.True(data.HasFamilyLegalIssues == true);
			Assert.True(data.IsConvictedOfCrime == true);

			Assert.True(data.Convictions.Count() == 11);

		    foreach (var x in data.Convictions)
		    {
			    if (x.ConvictionTypeID == 1 &&
			        x.DateConvicted?.ToString("MM/yyyy") == "02/2015" &&
			        x.Details == longString
				    )
			    {
				    Assert.True(true);
			    }
			    else
			    {
				    Assert.True(false);
			    }
		    }

			// Not including minor traffic violations, have you ever been convicted of a crime?
			Assert.True(data.IsConvictedOfCrime == true);
			Assert.True(data.Convictions.Count == 11);

			// Are you currently under community supervision?
			Assert.True(data.IsUnderCommunitySupervision == true);
			Assert.True(data.CommunitySupervisonDetails == "CommunitySupervisonDetails");

			// Do you have any pending charges?
			Assert.True(data.HasPendingCharges == true);

			Assert.True(data.PendingCharges.Count() == 10);

			foreach (var p in data.PendingCharges)
			{
				if (p.ConvictionTypeID == 1 &&
				p.ChargeDate?.ToString("MM/yyyy") == "03/2015" &&
				p.Details == longString
				)
				{
					Assert.True(true);
				}
				else
				{
					Assert.True(false);
				}
			}

			// Do you have any immediate family members with legal issues?
			Assert.True(data.HasFamilyLegalIssues == true);
			Assert.True(data.FamilyLegalIssueNotes == "FamilyLegalIssueNotes");

			// Do you currently have a child welfare worker?
			Assert.True(data.HasChildWelfareWorker == true);

			// Are you currently ordered to pay child support?
			Assert.True(data.OrderedToPayChildSupport == true);

			// Do you owe any back child support?
			Assert.True(data.OweAnyChildSupportBack == true);

			// Have you been ordered to appear for any upcoming court dates?
			Assert.True(data.HasCourtDates == true);

			foreach (var p in data.CourtDates)
			{
				if (p.Location == longString &&
				p.Date?.ToString("MM/yyyy") == "03/2015" &&
				p.Details == longString
				)
				{
					Assert.True(true);
				}
				else
				{
					Assert.True(false);
				}
			}

			// Action Neededs.
			Assert.True(data.LegalIssuesActionBridges.Count == 3);
			Assert.True(data.ActionNeededDetails == longString);

			// Notes
			Assert.True(data.Notes == longString);
		}
		[Fact]
		public void UpsertLegalIssuesSection_NoLegalIssuesSectionDataWithUnknownDates_SuccessfulSave()
		{
			// Arrange informal assessment with no sections.
			VMTest.IaWithNoSections(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			// Act - Simuate API POST call with JSON data binding
			var ia = new InformalAssessmentData();
			var lid = new LegalIssuesData();
			ia.LegalIssueData = lid;

			var version = new byte[2] { 1, 2 };
			lid.RowVersion = version;

			lid.IsConvictedOfCrime = true;

			string longString = null;
			for (int i = 0; i < 1000; i++)
			{
				longString += "x";
			}

			var crimes = new List<ConvictionContract>();
			lid.Convictions = crimes;
			for (int i = 0; i < 11; i++)
			{
				var x = new ConvictionContract();
				x.Type = 1;
				x.IsDateUnknown = new bool?[] {true};
				x.Details = longString;
				crimes.Add(x);
			}

			// Are you currently under community supervision?
			lid.IsParoled = true;
			lid.ParoledDetails = longString;

			lid.IsPending = true;

			var pendings = new List<PendingContract>();
			lid.Pendings = pendings;
			for (int i = 0; i < 10; i++)
			{
				var x = new PendingContract();
				x.Type = 1;
				x.IsDateUnknown = new bool?[] { true };
				x.Details = longString;
				pendings.Add(x);
			}

			// Do you have any immediate family members with legal issues?
			lid.HasFamilyLegalIssues = true;
			lid.FamilyLegalIssueNotes = longString;


			// Do you currently have a child welfare worker?
			lid.HasChildWelfareWorker = true;
			lid.ChildWelfareNotes = longString;

			// Are you currently ordered to pay child support?
			lid.HasChildSupport = true;
			lid.ChildSupportAmount = "123";
			lid.HasBackChildSupport = true;
			lid.ChildSupportDetails = longString;

			// Have you been ordered to appear for any upcoming court dates ?
			lid.HasUpcomingCourtDates = true;

			var courtDates = new List<CourtContract>();
			lid.CourtDates = courtDates;
			for (int i = 0; i < 10; i++)
			{
				var x = new CourtContract();
				x.Location = longString;
				x.IsDateUnknown = new bool?[] { true };
				x.Details = longString;
				courtDates.Add(x);
			}

			lid.ActionNeededs = new List<int> { 1, 2, 3 };
			lid.ActionNeededDetails = longString;
			lid.Notes = longString;

			// Act - Controller
			LegalIssuesViewModel.UpsertData(repo, "123", ia.LegalIssueData, "test");

			// Assert
			var data = (from lids in Db.LegalIssuesSections where lids.Id == 1 select lids).SingleOrDefault();

			Assert.NotNull(data);
			Assert.True(data.HasFamilyLegalIssues == true);
			Assert.True(data.IsConvictedOfCrime == true);

			Assert.True(data.Convictions.Count() == 11);

			foreach (var x in data.Convictions)
			{
				if (x.ConvictionTypeID == 1 &&
					x.IsUnknown == true &&
					x.Details == longString
					)
				{
					Assert.True(true);
				}
				else
				{
					Assert.True(false);
				}
			}

			// Not including minor traffic violations, have you ever been convicted of a crime?
			Assert.True(data.IsConvictedOfCrime == true);
			Assert.True(data.Convictions.Count == 11);

			// Are you currently under community supervision?
			Assert.True(data.IsUnderCommunitySupervision == true);
			Assert.True(data.CommunitySupervisonDetails == longString);

			// Do you have any pending charges?
			Assert.True(data.HasPendingCharges == true);

			Assert.True(data.PendingCharges.Count() == 10);

			foreach (var p in data.PendingCharges)
			{
				if (p.ConvictionTypeID == 1 &&
				p.IsUnknown == true &&
				p.Details == longString
				)
				{
					Assert.True(true);
				}
				else
				{
					Assert.True(false);
				}
			}

			// Do you have any immediate family members with legal issues?
			Assert.True(data.HasFamilyLegalIssues == true);

			// Do you currently have a child welfare worker?
			Assert.True(data.HasChildWelfareWorker == true);

			// Are you currently ordered to pay child support?
			Assert.True(data.OrderedToPayChildSupport == true);

			// Do you owe any back child support?
			Assert.True(data.OweAnyChildSupportBack == true);

			// Have you been ordered to appear for any upcoming court dates?
			Assert.True(data.HasCourtDates == true);

			foreach (var p in data.CourtDates)
			{
				if (p.Location == longString &&
				p.IsUnknown == true &&
				p.Details == longString
				)
				{
					Assert.True(true);
				}
				else
				{
					Assert.True(false);
				}
			}

			// Action Neededs.
			Assert.True(data.LegalIssuesActionBridges.Count == 3);
			Assert.True(data.ActionNeededDetails == longString);

			// Notes
			Assert.True(data.Notes == longString);
		}

		[Fact]
	    public void GetLegalIssuesSection_HeavyData_SuccessfulGet()
		{   
			// Arrange informal assessment with heavy legal section.
			VMTest.IaWithLegalSection(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

		    var contact1 = repo.ContactById(0);
			var contact2 = repo.ContactById(0);

			List<IContact> list = new List<IContact>();

			list.Add(contact1);
			list.Add(contact2);

			var ia = repo.InformalAssessmentById(1);
			var data = LegalIssuesViewModel.GetData(ia, list);

			Assert.True(data.HasFamilyLegalIssues == true);
			Assert.True(data.HasBackChildSupport == true);
			Assert.True(data.HasChildSupport == true);
			Assert.True(data.HasChildWelfareWorker == true);
			Assert.True(data.HasUpcomingCourtDates == true);


			Assert.True(data.CourtDates.Count == 10);

			foreach (var x in data.CourtDates)
			{
				Assert.True(x.Location == "Location");
				Assert.True(x.Details == "Details");
				Assert.True(x.Date == DateTime.Now.ToString("MM/dd/yyyy")); 
			}
			Assert.True(data.Convictions.Count == 10);

			foreach (var x in data.Convictions)
			{
				Assert.True(x.Type == 1);
				Assert.True(x.Details == "Details");
				Assert.True(x.Date == DateTime.Now.ToString("MM/yyyy"));
			}
			Assert.True(data.Pendings.Count == 10);

			foreach (var x in data.Pendings)
			{
				Assert.True(x.Type == 1);
				Assert.True(x.Details == "Details");
				Assert.True(x.Date == DateTime.Now.ToString("MM/yyyy"));
			}

			Assert.True(data.ChildWelfareNotes == "ChildWelfareNotes");
			Assert.True(data.FamilyLegalIssueNotes == "FamilyLegalIssueNotes");

			Assert.True(data.ChildSupportAmount == "23");
			Assert.True(data.ChildSupportDetails == "ChildSupportDetails");

			Assert.True(data.ActionNeededDetails == "ActionNeededDetails");

			Assert.True(data.ActionNeededs.Count == 5);

			foreach (var x in data.ActionNeededs)
			{
				Assert.True(x == 1);
			}

			Assert.True(data.Notes == "Notes");
		}

		[Fact]
		public void GetLegalIssuesSection_HeavyDataWithUnknownDates_SuccessfulGet()
		{
			// Arrange informal assessment with heavy legal section.
			VMTest.IaWithLegalSectionWithUnknowns(Db);
			IRepository repo = new Repository(Db);
			BaseViewModel.InjectDependencies(repo);

			var contact1 = repo.ContactById(0);
			var contact2 = repo.ContactById(0);

			List<IContact> list = new List<IContact>();

			list.Add(contact1);
			list.Add(contact2);

			var ia = repo.InformalAssessmentById(1);
			var data = LegalIssuesViewModel.GetData(ia, list);

			Assert.True(data.HasFamilyLegalIssues == true);
			Assert.True(data.HasBackChildSupport == true);
			Assert.True(data.HasChildSupport == true);
			Assert.True(data.HasChildWelfareWorker == true);
			Assert.True(data.HasUpcomingCourtDates == true);


			Assert.True(data.CourtDates.Count == 10);

			var IsUnknown = new bool?[] { true };
			foreach (var x in data.CourtDates)
			{
				Assert.True(x.Location == "Location");
				Assert.True(x.Details == "Details");
				Assert.True(x.Date == null);
				Assert.True(x.IsDateUnknown.FirstOrDefault() == IsUnknown.FirstOrDefault());
			}
			Assert.True(data.Convictions.Count == 10);

			foreach (var x in data.Convictions)
			{
				Assert.True(x.Type == 1);
				Assert.True(x.Details == "Details");
				Assert.True(x.Date == null);
		
				Assert.True(x.IsDateUnknown.FirstOrDefault() == IsUnknown.FirstOrDefault());
			}
			Assert.True(data.Pendings.Count == 10);

			foreach (var x in data.Pendings)
			{
				Assert.True(x.Type == 1);
				Assert.True(x.Details == "Details");
				Assert.True(x.Date == null);
				Assert.True(x.IsDateUnknown.FirstOrDefault() == IsUnknown.FirstOrDefault());
			}

			Assert.True(data.ChildWelfareNotes == "ChildWelfareNotes");
			Assert.True(data.FamilyLegalIssueNotes == "FamilyLegalIssueNotes");

			Assert.True(data.ChildSupportAmount == "23");
			Assert.True(data.ChildSupportDetails == "ChildSupportDetails");

			Assert.True(data.ActionNeededDetails == "ActionNeededDetails");

			Assert.True(data.ActionNeededs.Count == 5);

			foreach (var x in data.ActionNeededs)
			{
				Assert.True(x == 1);
			}

			Assert.True(data.Notes == "Notes");
		}
	}

	}

