using Dcf.Wwp.Data.Sql.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers
{
	public static class VMTest
	{
		#region SeedLookups

		private static void ConvictionTypeLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.ConvictionType[] convictionTypes = new Data.Sql.Model.ConvictionType[3];

			// Seed 7 HousingSituationes branches.
			for (int i = 0; i < 3; i++)
			{
				convictionTypes[i] = new Data.Sql.Model.ConvictionType();
				{
					convictionTypes[i].Name = "abc" + i.ToString();
				}

				Db.ConvictionTypes.Add(convictionTypes[i]);
			}
		}




		private static void HousingSituationLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.HousingSituation[] housingSituationes = new Data.Sql.Model.HousingSituation[7];

			// Seed 7 HousingSituationes branches.
			for (int i = 0; i <= 6; i++)
			{
				housingSituationes[i] = new Data.Sql.Model.HousingSituation();
				{
					housingSituationes[i].Name = "abc" + i.ToString();
				}

				Db.HousingSituations.Add(housingSituationes[i]);
			}
		}

		private static void ActionNeededLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.ActionNeeded[] actionNeededs = new Data.Sql.Model.ActionNeeded[7];

			// Seed 7 HousingSituationes branches.
			for (int i = 0; i <= 6; i++)
			{
				actionNeededs[i] = new Data.Sql.Model.ActionNeeded();
				{
					actionNeededs[i].ElementLabelName = "abc" + i.ToString();
					actionNeededs[i].SectionName = "LegalIssues";
				}

				Db.ActionNeededs.Add(actionNeededs[i]);
			}
		}

		private static void MilitaryBranchLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.MilitaryBranch[] militaryBranches = new Data.Sql.Model.MilitaryBranch[6];

			// Seed 6 Miltary branches.
			for (int i = 0; i <= 5; i++)
			{
				militaryBranches[i] = new Data.Sql.Model.MilitaryBranch();
				{
					militaryBranches[i].Name = "abc" + i.ToString();
				}

				Db.MilitaryBranches.Add(militaryBranches[i]);
			}
		}

		private static void MilitaryRankLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.MilitaryRank[] militaryRankes = new Data.Sql.Model.MilitaryRank[25];

			// Seed 25 Miltary branches.
			for (int i = 0; i <= 24; i++)
			{
				militaryRankes[i] = new Data.Sql.Model.MilitaryRank();
				{
					militaryRankes[i].Name = "abc" + i.ToString();
				}

				Db.MilitaryRanks.Add(militaryRankes[i]);
			}
		}

		private static void MilitaryDischargeLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.MilitaryDischargeType[] militaryDischargeTypes = new Data.Sql.Model.MilitaryDischargeType[4];

			// Seed 4 Miltary discharge types.
			for (int i = 0; i <= 3; i++)
			{
				militaryDischargeTypes[i] = new Data.Sql.Model.MilitaryDischargeType();
				{
					militaryDischargeTypes[i].Name = "abc" + i.ToString();
				}

				Db.MilitaryDischargeTypes.Add(militaryDischargeTypes[i]);
			}
		}

        private static void CareArrangmentLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
        {
            Data.Sql.Model.ChildCareArrangement[] childCareArrangements = new Data.Sql.Model.ChildCareArrangement[4];

            // Seed 4 Miltary discharge types.
            for (int i = 0; i <= 3; i++)
            {
                childCareArrangements[i] = new Data.Sql.Model.ChildCareArrangement();
                {
                    childCareArrangements[i].Name = "abc" + i.ToString();
                }

                Db.ChildCareArrangements.Add(childCareArrangements[i]);
            }
        }

		private static void LanguagesLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			Data.Sql.Model.Language[] languages = new Data.Sql.Model.Language[159];

			// Seed 160 languages.
			for (int i = 0; i < 159; i++)
			{
				languages[i] = new Data.Sql.Model.Language();
				{
					languages[i].Name = "abc" + i.ToString();
				}

				Db.Languages.Add(languages[i]);
			}
		}

		#endregion

        #region HeavySeedSections
        private static void HeavyChildCareSectionSeedWithChildrenAndTeenData(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
        {
            var cc = new Data.Sql.Model.ChildCareSection();
            //ia.ChildCareSection = cc;
            cc.ModifiedBy = "Sohinder";
            cc.HasChildren12OrUnder = true;
            var ccc = new ChildCareChild();
            ccc.Name = "Sohi";
            ccc.ChildCareArrangementId = 1;
            ccc.Details = "xxx";
            ccc.IsSpecialNeeds = false;
            ccc.HasDisability = false;
            ccc.IsDeleted = false;
            cc.ChildCareChilds.Add(ccc);
            cc.HasChildren12OrUnder = true;
            cc.HasChildrenWithDisabilityInNeedOfChildCare = true;
            var teen = new ChildCareChild();
            teen.Name = "Dinu";
            teen.Details = "yyyyy";
            teen.HasDisability = true;
            teen.IsSpecialNeeds = true;
            teen.IsDeleted = false;
            cc.ChildCareChilds.Add(teen);
            cc.HasFutureChildCareNeed = true;
            cc.FutureChildCareNeedNotes = "zzzz";
            //cc.HasNoAssistanceNeed = true;
            //cc.AssistDetails = "aaaaa";
            cc.Notes = "Need child care";
            ia.ChildCareSection = cc;
            //Db.ChildCareSections.Add(cc);

        }
        #endregion
		#region SeedSections
		private static InformalAssessment TestParticipantWithIASeedReturnsIA(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			p.InformalAssessments.Add(ia);

			Db.Participants.Add(p);

			return ia;
		}

		private static void LanguageSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
		{
			var la = new Data.Sql.Model.LanguageSection();
			ia.LanguageSection = la;
			la.ModifiedBy = "SohinderTest";
			Db.LanguageSections.Add(la);
		}

		private static void LegalSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
		{
			var li = new Data.Sql.Model.LegalIssuesSection();
			ia.LegalIssuesSection = li;
			li.ModifiedBy = "SohinderTest";
			Db.LegalIssuesSections.Add(li);
		}


		private static WorkProgramSection WorkProgramsSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
		{
			var wps = new Data.Sql.Model.WorkProgramSection();
			ia.WorkProgramSection = wps;
			wps.ModifiedBy = "SohinderTest";
			Db.WorkProgramSections.Add(wps);
			return wps;
		}

		private static void MilitarySectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
		{
			var mts = new Data.Sql.Model.MilitaryTrainingSection();
			ia.MilitaryTrainingSection = mts;
			mts.ModifiedBy = "SohinderTest";
			Db.MilitaryTrainingSections.Add(mts);
		}

		private static void ChildCareSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
		{
			var cc = new Data.Sql.Model.ChildCareSection();
			ia.ChildCareSection = cc;
			cc.ModifiedBy = "SohinderTest";
			Db.ChildCareSections.Add(cc);
		}

		private static void HousingSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
		{
			var hs = new Data.Sql.Model.HousingSection();
			ia.HousingSection = hs;
			hs.ModifiedBy = "SohinderTest";
			Db.HousingSections.Add(hs);
		}

		#endregion


		public static void IaWithChildCareSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var ia = TestParticipantWithIASeedReturnsIA(Db);
            CareArrangmentLookupDataSeed(Db);
            ChildCareSectionSeed(Db, ia);
		Db.SaveChanges();

		}

        public static void IaWithHeavyChildCareSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
            var ia = TestParticipantWithIASeedReturnsIA(Db);
            CareArrangmentLookupDataSeed(Db);
            HeavyChildCareSectionSeedWithChildrenAndTeenData(Db, ia);
		Db.SaveChanges();
		}


		public static void IaWithHousingSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			Db.Participants.Add(p);

			p.InformalAssessments.Add(ia);
			HousingSituationLookupDataSeed(Db);
			HousingSectionSeed(Db, ia);

			Db.SaveChanges();

		}
		public static void IaWithHeavyHousingSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			Db.Participants.Add(p);

			p.InformalAssessments.Add(ia);
			HousingSituationLookupDataSeed(Db);
			HousingSectionSeed(Db, ia);
			Db.SaveChanges();

			string longString = null;
		
			for (int i = 0; i < 1000 ; i++)
			{
				longString += "x";

			}

			var housingHistories = new List<HousingHistory>();

			for (int i = 0; i < 20; i++)
			{
				var x = new HousingHistory();
				x.Details = longString;
				x.HasEvicted = true;
				x.HousingSituationId = 1;
				x.YearsLivingInMonths = 999;
				x.RentPaid = 32;
				x.IsDeleted = false;
				housingHistories.Add(x);
			}

			for (int i = 0; i < 20; i++)
			{
				var x = new HousingHistory();
				x.Details = longString;
				x.HasEvicted = true;
				x.HousingSituationId = 1;
				x.YearsLivingInMonths = 999;
				x.RentPaid = 32;
				x.IsDeleted = true;
				housingHistories.Add(x);
			}

			ia.HousingSection.HousingHistories = housingHistories;
			ia.HousingSection.CurrentHousingDetails = longString;
			ia.HousingSection.HasEvictionRisk = true;
			//ia.HousingSection.HousingAssistanceDetails = longString;
			//ia.HousingSection.HousingSituationId = 1;
			//ia.HousingSection.RentPaid = 12;
			//ia.HousingSection.CurrentHousingMonths = 13;

			//ia.HousingSection.HasUtilityRiskDisconnection = true;
			//ia.HousingSection.UtilityRiskDisconnectionNotes = longString;


			//ia.HousingSection.HasParticipationDifficulties = true;
			//ia.HousingSection.ParticiapantDifficultNotes = longString;


			ia.HousingSection.Notes = longString;


			// Radio buttons
			//ia.HousingSection.HasRefertoCommunityProgramOrShelter = true;
			//ia.HousingSection.HasReferToOtherAgency = true;
			//ia.HousingSection.HasReferToSubsidizedHousing = true;
			//ia.HousingSection.HasEnergyAssistance = true;
			//ia.HousingSection.HasEmergencyAssistance = true;
			//ia.HousingSection.HasEmergencyPayment = true;
			//ia.HousingSection.HasJobAccessLoan = true;
			//ia.HousingSection.HasReferToOtherAgency = true;

			//ia.HousingSection.ReferToOtherAgencyNotes = longString;

			Db.SaveChanges();
		}


		public static void IaHeavyWithFalsesHousingSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			Db.Participants.Add(p);

			p.InformalAssessments.Add(ia);
			HousingSituationLookupDataSeed(Db);
			HousingSectionSeed(Db, ia);
			Db.SaveChanges();

			string longString = null;

			for (int i = 0; i < 1000; i++)
			{
				longString += "x";

			}

			var housingHistories = new List<HousingHistory>();

			for (int i = 0; i < 20; i++)
			{
				var x = new HousingHistory();
				x.Details = longString;
				x.HasEvicted = true;
				x.HousingSituationId = 1;
				x.YearsLivingInMonths = 999;
				x.RentPaid = 32;
				x.IsDeleted = false;
				housingHistories.Add(x);
			}

			for (int i = 0; i < 20; i++)
			{
				var x = new HousingHistory();
				x.Details = longString;
				x.HasEvicted = true;
				x.HousingSituationId = 1;
				x.YearsLivingInMonths = 999;
				x.RentPaid = 32;
				x.IsDeleted = true;
				housingHistories.Add(x);
			}

			ia.HousingSection.HousingHistories = housingHistories;
			ia.HousingSection.CurrentHousingDetails = longString;
			ia.HousingSection.HasEvictionRisk = false;
			//ia.HousingSection.HousingAssistanceDetails = longString;
			//ia.HousingSection.HousingSituationId = 1;
			//ia.HousingSection.RentPaid = 12;
			//ia.HousingSection.CurrentHousingMonths = 13;

	

			//ia.HousingSection.HasUtilityRiskDisconnection = false;
			//ia.HousingSection.UtilityRiskDisconnectionNotes = longString;


			//ia.HousingSection.HasParticipationDifficulties = false;
			//ia.HousingSection.ParticiapantDifficultNotes = longString;


			ia.HousingSection.Notes = longString;


			// Radio buttons
			//ia.HousingSection.HasRefertoCommunityProgramOrShelter = false;
			//ia.HousingSection.HasReferToOtherAgency = false;
			//ia.HousingSection.HasReferToSubsidizedHousing = false;
			//ia.HousingSection.HasEnergyAssistance = false;
			//ia.HousingSection.HasEmergencyAssistance = false;
			//ia.HousingSection.HasEmergencyPayment = false;
			//ia.HousingSection.HasJobAccessLoan = false;
			//ia.HousingSection.HasReferToOtherAgency = false;

			//ia.HousingSection.ReferToOtherAgencyNotes = longString;

			Db.SaveChanges();
		}

		public static void IaWithMilitarySection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			p.InformalAssessments.Add(ia);

			MilitaryBranchLookupDataSeed(Db);
			MilitaryRankLookupDataSeed(Db);
			MilitaryDischargeLookupDataSeed(Db);


			MilitarySectionSeed(Db, ia);


			Db.Participants.Add(p);
			Db.SaveChanges();

		}


		public static void IaWithLanguagesSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			p.InformalAssessments.Add(ia);

			LanguagesLookupDataSeed(Db);
			LanguageSectionSeed(Db, ia);

			Db.Participants.Add(p);
			Db.SaveChanges();

		}


		#region LegalIssuesSection


		public static void IaWithLegalSectionWithUnknowns(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			p.InformalAssessments.Add(ia);

			ConvictionTypeLookupDataSeed(Db);
			ActionNeededLookupDataSeed(Db);
			LegalSectionSeed(Db, ia);

			Db.Participants.Add(p);
			Db.SaveChanges();

			// Not including minor traffic violations, have you ever been convicted of a crime?
			ia.LegalIssuesSection.IsConvictedOfCrime = true;
			var convictions = new List<Conviction>();

			for (int i = 0; i < 10; i++)
			{
				var conviction = new Conviction();
				conviction.ConvictionTypeID = 1;
				conviction.Details = "Details";
				conviction.IsUnknown = true;
				conviction.IsDeleted = false;
				convictions.Add(conviction);
			}
			ia.LegalIssuesSection.Convictions = convictions;


			// Are you currently under community supervision?
			ia.LegalIssuesSection.IsUnderCommunitySupervision = true;
			ia.LegalIssuesSection.CommunitySupervisonDetails = "CommunitySupervisonDetails";


			// Do you have any pending charges?
			ia.LegalIssuesSection.HasPendingCharges = true;
			var pendingCharges = new List<PendingCharge>();

			for (int i = 0; i < 10; i++)
			{
				var pendingCharge = new PendingCharge();
				pendingCharge.ConvictionTypeID = 1;
				pendingCharge.Details = "Details";
				pendingCharge.IsUnknown = true;
				pendingCharge.IsDeleted = false;
				pendingCharges.Add(pendingCharge);
			}
			ia.LegalIssuesSection.PendingCharges = pendingCharges;

			// Do you have any immediate family members with legal issues?
			ia.LegalIssuesSection.HasFamilyLegalIssues = true;
			ia.LegalIssuesSection.FamilyLegalIssueNotes = "FamilyLegalIssueNotes";


			// Do you currently have a child welfare worker?
			ia.LegalIssuesSection.HasChildWelfareWorker = true;
			ia.LegalIssuesSection.ChildWelfareNotes = "ChildWelfareNotes";

			// Are you currently ordered to pay child support?
			ia.LegalIssuesSection.OrderedToPayChildSupport = true;
			ia.LegalIssuesSection.OweAnyChildSupportBack = true;
			ia.LegalIssuesSection.MonthlyAmount = 23m;
			ia.LegalIssuesSection.ChildSupportDetails = "ChildSupportDetails";

			// Have you been ordered to appear for any upcoming court dates?
			ia.LegalIssuesSection.HasCourtDates = true;

			var courtDates = new List<CourtDate>();

			for (int i = 0; i < 10; i++)
			{
				var courtDate = new CourtDate();
				courtDate.Location = "Location";
				courtDate.Details = "Details";
				courtDate.IsUnknown = true;
				courtDate.IsDeleted = false;
				courtDates.Add(courtDate);
			}
			ia.LegalIssuesSection.CourtDates = courtDates;


			// ACTION NEEDED
			ia.LegalIssuesSection.ActionNeededDetails = "ActionNeededDetails";

			var legalIssuesActionBridges = new List<LegalIssuesActionBridge>();

			for (int i = 0; i < 5; i++)
			{
				var legalIssuesActionBridge = new LegalIssuesActionBridge();
				legalIssuesActionBridge.ActionNeededId = 1;
				legalIssuesActionBridge.IsDeleted = false;
				legalIssuesActionBridges.Add(legalIssuesActionBridge);
			}

			ia.LegalIssuesSection.LegalIssuesActionBridges = legalIssuesActionBridges;
			ia.LegalIssuesSection.Notes = "Notes";

			Db.SaveChanges();

		}

		public static void IaWithLegalSection(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			p.InformalAssessments.Add(ia);

			ConvictionTypeLookupDataSeed(Db);
			ActionNeededLookupDataSeed(Db);
			LegalSectionSeed(Db, ia);

			Db.Participants.Add(p);
			Db.SaveChanges();

			// Not including minor traffic violations, have you ever been convicted of a crime?
			ia.LegalIssuesSection.IsConvictedOfCrime = true;
			var convictions = new List<Conviction>();

			for (int i = 0; i < 10; i++)
			{
				var conviction = new Conviction();
				conviction.ConvictionTypeID = 1;
				conviction.Details = "Details";
				conviction.DateConvicted = _authUser.CDODate ?? DateTime.Now;
				conviction.IsDeleted = false;
				convictions.Add(conviction);
			}
			ia.LegalIssuesSection.Convictions = convictions;


			// Are you currently under community supervision?
			ia.LegalIssuesSection.IsUnderCommunitySupervision = true;
			ia.LegalIssuesSection.CommunitySupervisonDetails = "CommunitySupervisonDetails";


			// Do you have any pending charges?
			ia.LegalIssuesSection.HasPendingCharges = true;
			var pendingCharges = new List<PendingCharge>();

			for (int i = 0; i < 10; i++)
			{
				var pendingCharge = new PendingCharge();
				pendingCharge.ConvictionTypeID = 1;
				pendingCharge.Details = "Details";
				pendingCharge.ChargeDate = _authUser.CDODate ?? DateTime.Now;
				pendingCharge.IsDeleted = false;
				pendingCharges.Add(pendingCharge);
			}
			ia.LegalIssuesSection.PendingCharges = pendingCharges;

			// Do you have any immediate family members with legal issues?
			ia.LegalIssuesSection.HasFamilyLegalIssues = true;
			ia.LegalIssuesSection.FamilyLegalIssueNotes = "FamilyLegalIssueNotes";


			// Do you currently have a child welfare worker?
			ia.LegalIssuesSection.HasChildWelfareWorker = true;
			ia.LegalIssuesSection.ChildWelfareNotes = "ChildWelfareNotes";

			// Are you currently ordered to pay child support?
			ia.LegalIssuesSection.OrderedToPayChildSupport = true;
			ia.LegalIssuesSection.OweAnyChildSupportBack = true;
			ia.LegalIssuesSection.MonthlyAmount = 23m;
			ia.LegalIssuesSection.ChildSupportDetails = "ChildSupportDetails";

			// Have you been ordered to appear for any upcoming court dates?
			ia.LegalIssuesSection.HasCourtDates = true;

			var courtDates = new List<CourtDate>();

			for (int i = 0; i < 10; i++)
			{
				var courtDate = new CourtDate();
				courtDate.Location = "Location";
				courtDate.Details = "Details";
				courtDate.Date = _authUser.CDODate ?? DateTime.Now;
				courtDate.IsDeleted = false;
				courtDates.Add(courtDate);
			}
			ia.LegalIssuesSection.CourtDates = courtDates;


			// ACTION NEEDED
			ia.LegalIssuesSection.ActionNeededDetails = "ActionNeededDetails";

			var legalIssuesActionBridges = new List<LegalIssuesActionBridge>();

			for (int i = 0; i < 5; i++)
			{
				var legalIssuesActionBridge = new LegalIssuesActionBridge();
				legalIssuesActionBridge.ActionNeededId = 1;
				legalIssuesActionBridge.IsDeleted = false;
				legalIssuesActionBridges.Add(legalIssuesActionBridge);
			}

			ia.LegalIssuesSection.LegalIssuesActionBridges = legalIssuesActionBridges;
			ia.LegalIssuesSection.Notes = "Notes";

			Db.SaveChanges();

		}

