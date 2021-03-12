//using Dcf.Wwp.Data.Sql.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Dcf.Wwp.Model.Cww;
//using Dcf.Wwp.Model.Interface;
//using Dcf.Wwp.Model.Interface.Cww;
//using Dcf.Wwp.Model.Interface.Repository;
//using Moq;

//namespace Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers
//{
//    public static class VMTest
//    {
//        #region SeedLookups

//        private static void ConvictionTypeLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ConvictionType[] convictionTypes = new Data.Sql.Model.ConvictionType[3];

//            // Seed 7 HousingSituationes branches.
//            for (int i = 0; i < 3; i++)
//            {
//                convictionTypes[i] = new Data.Sql.Model.ConvictionType();
//                {
//                    convictionTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.ConvictionTypes.Add(convictionTypes[i]);
//            }
//        }




//        private static void HousingSituationLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.HousingSituation[] housingSituationes = new Data.Sql.Model.HousingSituation[7];

//            // Seed 7 HousingSituationes branches.
//            for (int i = 0; i <= 6; i++)
//            {
//                housingSituationes[i] = new Data.Sql.Model.HousingSituation();
//                {
//                    housingSituationes[i].Name = "abc" + i.ToString();
//                }

//                Db.HousingSituations.Add(housingSituationes[i]);
//            }
//        }

//        private static void ActionNeededLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ActionNeeded[] actionNeededs = new Data.Sql.Model.ActionNeeded[7];

//            // Seed 7 HousingSituationes branches.
//            for (int i = 0; i <= 6; i++)
//            {
//                actionNeededs[i] = new Data.Sql.Model.ActionNeeded();
//                {
//                    actionNeededs[i].ElementLabelName = "abc" + i.ToString();
//                    actionNeededs[i].SectionName = "LegalIssues";
//                }

//                Db.ActionNeededs.Add(actionNeededs[i]);
//            }
//        }

//        private static void MilitaryBranchLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.MilitaryBranch[] militaryBranches = new Data.Sql.Model.MilitaryBranch[6];

//            // Seed 6 Miltary branches.
//            for (int i = 0; i <= 5; i++)
//            {
//                militaryBranches[i] = new Data.Sql.Model.MilitaryBranch();
//                {
//                    militaryBranches[i].Name = "abc" + i.ToString();
//                }

//                Db.MilitaryBranches.Add(militaryBranches[i]);
//            }
//        }

//        private static void MilitaryRankLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.MilitaryRank[] militaryRankes = new Data.Sql.Model.MilitaryRank[25];

//            // Seed 25 Miltary branches.
//            for (int i = 0; i <= 24; i++)
//            {
//                militaryRankes[i] = new Data.Sql.Model.MilitaryRank();
//                {
//                    militaryRankes[i].Name = "abc" + i.ToString();
//                }

//                Db.MilitaryRanks.Add(militaryRankes[i]);
//            }
//        }

//        private static void EmploymentSatusTypeLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.EmploymentStatusType[] employmentStatusTypes = new Data.Sql.Model.EmploymentStatusType[4];

//            // Seed 3 Employment Status types.
//            for (int i = 0; i <= 3; i++)
//            {
//                employmentStatusTypes[i] = new Data.Sql.Model.EmploymentStatusType();
//                {
//                    employmentStatusTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.EmploymentStatusTypes.Add(employmentStatusTypes[i]);
//            }
//        }

//        private static void MilitaryDischargeLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.MilitaryDischargeType[] militaryDischargeTypes = new Data.Sql.Model.MilitaryDischargeType[4];

//            // Seed 4 Miltary discharge types.
//            for (int i = 0; i <= 3; i++)
//            {
//                militaryDischargeTypes[i] = new Data.Sql.Model.MilitaryDischargeType();
//                {
//                    militaryDischargeTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.MilitaryDischargeTypes.Add(militaryDischargeTypes[i]);
//            }
//        }

//        private static void CareArrangmentLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ChildCareArrangement[] childCareArrangements = new Data.Sql.Model.ChildCareArrangement[4];

//            // Seed 4 Miltary discharge types.
//            for (int i = 0; i <= 3; i++)
//            {
//                childCareArrangements[i] = new Data.Sql.Model.ChildCareArrangement();
//                {
//                    childCareArrangements[i].Name = "abc" + i.ToString();
//                }

//                Db.ChildCareArrangements.Add(childCareArrangements[i]);
//            }
//        }

//        private static void LanguagesLookupDataSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.Language[] languages = new Data.Sql.Model.Language[159];

//            // Seed 160 languages.
//            for (int i = 1; i <= 150; i++)
//            {
//                languages[i] = new Data.Sql.Model.Language();
//                {
//                    languages[i].Name = "abc" + i.ToString();
//                }

//                Db.Languages.Add(languages[i]);
//            }
//        }
//        private static void ActionNeededLookupDataSeedForWorkHistorySection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ActionNeeded[] actionNeededs = new Data.Sql.Model.ActionNeeded[11];

//            //Seed 10 action items
//            for (int i = 0; i <= 10; i++)
//            {
//                actionNeededs[i] = new Data.Sql.Model.ActionNeeded();
//                {
//                    actionNeededs[i].ElementLabelName = "abc" + i.ToString();
//                    actionNeededs[i].SectionName = "WorkHistorySection";
//                }

