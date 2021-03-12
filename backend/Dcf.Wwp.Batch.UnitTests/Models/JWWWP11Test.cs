using System;
using System.Data;
using Dcf.Wwp.Batch.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.Batch.UnitTests.Models
{
    [TestClass]
    public class JWWWP11Test
    {
        #region Properties

        private readonly DataTable _participationPeriodDetails = new DataTable();
        private readonly JWWWP11   _jwwwp11;

        #endregion

        #region Methods

        public JWWWP11Test()
        {
            _participationPeriodDetails.Columns.AddRange(new[]
                                                         {
                                                             new DataColumn("CaseNumber",                   typeof(decimal)),
                                                             new DataColumn("PinNumber",                    typeof(decimal)),
                                                             new DataColumn("ParticipantId",                typeof(int)),
                                                             new DataColumn("ParticipationBeginDate",       typeof(DateTime)),
                                                             new DataColumn("ParticipationEndDate",         typeof(DateTime)),
                                                             new DataColumn("PreviousUnAppliedHours",       typeof(decimal)),
                                                             new DataColumn("CurrentUnAppliedHours",        typeof(decimal)),
                                                             new DataColumn("PreviousNonParticipatedHours", typeof(decimal)),
                                                             new DataColumn("NonParticipatedHours",         typeof(decimal)),
                                                             new DataColumn("PreviousGoodCausedHours",      typeof(decimal)),
                                                             new DataColumn("GoodCausedHours",              typeof(decimal)),
                                                             new DataColumn("AppliedHours",                 typeof(decimal)),
                                                             new DataColumn("BaseW2Payment",                typeof(decimal)),
                                                             new DataColumn("NewBaseW2Payment",             typeof(decimal)),
                                                             new DataColumn("DFPenaltyPct",                 typeof(decimal)),
                                                             new DataColumn("DrugFelonPenalty",             typeof(decimal)),
                                                             new DataColumn("Recoupment",                   typeof(decimal)),
                                                             new DataColumn("LearnFarePenalty",             typeof(decimal)),
                                                             new DataColumn("LFPenalty",                    typeof(decimal)),
                                                             new DataColumn("NonParticipationReduction",    typeof(decimal)),
                                                             new DataColumn("FinalPayment",                 typeof(decimal)),
                                                             new DataColumn("VendorPayment",                typeof(decimal)),
                                                             new DataColumn("OverPaymentDollar",            typeof(decimal))
                                                         });
            _jwwwp11 = new JWWWP11(null, null);
        }

        private void SetUpBaseDataForTest()
        {
            _participationPeriodDetails.Clear();
            _participationPeriodDetails.Rows.Add();
            _participationPeriodDetails.Rows[0]["CaseNumber"]             = 1003990916;
            _participationPeriodDetails.Rows[0]["PinNumber"]              = 1009969382;
            _participationPeriodDetails.Rows[0]["ParticipantId"]          = 13660;
            _participationPeriodDetails.Rows[0]["ParticipationBeginDate"] = DateTime.Parse("2020 -06-16");
            _participationPeriodDetails.Rows[0]["ParticipationEndDate"]   = DateTime.Parse("2020 -07-15");
            _participationPeriodDetails.Rows[0]["OverPaymentDollar"]      = 5.00m;
        }

        [TestMethod]
        public void TestCalculateOverPaymentWithNoNonParticipationForPaidToUnPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 0.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 610.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 50.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 603.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(0.0m,   result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 0.");
            Assert.AreEqual(0.0m,   result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(43.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be 43.");
        }

        [TestMethod]
        public void TestCalculateOverPaymentWithPenaltiesNoNonParticipationForPaidToUnPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 0.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 610.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 65.00m;
            _participationPeriodDetails.Rows[0]["DFPenaltyPct"]              = 0.10m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 50.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 538.00;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(0.0m,   result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 0.");
            Assert.AreEqual(0.0m,   result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(39.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be 39.");
        }

        [TestMethod]
        public void TestCalculateOverPaymentWithNoPenaltiesAndNonParticipationForPaidToUnPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 140.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 610.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 0.00;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(122.0m, result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 122.");
            Assert.AreEqual(18.0m,  result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 18.");
            Assert.AreEqual(0.00m,  result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be 0.");
        }

        [TestMethod]
        public void TestCalculateOverPaymentWithNoPenaltiesAndAdjustedNonParticipationForPaidToUnPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 5.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 610.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 653.00;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(5.0m,   result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 5.");
            Assert.AreEqual(0.0m,   result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(68.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be 68.");
        }

        [TestMethod]
        public void TestCalculateUnderPaymentWithNoNonParticipationForUnPaidToPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 0.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 0.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 42.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 0.00;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(0.0m,    result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 0.");
            Assert.AreEqual(0.0m,    result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(-42.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -42.");
        }

        [TestMethod]
        public void TestCalculateUnderPaymentWithPenaltiesAdjustedNonParticipationForUnPaidToPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 5.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 0.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 42.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 0.00;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(5.0m,    result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 0.");
            Assert.AreEqual(0.0m,    result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(-17.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -17.");
        }

        [TestMethod]
        public void TestCalculateUnderPaymentWithUnAppliedHoursForUnPaidToPaid()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 149.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 610.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 0.00;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(122.0m, result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 0.");
            Assert.AreEqual(27.0m,  result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 27.");
            Assert.AreEqual(0.00m,  result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be 0.");
        }

        [TestMethod]
        public void TestCalculateUnderPaymentWithUnAppliedPart5_C1()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 0.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 653.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 0.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 0.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(0.0m,     result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 0.");
            Assert.AreEqual(0.0m,     result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(-653.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -653.");
        }


        [TestMethod]
        public void TestCalculateUnderPaymentPlacementAndGCPart5_C3()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 5.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 653.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 610.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 25.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 0.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(5.0m,     result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 5.");
            Assert.AreEqual(0.0m,     result.Rows[0]["CalculatedUnAppliedHours"], "Expecting NewCalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(-585.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmt to be -585.");
        }


        [TestMethod]
        public void TestCalculateOverpaymentWithPenalties()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 70.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 608.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 608.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 251.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 350.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 57.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(70.0m,  result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 70.");
            Assert.AreEqual(0.0m,   result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(50.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -50.");
        }


        [TestMethod]
        public void TestCalculateOverpaymentWithPenaltiesIntoUnApplied()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 80.0m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 608.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 608.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 251.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 357.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 57.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(72.0m,  result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 72.");
            Assert.AreEqual(8.0m,   result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 8.");
            Assert.AreEqual(57.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -57.");
        }

        [TestMethod]
        public void TestCalculateUnderpaymentWithDecimalGoodCaused_1()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 3.0m;
            _participationPeriodDetails.Rows[0]["GoodCausedHours"]           = 0.5m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 326.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 565.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 10.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 316.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(2.0m,     result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 2.");
            Assert.AreEqual(0.0m,     result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(-239.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -239.");
        }

        [TestMethod]
        public void TestCalculateUnderpaymentWithDecimalGoodCaused_2()
        {
            //Arrange
            SetUpBaseDataForTest();

            _participationPeriodDetails.Rows[0]["NonParticipatedHours"]      = 4.0m;
            _participationPeriodDetails.Rows[0]["GoodCausedHours"]           = 2.5m;
            _participationPeriodDetails.Rows[0]["BaseW2Payment"]             = 130.00m;
            _participationPeriodDetails.Rows[0]["NewBaseW2Payment"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["DrugFelonPenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["Recoupment"]                = 0.00m;
            _participationPeriodDetails.Rows[0]["LearnFarePenalty"]          = 0.00m;
            _participationPeriodDetails.Rows[0]["NonParticipationReduction"] = 20.00m;
            _participationPeriodDetails.Rows[0]["FinalPayment"]              = 110.00m;

            //Act
            var result = _jwwwp11.CalculateOverOrUnderPaymentBasedOnParticipation(_participationPeriodDetails);

            // Assert
            Assert.AreEqual(1.0m,    result.Rows[0]["CalculatedAppliedHours"],   "Expecting CalculatedAppliedHours to be 1.");
            Assert.AreEqual(0.0m,    result.Rows[0]["CalculatedUnAppliedHours"], "Expecting CalculatedUnAppliedHours to be 0.");
            Assert.AreEqual(-15.00m, result.Rows[0]["RevisedPaymentAmount"],     "Expecting RevisedPaymentAmount to be -15.");
        }

        #endregion
    }
}