#endregion
		public static void IaWithWorkProgramsSection(Wwp.Data.Sql.Model.WwpEntities Db, bool restoreData = false)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			p.InformalAssessments.Add(ia);

			var wps = WorkProgramsSectionSeed(Db,ia);


			if (restoreData == true)
			{
				var iwp = new InvolvedWorkProgram();
				iwp.Details = "Test";
				iwp.IsDeleted = true;
				wps.InvolvedWorkPrograms.Add(iwp);
				//Db.InvolvedWorkPrograms.Add(iwp);
			}

			Db.Participants.Add(p);
			Db.SaveChanges();

		}

		public static void IaWithNoSections(Wwp.Data.Sql.Model.WwpEntities Db)
		{
			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "Ashan";
			p.PinNumber = 123;

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;

			LanguagesLookupDataSeed(Db);
			MilitaryBranchLookupDataSeed(Db);
			MilitaryRankLookupDataSeed(Db);
			MilitaryDischargeLookupDataSeed(Db);
            CareArrangmentLookupDataSeed(Db);
			ConvictionTypeLookupDataSeed(Db);
			HousingSituationLookupDataSeed(Db);
			ActionNeededLookupDataSeed(Db);


			Db.Participants.Add(p);
			Db.SaveChanges();
		}


		public static void LanguageAndEducationSection_withKnownLanguage_Data(Wwp.Data.Sql.Model.WwpEntities Db)
		{

			var la = new Data.Sql.Model.LanguageSection();
			la.ModifiedBy = "Ram";
			Db.LanguageSections.Add(la);
			Db.SaveChanges();

			byte[] v1 = new byte[2] { 1, 3 };

			var co = new Country();
			co.ModifiedBy = "Dinu";
			co.ModifiedDate = DateTime.Now;
			co.Name = "United States";


			Db.Countries.Add(co);

			var st = new Data.Sql.Model.State();
			st.ModifiedBy = "Sohi";
			st.ModifiedDate = DateTime.Now;
			st.Code = "WI";
			st.Country = co;
			Db.States.Add(st);

			var s = new Data.Sql.Model.SchoolCollegeEstablishment();
			s.ModifiedBy = "Sohi";
			s.ModifiedDate = DateTime.Now;
			//s.State = st;
			s.Street = "123";
			s.Name = "West";
			//s.City = "MD";
			Db.SchoolCollegeEstablishments.Add(s);

			var cia = new CertificateIssuingAuthority();
			cia.ModifiedBy = "Sohi";
			cia.ModifiedDate = DateTime.Now;
			cia.Code = "iA";
			cia.SortOrder = 1;
			Db.CertificateIssuingAuthorities.Add(cia);

			var c1 = new Data.Sql.Model.SchoolGraduationStatus();
			c1.ModifiedBy = "Sohi";
			c1.ModifiedDate = DateTime.Now;
			Db.SchoolGraduationStatus.Add(c1);

			var c2 = new Data.Sql.Model.SchoolGradeLevel();
			c2.ModifiedBy = "Sohi";
			c2.SortOrder = 1;
			c2.ModifiedDate = DateTime.Now;
			c2.Grade = 1;
			Db.SchoolGradeLevels.Add(c2);

			var ea = new Data.Sql.Model.EducationSection();
			ea.ModifiedBy = "Sohi";
			ea.ModifiedDate = DateTime.Now;
			ea.SchoolCollegeEstablishment = s;
			ea.SchoolGraduationStatus = c1;
			ea.SchoolGradeLevel = c2;
			ea.CertificateIssuingAuthority = cia;
			Db.EducationSections.Add(ea);
			Db.SaveChanges();

			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "cheedu";
			p.PinNumber = 123;
			Db.Participants.Add(p);
			Db.SaveChanges();


			Data.Sql.Model.Language[] ixx = new Data.Sql.Model.Language[45];

			for (int ix = 0; ix < 41; ix++)
			{
				ixx[ix] = new Data.Sql.Model.Language();
				{
					ixx[ix].Name = "abc";
					if (ix == 40)
					{
						ixx[ix].Name = "English";
					}
					if (ix == 39)
					{
						ixx[ix].Name = "Spanish";
					}
					if (ix == 38)
					{
						ixx[ix].Name = "Latin";
					}
					if (ix == 37)
					{
						ixx[ix].Name = "Spanish";
					}
				}

				Db.Languages.Add(ixx[ix]);
				Db.SaveChanges();
			}
			//int ix = 0;
			//ixx[ix] = new Data.Sql.Model.Language();
			//foreach (int ixx[] in ixx[ix] )

			var kl = new Data.Sql.Model.KnownLanguage();
			kl.LanguageSection = la;
			//kl.Language = ixx;
			kl.ModifiedDate = DateTime.Now;
			kl.IsPrimary = true;
			kl.LanguageId = 40;
			Db.KnownLanguages.Add(kl);
			Db.SaveChanges();

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;
			ia.LanguageSection = la;

			ia.EducationSection = ea;
			ia.ModifiedDate = DateTime.Now;
			Db.InformalAssessments.Add(ia);
			Db.SaveChanges();

			var part = (from x in Db.Participants where x.PinNumber == 123m select x).FirstOrDefault();
			var part1 = (from x in Db.InformalAssessments where x.ParticipantId == 1 select x).FirstOrDefault();
		}
	}
}

//        public static void LanguageSection_withNoKnownLanguage_Data(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var la = new Data.Sql.Model.LanguageSection();
//            la.ModifiedBy = "Ram";
//            Db.LanguageSections.Add(la);
//            Db.SaveChanges();


//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "cheedu";
//            p.PinNumber = 123;
//            Db.Participants.Add(p);
//            Db.SaveChanges();


//            Data.Sql.Model.Language[] ixx = new Data.Sql.Model.Language[45];

//            for (int ix = 0; ix < 41; ix++)
//            {
//                ixx[ix] = new Data.Sql.Model.Language();
//                {
//                    ixx[ix].Name = "abc";
//                    if (ix == 40)
//                    {
//                        ixx[ix].Name = "English";
//                    }
//                }

//                Db.Languages.Add(ixx[ix]);
//                Db.SaveChanges();
//            }
//            //int ix = 0;
//            //ixx[ix] = new Data.Sql.Model.Language();
//            //foreach (int ixx[] in ixx[ix] )

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            ia.LanguageSection = la;
//            ia.ModifiedDate = DateTime.Now;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();
//        }

//        }
//    }