//                Db.ActionNeededs.Add(actionNeededs[i]);
//            }
//        }

      
//        private static void ExamTypeSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ExamType[] examTypes = new Data.Sql.Model.ExamType[9];

//            // Seed 8 exam types.
//            for (int i = 0; i <= 8; i++)
//            {
//                examTypes[i] = new Data.Sql.Model.ExamType();
//                {
//                    examTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.ExamTypes.Add(examTypes[i]);
//            }
//        }

//        private static void ExamSubjectTypeSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ExamSubjectType[] examSubjectTypes = new Data.Sql.Model.ExamSubjectType[15];

//            // Seed 14 exam subject types.
//            for (int i = 0; i <= 14; i++)
//            {
//                examSubjectTypes[i] = new Data.Sql.Model.ExamSubjectType();
//                {
//                    examSubjectTypes[i].Name = "GSED" + i.ToString();
//                }

//                Db.ExamSubjectTypes.Add(examSubjectTypes[i]);
//            }
//        }

//        private static void NrsTypeSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.NRSType[] nrsTypes = new Data.Sql.Model.NRSType[13];

//            // Seed 12 NrsTypes.
//            for (int i = 0; i <= 12; i++)
//            {
//                nrsTypes[i] = new Data.Sql.Model.NRSType();
//                {
//                    nrsTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.NRSTypes.Add(nrsTypes[i]);
//            }
//        }

//        private static void SplTypeSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.SPLType[] splTypes = new Data.Sql.Model.SPLType[13];

//            // Seed 12 spl types.
//            for (int i = 0; i <= 12; i++)
//            {
//                splTypes[i] = new Data.Sql.Model.SPLType();
//                {
//                    splTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.SPLTypes.Add(splTypes[i]);
//            }
//        }

//        private static void ExamPassTypeSeed(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            Data.Sql.Model.ExamPassType[] examPassTypes = new Data.Sql.Model.ExamPassType[3];

//            // Seed 2 exam pass types
//            for (int i = 0; i <= 2; i++)
//            {
//                examPassTypes[i] = new Data.Sql.Model.ExamPassType();
//                {
//                    examPassTypes[i].Name = "abc" + i.ToString();
//                }

//                Db.ExamPassTypes.Add(examPassTypes[i]);
//            }
//        }

//        #endregion

//        #region HeavySeedSections
//        private static void HeavyChildCareSectionSeedWithChildrenAndTeenData(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var cc = new Data.Sql.Model.ChildCareSection();
//            //ia.ChildCareSection = cc;
//            cc.ModifiedBy = "Sohinder";
//            cc.HasChildren12OrUnder = true;
//            var ccc = new ChildCareChild();
//            ccc.Name = "Sohi";
//            ccc.ChildCareArrangementId = 1;
//            ccc.Details = "xxx";
//            ccc.IsSpecialNeeds = false;
//          //  ccc.HasDisability = false;
//            ccc.IsDeleted = false;
//            cc.ChildCareChilds.Add(ccc);
//            cc.HasChildren12OrUnder = true;
//            cc.HasChildrenWithDisabilityInNeedOfChildCare = true;
//            var teen = new ChildCareChild();
//            teen.Name = "Dinu";
//            teen.Details = "yyyyy";
//         //   teen.HasDisability = true;
//            teen.IsSpecialNeeds = true;
//            teen.IsDeleted = false;
//            cc.ChildCareChilds.Add(teen);
//            cc.HasFutureChildCareNeed = true;
//            cc.FutureChildCareNeedNotes = "zzzz";
//            //cc.HasNoAssistanceNeed = true;
//            //cc.AssistDetails = "aaaaa";
//            cc.Notes = "Need child care";
//            ia.ChildCareSection = cc;
//            //Db.ChildCareSections.Add(cc);

//        }
//        #endregion
//        #region SeedSections
//        private static InformalAssessment TestParticipantWithIASeedReturnsIA(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);

//            Db.Participants.Add(p);

//            return ia;
//        }

//        private static void LanguageSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var la = new Data.Sql.Model.LanguageSection();
//            ia.LanguageSection = la;
//            la.ModifiedBy = "SohinderTest";
//            Db.LanguageSections.Add(la);
//        }

//        private static void LegalSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var li = new Data.Sql.Model.LegalIssuesSection();
//            ia.LegalIssuesSection = li;
//            li.ModifiedBy = "SohinderTest";
//            Db.LegalIssuesSections.Add(li);
//        }


//        private static WorkProgramSection WorkProgramsSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var wps = new Data.Sql.Model.WorkProgramSection();
//            ia.WorkProgramSection = wps;
//            wps.ModifiedBy = "SohinderTest";
//            Db.WorkProgramSections.Add(wps);
//            return wps;
//        }

//        private static void MilitarySectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var mts = new Data.Sql.Model.MilitaryTrainingSection();
//            ia.MilitaryTrainingSection = mts;
//            mts.ModifiedBy = "SohinderTest";
//            Db.MilitaryTrainingSections.Add(mts);
//        }

//        private static void ChildCareSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var cc = new Data.Sql.Model.ChildCareSection();
//            ia.ChildCareSection = cc;
//            cc.ModifiedBy = "SohinderTest";
//            Db.ChildCareSections.Add(cc);
//        }

//        private static void HousingSectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var hs = new Data.Sql.Model.HousingSection();
//            ia.HousingSection = hs;
//            hs.ModifiedBy = "SohinderTest";
//            Db.HousingSections.Add(hs);
//        }
//        private static void WorkHistorySectionSeed(Wwp.Data.Sql.Model.WwpEntities Db, InformalAssessment ia)
//        {
//            var whs = new Data.Sql.Model.WorkHistorySection();
//            ia.WorkHistorySection = whs;
//            whs.ModifiedBy = "dinesh";
//            Db.WorkHistorySections.Add(whs);

//        }

//        #endregion


//        public static void IaWithChildCareSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var ia = TestParticipantWithIASeedReturnsIA(Db);
//            CareArrangmentLookupDataSeed(Db);
//            ChildCareSectionSeed(Db, ia);
//            Db.SaveChanges();

//        }

//        public static void IaWithHeavyChildCareSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var ia = TestParticipantWithIASeedReturnsIA(Db);
//            CareArrangmentLookupDataSeed(Db);
//            HeavyChildCareSectionSeedWithChildrenAndTeenData(Db, ia);
//            Db.SaveChanges();
//        }


//        public static void IaWithHousingSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            Db.Participants.Add(p);

//            p.InformalAssessments.Add(ia);
//            HousingSituationLookupDataSeed(Db);
//            HousingSectionSeed(Db, ia);

//            Db.SaveChanges();

//        }
//        public static void IaWithHeavyHousingSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            Db.Participants.Add(p);

//            p.InformalAssessments.Add(ia);
//            HousingSituationLookupDataSeed(Db);
//            HousingSectionSeed(Db, ia);
//            Db.SaveChanges();

//            string longString = null;

//            for (int i = 0; i < 1000; i++)
//            {
//                longString += "x";

//            }

//            var housingHistories = new List<HousingHistory>();

//            for (int i = 0; i < 20; i++)
//            {
//                var x = new HousingHistory();
//                x.Details = longString;
//                x.HasEvicted = true;
//                x.HousingSituationId = 1;
//                //x.YearsLivingInMonths = 999;
//                //x.RentPaid = 32;
//                x.IsDeleted = false;
//                housingHistories.Add(x);
//            }

//            for (int i = 0; i < 20; i++)
//            {
//                var x = new HousingHistory();
//                x.Details = longString;
//                x.HasEvicted = true;
//                x.HousingSituationId = 1;
//                //x.YearsLivingInMonths = 999;
//                //x.RentPaid = 32;
//                x.IsDeleted = true;
//                housingHistories.Add(x);
//            }

//            ia.HousingSection.HousingHistories = housingHistories;
//            ia.HousingSection.CurrentHousingDetails = longString;
//           // ia.HousingSection.HasEvictionRisk = true;
//            //ia.HousingSection.HousingAssistanceDetails = longString;
//            //ia.HousingSection.HousingSituationId = 1;
//            //ia.HousingSection.RentPaid = 12;
//            //ia.HousingSection.CurrentHousingMonths = 13;

//            //ia.HousingSection.HasUtilityRiskDisconnection = true;
//            //ia.HousingSection.UtilityRiskDisconnectionNotes = longString;


//            //ia.HousingSection.HasParticipationDifficulties = true;
//            //ia.HousingSection.ParticiapantDifficultNotes = longString;


//            ia.HousingSection.Notes = longString;


//            // Radio buttons
//            //ia.HousingSection.HasRefertoCommunityProgramOrShelter = true;
//            //ia.HousingSection.HasReferToOtherAgency = true;
//            //ia.HousingSection.HasReferToSubsidizedHousing = true;
//            //ia.HousingSection.HasEnergyAssistance = true;
//            //ia.HousingSection.HasEmergencyAssistance = true;
//            //ia.HousingSection.HasEmergencyPayment = true;
//            //ia.HousingSection.HasJobAccessLoan = true;
//            //ia.HousingSection.HasReferToOtherAgency = true;

//            //ia.HousingSection.ReferToOtherAgencyNotes = longString;

//            Db.SaveChanges();
//        }


//        public static void IaHeavyWithFalsesHousingSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            Db.Participants.Add(p);

//            p.InformalAssessments.Add(ia);
//            HousingSituationLookupDataSeed(Db);
//            HousingSectionSeed(Db, ia);
//            Db.SaveChanges();

//            string longString = null;

//            for (int i = 0; i < 1000; i++)
//            {
//                longString += "x";

//            }

//            var housingHistories = new List<HousingHistory>();

//            for (int i = 0; i < 20; i++)
//            {
//                var x = new HousingHistory();
//                x.Details = longString;
//                x.HasEvicted = true;
//                x.HousingSituationId = 1;
//              //  x.YearsLivingInMonths = 999;
//              //  x.RentPaid = 32;
//                x.IsDeleted = false;
//                housingHistories.Add(x);
//            }

//            for (int i = 0; i < 20; i++)
//            {
//                var x = new HousingHistory();
//                x.Details = longString;
//                x.HasEvicted = true;
//                x.HousingSituationId = 1;
//            //    x.YearsLivingInMonths = 999;
//            //    x.RentPaid = 32;
//                x.IsDeleted = true;
//                housingHistories.Add(x);
//            }

//            ia.HousingSection.HousingHistories = housingHistories;
//            ia.HousingSection.CurrentHousingDetails = longString;
//          //  ia.HousingSection.HasEvictionRisk = false;
//            //ia.HousingSection.HousingAssistanceDetails = longString;
//            //ia.HousingSection.HousingSituationId = 1;
//            //ia.HousingSection.RentPaid = 12;
//            //ia.HousingSection.CurrentHousingMonths = 13;



//            //ia.HousingSection.HasUtilityRiskDisconnection = false;
//            //ia.HousingSection.UtilityRiskDisconnectionNotes = longString;


//            //ia.HousingSection.HasParticipationDifficulties = false;
//            //ia.HousingSection.ParticiapantDifficultNotes = longString;


//            ia.HousingSection.Notes = longString;


//            // Radio buttons
//            //ia.HousingSection.HasRefertoCommunityProgramOrShelter = false;
//            //ia.HousingSection.HasReferToOtherAgency = false;
//            //ia.HousingSection.HasReferToSubsidizedHousing = false;
//            //ia.HousingSection.HasEnergyAssistance = false;
//            //ia.HousingSection.HasEmergencyAssistance = false;
//            //ia.HousingSection.HasEmergencyPayment = false;
//            //ia.HousingSection.HasJobAccessLoan = false;
//            //ia.HousingSection.HasReferToOtherAgency = false;

//            //ia.HousingSection.ReferToOtherAgencyNotes = longString;

//            Db.SaveChanges();
//        }

//        public static void IaWithMilitarySectionHavingMilitaryTrainingData(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;
//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            p.InformalAssessments.Add(ia);
//            Db.Participants.Add(p);
//            Db.SaveChanges();
//            MilitaryBranchLookupDataSeed(Db);
//            MilitaryRankLookupDataSeed(Db);
//            MilitaryDischargeLookupDataSeed(Db);
//            MilitarySectionSeed(Db, ia);

//            string militaryLongString = null;

//            for (int i = 0; i < 1000; i++)
//            {
//                militaryLongString += "x";

//            }
//            ia.MilitaryTrainingSection.DoesHaveTraining = true;
//            ia.MilitaryTrainingSection.MilitaryBranchId = 1;
//            ia.MilitaryTrainingSection.MilitaryRankId = 1;
//            ia.MilitaryTrainingSection.Rate = "50";
//            ia.MilitaryTrainingSection.YearsEnlisted = 15;
//            ia.MilitaryTrainingSection.MilitaryDischargeTypeId = 1;
//            ia.MilitaryTrainingSection.SkillsAndTraining = militaryLongString;
//            ia.MilitaryTrainingSection.Notes = militaryLongString;
//            Db.SaveChanges();

//        }

//        public static void IaWithMilitarySectionHavingNoMilitaryTrainingData(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;
//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            p.InformalAssessments.Add(ia);
//            Db.Participants.Add(p);
//            Db.SaveChanges();
//            MilitaryBranchLookupDataSeed(Db);
//            MilitaryRankLookupDataSeed(Db);
//            MilitaryDischargeLookupDataSeed(Db);
//            MilitarySectionSeed(Db, ia);
//            string militaryLongString = null;

//            for (int i = 0; i < 1000; i++)
//            {
//                militaryLongString += "x";

//            }
//            ia.MilitaryTrainingSection.DoesHaveTraining = false;
//            ia.MilitaryTrainingSection.MilitaryBranchId = null;
//            ia.MilitaryTrainingSection.MilitaryRankId = null;
//            ia.MilitaryTrainingSection.Rate = null;
//            ia.MilitaryTrainingSection.YearsEnlisted = null;
//            ia.MilitaryTrainingSection.MilitaryDischargeTypeId = null;
//            ia.MilitaryTrainingSection.SkillsAndTraining = null;
//            ia.MilitaryTrainingSection.Notes = militaryLongString;
//            Db.SaveChanges();

//        }

//    //    var mockRepo = new Mock<IRepository>();
//    //    mockRepo.Setup(x => x.CwwCurrentChildren("300", "123")).Returns(new List<ICurrentChild>()
//    //        {
//    //           new CurrentChild() { LastName = "sohi"},
//    //           new CurrentChild() { FirstName = "Reddy"},
//    //           new CurrentChild() { Relationship = "brother"}

//    //});
            
//        public static void IaWithLanguagesSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var mockRepo = new Mock<IRepository>();
//            mockRepo.Setup(x => x.CwwCurrentChildren("1234")).Returns(new List<ICurrentChild>()
//            {
//               new CurrentChild() {LastName = "sohi"},
//               new CurrentChild() {FirstName = "Reddy"},
//               new CurrentChild() {Relationship = "brother"}

//            });

//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);

//            LanguagesLookupDataSeed(Db);
//            LanguageSectionSeed(Db, ia);

//            Db.Participants.Add(p);
//            Db.SaveChanges();

//        }


//        #region LegalIssuesSection


//        public static void IaWithLegalSectionWithUnknowns(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);

//            ConvictionTypeLookupDataSeed(Db);
//            ActionNeededLookupDataSeed(Db);
//            LegalSectionSeed(Db, ia);

//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            // Not including minor traffic violations, have you ever been convicted of a crime?
//            ia.LegalIssuesSection.IsConvictedOfCrime = true;
//            var convictions = new List<Conviction>();

//            for (int i = 0; i < 10; i++)
//            {
//                var conviction = new Conviction();
//                conviction.ConvictionTypeID = 1;
//                conviction.Details = "Details";
//                conviction.IsUnknown = true;
//                conviction.IsDeleted = false;
//                convictions.Add(conviction);
//            }
//            ia.LegalIssuesSection.Convictions = convictions;


//            // Are you currently under community supervision?
//            ia.LegalIssuesSection.IsUnderCommunitySupervision = true;
//            ia.LegalIssuesSection.CommunitySupervisonDetails = "CommunitySupervisonDetails";


//            // Do you have any pending charges?
//            ia.LegalIssuesSection.HasPendingCharges = true;
//            var pendingCharges = new List<PendingCharge>();

//            for (int i = 0; i < 10; i++)
//            {
//                var pendingCharge = new PendingCharge();
//                pendingCharge.ConvictionTypeID = 1;
//                pendingCharge.Details = "Details";
//                pendingCharge.IsUnknown = true;
//                pendingCharge.IsDeleted = false;
//                pendingCharges.Add(pendingCharge);
//            }
//            ia.LegalIssuesSection.PendingCharges = pendingCharges;

//            // Do you have any immediate family members with legal issues?
//            ia.LegalIssuesSection.HasFamilyLegalIssues = true;
//            ia.LegalIssuesSection.FamilyLegalIssueNotes = "FamilyLegalIssueNotes";


//            // Do you currently have a child welfare worker?
//            ia.LegalIssuesSection.HasChildWelfareWorker = true;
//            ia.LegalIssuesSection.ChildWelfareNotes = "ChildWelfareNotes";

//            // Are you currently ordered to pay child support?
//            ia.LegalIssuesSection.OrderedToPayChildSupport = true;
//            ia.LegalIssuesSection.OweAnyChildSupportBack = true;
//            ia.LegalIssuesSection.MonthlyAmount = 23m;
//            ia.LegalIssuesSection.ChildSupportDetails = "ChildSupportDetails";

//            // Have you been ordered to appear for any upcoming court dates?
//            ia.LegalIssuesSection.HasCourtDates = true;

//            var courtDates = new List<CourtDate>();

//            for (int i = 0; i < 10; i++)
//            {
//                var courtDate = new CourtDate();
//                courtDate.Location = "Location";
//                courtDate.Details = "Details";
//                courtDate.IsUnknown = true;
//                courtDate.IsDeleted = false;
//                courtDates.Add(courtDate);
//            }
//            ia.LegalIssuesSection.CourtDates = courtDates;


//            // ACTION NEEDED
//            ia.LegalIssuesSection.ActionNeededDetails = "ActionNeededDetails";

//            var legalIssuesActionBridges = new List<LegalIssuesActionBridge>();

//            for (int i = 0; i < 5; i++)
//            {
//                var legalIssuesActionBridge = new LegalIssuesActionBridge();
//                legalIssuesActionBridge.ActionNeededId = 1;
//                legalIssuesActionBridge.IsDeleted = false;
//                legalIssuesActionBridges.Add(legalIssuesActionBridge);
//            }

//            ia.LegalIssuesSection.LegalIssuesActionBridges = legalIssuesActionBridges;
//            ia.LegalIssuesSection.Notes = "Notes";

//            Db.SaveChanges();

//        }

//        public static void IaWithLegalSection(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);

//            ConvictionTypeLookupDataSeed(Db);
//            ActionNeededLookupDataSeed(Db);
//            LegalSectionSeed(Db, ia);

//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            // Not including minor traffic violations, have you ever been convicted of a crime?
//            ia.LegalIssuesSection.IsConvictedOfCrime = true;
//            var convictions = new List<Conviction>();

//            for (int i = 0; i < 10; i++)
//            {
//                var conviction = new Conviction();
//                conviction.ConvictionTypeID = 1;
//                conviction.Details = "Details";
//                conviction.DateConvicted = DateTime.Now;
//                conviction.IsDeleted = false;
//                convictions.Add(conviction);
//            }
//            ia.LegalIssuesSection.Convictions = convictions;


//            // Are you currently under community supervision?
//            ia.LegalIssuesSection.IsUnderCommunitySupervision = true;
//            ia.LegalIssuesSection.CommunitySupervisonDetails = "CommunitySupervisonDetails";


//            // Do you have any pending charges?
//            ia.LegalIssuesSection.HasPendingCharges = true;
//            var pendingCharges = new List<PendingCharge>();

//            for (int i = 0; i < 10; i++)
//            {
//                var pendingCharge = new PendingCharge();
//                pendingCharge.ConvictionTypeID = 1;
//                pendingCharge.Details = "Details";
//                pendingCharge.ChargeDate = DateTime.Now;
//                pendingCharge.IsDeleted = false;
//                pendingCharges.Add(pendingCharge);
//            }
//            ia.LegalIssuesSection.PendingCharges = pendingCharges;

//            // Do you have any immediate family members with legal issues?
//            ia.LegalIssuesSection.HasFamilyLegalIssues = true;
//            ia.LegalIssuesSection.FamilyLegalIssueNotes = "FamilyLegalIssueNotes";


//            // Do you currently have a child welfare worker?
//            ia.LegalIssuesSection.HasChildWelfareWorker = true;
//            ia.LegalIssuesSection.ChildWelfareNotes = "ChildWelfareNotes";

//            // Are you currently ordered to pay child support?
//            ia.LegalIssuesSection.OrderedToPayChildSupport = true;
//            ia.LegalIssuesSection.OweAnyChildSupportBack = true;
//            ia.LegalIssuesSection.MonthlyAmount = 23m;
//            ia.LegalIssuesSection.ChildSupportDetails = "ChildSupportDetails";

//            // Have you been ordered to appear for any upcoming court dates?
//            ia.LegalIssuesSection.HasCourtDates = true;

//            var courtDates = new List<CourtDate>();

//            for (int i = 0; i < 10; i++)
//            {
//                var courtDate = new CourtDate();
//                courtDate.Location = "Location";
//                courtDate.Details = "Details";
//                courtDate.Date = DateTime.Now;
//                courtDate.IsDeleted = false;
//                courtDates.Add(courtDate);
//            }
//            ia.LegalIssuesSection.CourtDates = courtDates;


//            // ACTION NEEDED
//            ia.LegalIssuesSection.ActionNeededDetails = "ActionNeededDetails";

//            var legalIssuesActionBridges = new List<LegalIssuesActionBridge>();

//            for (int i = 0; i < 5; i++)
//            {
//                var legalIssuesActionBridge = new LegalIssuesActionBridge();
//                legalIssuesActionBridge.ActionNeededId = 1;
//                legalIssuesActionBridge.IsDeleted = false;
//                legalIssuesActionBridges.Add(legalIssuesActionBridge);
//            }

//            ia.LegalIssuesSection.LegalIssuesActionBridges = legalIssuesActionBridges;
//            ia.LegalIssuesSection.Notes = "Notes";

//            Db.SaveChanges();

//        }

//        #endregion
//        public static void IaWithWorkProgramsSection(Wwp.Data.Sql.Model.WwpEntities Db, bool restoreData = false)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);

//            var wps = WorkProgramsSectionSeed(Db, ia);


//            if (restoreData == true)
//            {
//                var iwp = new InvolvedWorkProgram();
//                iwp.Details = "Test";
//                iwp.IsDeleted = true;
//                wps.InvolvedWorkPrograms.Add(iwp);
//                //Db.InvolvedWorkPrograms.Add(iwp);
//            }

//            Db.Participants.Add(p);
//            Db.SaveChanges();

//        }

//        public static void IaWithNoSections(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            Db.Participants.Add(p);
//            Db.SaveChanges();
//            LanguagesLookupDataSeed(Db);
//            MilitaryBranchLookupDataSeed(Db);
//            MilitaryRankLookupDataSeed(Db);
//            MilitaryDischargeLookupDataSeed(Db);
//            CareArrangmentLookupDataSeed(Db);
//            ConvictionTypeLookupDataSeed(Db);
//            HousingSituationLookupDataSeed(Db);
//            EmploymentSatusTypeLookupDataSeed(Db);
//            ActionNeededLookupDataSeed(Db);
//            ActionNeededLookupDataSeedForWorkHistorySection(Db);
//            Db.SaveChanges();


//        }


//        public static void IaWithLanguageSection_WithKnownLanguages_Data(Wwp.Data.Sql.Model.WwpEntities Db)
//        {

//            var la = new Data.Sql.Model.LanguageSection();
//            la.ModifiedBy = "Ram";
//            Db.LanguageSections.Add(la);
//            Db.SaveChanges();

//            byte[] v1 = new byte[2] { 1, 3 };


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
//                    if (ix == 39)
//                    {
//                        ixx[ix].Name = "Spanish";
//                    }
//                    if (ix == 38)
//                    {
//                        ixx[ix].Name = "Latin";
//                    }
//                    if (ix == 37)
//                    {
//                        ixx[ix].Name = "Spanish";
//                    }
//                }

//                Db.Languages.Add(ixx[ix]);
//                Db.SaveChanges();
//            }
//            //int ix = 0;
//            //ixx[ix] = new Data.Sql.Model.Language();
//            //foreach (int ixx[] in ixx[ix] )


//            for (int i = 1; i < 3; i++)
//            {
//                var kl = new Data.Sql.Model.KnownLanguage();
//                kl.LanguageSection = la;
//                kl.LanguageId = i;
//                if (i == 1)
//                {
//                    kl.IsAbleToRead = true;
//                    kl.IsAbleToSpeak = false;
//                    kl.IsAbleToWrite = true;
//                    kl.IsPrimary = false;

//                }
//                if (i == 2)
//                {
//                    kl.IsAbleToRead = true;
//                    kl.IsAbleToSpeak = true;
//                    kl.IsAbleToWrite = true;
//                    kl.IsPrimary = false;
//                }
//                Db.KnownLanguages.Add(kl);
//                Db.SaveChanges();
//            }



//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            ia.LanguageSection = la;
//            ia.ModifiedDate = DateTime.Now;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();

//            var part = (from x in Db.Participants where x.PinNumber == 123m select x).FirstOrDefault();
//            var part1 = (from x in Db.InformalAssessments where x.ParticipantId == 1 select x).FirstOrDefault();
//        }

//        public static void IaWithLanguageSectionAndEducationSection_WithEnglishAsKnownLanguages(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var la = new Data.Sql.Model.LanguageSection();
//            la.ModifiedBy = "Ram";
//            Db.LanguageSections.Add(la);
//            Db.SaveChanges();

//            byte[] v1 = new byte[2] { 1, 3 };

//            var co = new Country();
//            co.ModifiedBy = "Dinu";
//            co.ModifiedDate = DateTime.Now;
//            co.Name = "United States";
//            Db.Countries.Add(co);

//            var st = new Data.Sql.Model.State();
//            st.ModifiedBy = "Sohi";
//            st.ModifiedDate = DateTime.Now;
//            st.Code = "WI";
//            st.Country = co;
//            Db.States.Add(st);

//            var s = new Data.Sql.Model.SchoolCollegeEstablishment();
//            s.ModifiedBy = "Sohi";
//            s.ModifiedDate = DateTime.Now;
//            //s.State = st;
//            s.Street = "123";
//            s.Name = "West";
//            //s.City = "MD";
//            Db.SchoolCollegeEstablishments.Add(s);

//            var cia = new CertificateIssuingAuthority();
//            cia.ModifiedBy = "Sohi";
//            cia.ModifiedDate = DateTime.Now;
//            cia.Code = "iA";
//            cia.SortOrder = 1;
//            Db.CertificateIssuingAuthorities.Add(cia);

//            var c1 = new Data.Sql.Model.SchoolGraduationStatus();
//            c1.ModifiedBy = "Sohi";
//            c1.ModifiedDate = DateTime.Now;
//            Db.SchoolGraduationStatus.Add(c1);

//            var c2 = new Data.Sql.Model.SchoolGradeLevel();
//            c2.ModifiedBy = "Sohi";
//            c2.SortOrder = 1;
//            c2.ModifiedDate = DateTime.Now;
//            c2.Grade = 1;
//            Db.SchoolGradeLevels.Add(c2);

//            var ea = new Data.Sql.Model.EducationSection();
//            ea.ModifiedBy = "Sohi";
//            ea.ModifiedDate = DateTime.Now;
//            ea.SchoolCollegeEstablishment = s;
//            ea.SchoolGraduationStatus = c1;
//            ea.SchoolGradeLevel = c2;
//            ea.CertificateIssuingAuthority = cia;
//            Db.EducationSections.Add(ea);
//            Db.SaveChanges();

//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "cheedu";
//            p.PinNumber = 123;
//            Db.Participants.Add(p);
//            Db.SaveChanges();


//            Data.Sql.Model.Language[] ixx = new Data.Sql.Model.Language[45];

//            for (int ix = 1; ix < 41; ix++)
//            {
//                ixx[ix] = new Data.Sql.Model.Language();
//                {
                    
//                    if (ix == 7)
//                    {
//                        ixx[ix].Name = "English";
//                    }
//                    if (ix == 39)
//                    {
//                        ixx[ix].Name = "Spanish";
//                    }
//                    if (ix == 38)
//                    {
//                        ixx[ix].Name = "Latin";
//                    }
//                    if (ix == 37)
//                    {
//                        ixx[ix].Name = "Spanish";
//                    }
//                }

//                Db.Languages.Add(ixx[ix]);
//                Db.SaveChanges();
//            }
//            //int ix = 0;
//            //ixx[ix] = new Data.Sql.Model.Language();
//            //foreach (int ixx[] in ixx[ix] )

//            var kl = new Data.Sql.Model.KnownLanguage();
//            kl.LanguageSection = la;
//            //kl.Language = ixx;
//            kl.ModifiedDate = DateTime.Now;
//            kl.IsPrimary = true;
//            kl.LanguageId = 7;          
//            kl.IsAbleToRead = true;
//            kl.IsAbleToSpeak = true;
//            kl.IsAbleToWrite = false;
//            Db.KnownLanguages.Add(kl);
//            Db.SaveChanges();

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            ia.LanguageSection = la;

//            ia.EducationSection = ea;
//            ia.ModifiedDate = DateTime.Now;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();

//            var part = (from x in Db.Participants where x.PinNumber == 123m select x).FirstOrDefault();
//            var part1 = (from x in Db.InformalAssessments where x.ParticipantId == 1 select x).FirstOrDefault();
//        }

//        public static void IaWithWorkhistorySection(Dcf.Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);
//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            ActionNeededLookupDataSeedForWorkHistorySection(Db);
//            EmploymentSatusTypeLookupDataSeed(Db);
//            WorkHistorySectionSeed(Db, ia);
//            ia.WorkHistorySection.EmploymentStatusTypeId = 2;
//            //Actionneeded
//            var workhistoryactionbridges = new List<EmploymentWorkHistoryBridge>();
//            for (int i = 0; i <= 9; i++)
//            {
//                var workhistoryactionbridge = new EmploymentWorkHistoryBridge();
//                workhistoryactionbridge.ActionNeededId = 1;
//                workhistoryactionbridge.IsDeleted = true;
//                workhistoryactionbridges.Add(workhistoryactionbridge);
//            }
//            for (int i = 10; i <= 19; i++)
//            {
//                var workhistoryactionbridge = new EmploymentWorkHistoryBridge();
//                workhistoryactionbridge.ActionNeededId = 1;
//                workhistoryactionbridge.IsDeleted = false;
//                workhistoryactionbridges.Add(workhistoryactionbridge);
//            }
//            ia.WorkHistorySection.EmploymentWorkHistoryBridges = workhistoryactionbridges;
//            ia.WorkHistorySection.Details = "details";
//            //Has performed volunteered work
//            ia.WorkHistorySection.HasVolunteered = true;
//            Db.SaveChanges();
//        }

//        public static void IaWithWorkhistorySectionWithFulltimeEmploymentStatus(Dcf.Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);
//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            ActionNeededLookupDataSeedForWorkHistorySection(Db);
//            EmploymentSatusTypeLookupDataSeed(Db);
//            WorkHistorySectionSeed(Db, ia);
//            ia.WorkHistorySection.EmploymentStatusTypeId = 1;
//            //Actionneeded            
//            Db.SaveChanges();
//        }

//        public static void IaWithWorkhistorySection_WithUnemploymentStatus(Dcf.Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;

//            p.InformalAssessments.Add(ia);
//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            ActionNeededLookupDataSeedForWorkHistorySection(Db);
//            EmploymentSatusTypeLookupDataSeed(Db);
//            WorkHistorySectionSeed(Db, ia);
//            ia.WorkHistorySection.EmploymentStatusTypeId = 3;
//            //Actionneeded
//            var workhistoryactionbridges = new List<EmploymentWorkHistoryBridge>();
//            for (int i = 0; i <= 9; i++)
//            {
//                var workhistoryactionbridge = new EmploymentWorkHistoryBridge();
//                workhistoryactionbridge.ActionNeededId = 1;
//                workhistoryactionbridge.IsDeleted = true;
//                workhistoryactionbridges.Add(workhistoryactionbridge);
//            }
//            for (int i = 10; i <= 19; i++)
//            {
//                var workhistoryactionbridge = new EmploymentWorkHistoryBridge();
//                workhistoryactionbridge.ActionNeededId = 1;
//                workhistoryactionbridge.IsDeleted = false;
//                workhistoryactionbridges.Add(workhistoryactionbridge);
//            }
//            ia.WorkHistorySection.EmploymentWorkHistoryBridges = workhistoryactionbridges;
//            ia.WorkHistorySection.Details = "details";
//            ia.WorkHistorySection.HasVolunteered = true;
//            //Has performed volunteered work
//            ia.WorkHistorySection.HasVolunteered = true;
//            Db.SaveChanges();
//        }

//        public static void ParticipantWithSingleExamScoreAndHavingMultipleSujects(Dcf.Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            ExamTypeSeed(Db);
           
//            NrsTypeSeed(Db);
//            SplTypeSeed(Db);
//            ExamSubjectTypeSeed(Db);
//            ExamPassTypeSeed(Db);
//            Db.SaveChanges();
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var edx = new Data.Sql.Model.EducationExam();
//            edx.Participant = p;
//            edx.ParticipantId = 1;
//            //var version = new byte[2] { 1, 2 };
//            //edx.RowVersion = version;

//            edx.DateTaken = DateTime.Now;
//            edx.ExamTypeId = 1;
//            edx.IsDeleted = false;
//            var examResults = new List<ExamResult>();
//            for (int i = 1; i <= 10; i++)
//            {
//                var ixr = new ExamResult();
//                ixr.Score = 45;
//                ixr.ExamLevelType = 1;
//                ixr.NRSTypeId = 3;
//                ixr.SPLTypeId = 4;
//                ixr.ExamSubjectTypeId = 5;
//                ixr.ExamPassTypeId = 1;
//                examResults.Add(ixr);
//            }
//            edx.ExamResults = examResults;
//            p.EducationExams.Add(edx);
//            Db.Participants.Add(p);
//            Db.SaveChanges();

//        }


//        public static void ParticipantWithMultipleExamScoresAndHavingMultipleSubjects(Dcf.Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            ExamTypeSeed(Db);          
//            NrsTypeSeed(Db);
//            SplTypeSeed(Db);
//            ExamSubjectTypeSeed(Db);
//            ExamPassTypeSeed(Db);
//            Db.SaveChanges();
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;

//            var educationExams = new List<EducationExam>();
//            for (int i = 1; i <= 10; i++)
//            {
//                var edx = new EducationExam();
//                edx.Participant = p;
//                edx.ParticipantId = 1;
//                edx.DateTaken = DateTime.Now;
//                edx.ExamTypeId = 1;
//                edx.IsDeleted = false;

//                var examResults = new List<ExamResult>();
//                for (int j = 1; j <= 10; j++)
//                {
//                    var ixr = new ExamResult();
//                    ixr.Score = 45;
//                    ixr.ExamLevelType = 1;
//                    ixr.NRSTypeId = 3;                   
//                    ixr.SPLTypeId = 4;
//                    ixr.ExamSubjectTypeId = 5;
//                    ixr.ExamPassTypeId = 1;
//                    examResults.Add(ixr);
//                }
//                edx.ExamResults = examResults;
//                educationExams.Add(edx);               
//            }
//            p.EducationExams = educationExams;
//            Db.Participants.Add(p);
//            Db.SaveChanges();
//        }

//        public static void ParticipantWithNoExamScores(Dcf.Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            ExamTypeSeed(Db);          
//            NrsTypeSeed(Db);
//            SplTypeSeed(Db);
//            ExamSubjectTypeSeed(Db);
//            ExamPassTypeSeed(Db);
//            Db.SaveChanges();
//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "Ashan";
//            p.PinNumber = 123;
        
//            Db.Participants.Add(p);
//            Db.SaveChanges();
//        }
//    }
//}


