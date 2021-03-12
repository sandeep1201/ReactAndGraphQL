// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dcf.Wwp.Batch.Infrastructure;
using Dcf.Wwp.Batch.Interfaces;
using Fclp.Internals.Extensions;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP11 : IBatchJob
    {
        #region Properties

        private readonly IBaseJob               _baseJob;
        private readonly IOverUnderPaymentEmail _overUnderPaymentEmail;

        public string Name    => GetType().Name;
        public string Desc    => "After Pull Down Weekly Batch (calculate whether to issue over/under payment and issue accordingly)";
        public string Sproc   { get; private set; } = "wwp.USP_After_Pull_Down_Batch";
        public int    NumRows => 0;

        private List<ParticipantPaymentHistory> _participantPaymentHistories = new List<ParticipantPaymentHistory>();
        private List<OverUnderPayment>          _overUnderPayments           = new List<OverUnderPayment>();

        #endregion

        #region Methods

        public JWWWP11(IBaseJob baseJob, IOverUnderPaymentEmail overUnderPaymentEmail)
        {
            _baseJob               = baseJob;
            _overUnderPaymentEmail = overUnderPaymentEmail;
        }

        public DataTable Run()
        {
            _baseJob.RunSproc(Name, Sproc);

            var participationPeriodDetails = CalculateOverOrUnderPaymentBasedOnParticipation(_baseJob.DataTable);

            UpdateParticipationPeriodSummary(participationPeriodDetails);

            UpdateParticipantPaymentHistory();

            IssueOverOrUnderPayment(participationPeriodDetails);

            return participationPeriodDetails;
        }

        public DataTable CalculateOverOrUnderPaymentBasedOnParticipation(DataTable participationPeriodDetails)
        {
            if (_participantPaymentHistories == null) _participantPaymentHistories = new List<ParticipantPaymentHistory>();
            if ((_overUnderPayments)         == null) _overUnderPayments           = new List<OverUnderPayment>();

            participationPeriodDetails.Columns.Add("CalculatedAppliedHours",              typeof(decimal));
            participationPeriodDetails.Columns.Add("CalculatedUnAppliedHours",            typeof(decimal));
            participationPeriodDetails.Columns.Add("CalculatedNonParticipationReduction", typeof(decimal));
            participationPeriodDetails.Columns.Add("CalculatedFinalPayment",              typeof(decimal));
            participationPeriodDetails.Columns.Add("RevisedPaymentAmount",                typeof(decimal));

            var participationDetails = participationPeriodDetails.AsEnumerable()
                                                                 .ToLookup(i => new
                                                                                {
                                                                                    ParticipationBeginDate = i.Field<DateTime?>("ParticipationBeginDate"),
                                                                                    ParticipationEndDate   = i.Field<DateTime?>("ParticipationEndDate"),
                                                                                    CaseNumber             = i.Field<decimal?>("CaseNumber")
                                                                                });

            participationDetails.ForEach(group =>
                                         {
                                             var firstParticipant         = group.First();
                                             var overPaymentDollar        = group.First().Field<decimal?>("OverPaymentDollar").GetValueOrDefault();
                                             var newBaseW2Payment         = Math.Floor(group.Sum(i => i.Field<decimal?>("NewBaseW2Payment")).GetValueOrDefault());
                                             var origDrugFelonPenalty     = group.First().Field<decimal?>("DrugFelonPenalty").GetValueOrDefault();
                                             var dFPenaltyPct             = group.First().Field<decimal?>("DFPenaltyPct").GetValueOrDefault();
                                             var baseW2Payment            = newBaseW2Payment > 0 ? newBaseW2Payment : group.First().Field<decimal?>("BaseW2Payment").GetValueOrDefault();
                                             var recoupment               = group.First().Field<decimal?>("Recoupment").GetValueOrDefault();
                                             var originalLearnFarePenalty = group.First().Field<decimal?>("LearnFarePenalty").GetValueOrDefault();
                                             var lFPenalty                = group.First().Field<decimal?>("LFPenalty").GetValueOrDefault();
                                             var vendorPayment            = group.First().Field<decimal?>("VendorPayment").GetValueOrDefault();
                                             var finalPayment             = group.First().Field<decimal?>("FinalPayment").GetValueOrDefault();
                                             var newDrugFelonPenalty      = origDrugFelonPenalty;
                                             var newLearnFarePenalty      = originalLearnFarePenalty;

                                             if (newBaseW2Payment > 0)
                                             {
                                                 newDrugFelonPenalty = Math.Floor(decimal.Multiply(newBaseW2Payment, dFPenaltyPct));
                                                 newLearnFarePenalty = Math.Floor(lFPenalty);
                                             }

                                             var calculatedBaseW2Payment         = new List<decimal?> { baseW2Payment, newDrugFelonPenalty, recoupment, newLearnFarePenalty }.SubtractDecimal();
                                             var calculatedNonParticipationHours = group.Sum(i => i.Field<decimal?>("NonParticipatedHours").GetValueOrDefault());
                                             var calculatedGoodCausedHours       = group.Sum(i => i.Field<decimal?>("GoodCausedHours").GetValueOrDefault());
                                             var calculatedSanctionableHours     = Math.Floor(decimal.Subtract(calculatedNonParticipationHours, calculatedGoodCausedHours));

                                             var calculatedDockingAmount = decimal.Multiply(calculatedSanctionableHours, overPaymentDollar);

                                             decimal calculatedNonParticipationReduction;

                                             if (calculatedBaseW2Payment >= calculatedDockingAmount)
                                             {
                                                 calculatedNonParticipationReduction = calculatedDockingAmount;

                                                 group.ForEach(i =>
                                                               {
                                                                   var nonParticipatedHours             = i.Field<decimal?>("NonParticipatedHours").GetValueOrDefault();
                                                                   var goodCausedHours                  = i.Field<decimal?>("GoodCausedHours").GetValueOrDefault();
                                                                   var nonParticipatedSanctionableHours = Math.Floor(decimal.Subtract(nonParticipatedHours, goodCausedHours));

                                                                   i["CalculatedAppliedHours"]   = nonParticipatedSanctionableHours;
                                                                   i["CalculatedUnAppliedHours"] = decimal.Subtract(nonParticipatedSanctionableHours, i.Field<decimal>("CalculatedAppliedHours"));
                                                               });
                                             }
                                             else
                                             {
                                                 var calculatedNonParticipationHoursRequired = Math.Ceiling(decimal.Divide(calculatedBaseW2Payment, overPaymentDollar));
                                                 calculatedNonParticipationReduction = calculatedBaseW2Payment;

                                                 group.ForEach(i =>
                                                               {
                                                                   var nonParticipatedHours             = i.Field<decimal?>("NonParticipatedHours").GetValueOrDefault();
                                                                   var goodCausedHours                  = i.Field<decimal?>("GoodCausedHours").GetValueOrDefault();
                                                                   var nonParticipatedSanctionableHours = Math.Floor(decimal.Subtract(nonParticipatedHours, goodCausedHours));

                                                                   if (calculatedNonParticipationHoursRequired >= nonParticipatedSanctionableHours)
                                                                       i["CalculatedAppliedHours"] = nonParticipatedSanctionableHours;
                                                                   else
                                                                       i["CalculatedAppliedHours"] = calculatedNonParticipationHoursRequired;

                                                                   i["CalculatedUnAppliedHours"] = decimal.Subtract(nonParticipatedSanctionableHours, i.Field<decimal>("CalculatedAppliedHours"));

                                                                   calculatedNonParticipationHoursRequired -= nonParticipatedSanctionableHours;
                                                                   calculatedNonParticipationHoursRequired = calculatedNonParticipationHoursRequired <= 0
                                                                                                                 ? 0
                                                                                                                 : calculatedNonParticipationHoursRequired;
                                                               });
                                             }

                                             var calculatedFinalPayment = decimal.Subtract(calculatedBaseW2Payment, calculatedNonParticipationReduction);
                                             var revisedPaymentAmount   = decimal.Subtract(finalPayment,            calculatedFinalPayment);

                                             //If revisedPaymentAmount is positive then Overpayment else Aux
                                             group.ForEach(i => i["CalculatedNonParticipationReduction"] = calculatedNonParticipationReduction);
                                             group.ForEach(i => i["CalculatedFinalPayment"]              = calculatedFinalPayment);
                                             group.ForEach(i => i["RevisedPaymentAmount"]                = revisedPaymentAmount);

                                             var participantPaymentHistory = new ParticipantPaymentHistory
                                                                             {
                                                                                 CaseNumber                = group.Key.CaseNumber.GetValueOrDefault(),
                                                                                 ParticipationBeginDate    = group.Key.ParticipationBeginDate.GetValueOrDefault(),
                                                                                 ParticipationEndDate      = group.Key.ParticipationEndDate.GetValueOrDefault(),
                                                                                 BaseW2Payment             = baseW2Payment,
                                                                                 DrugFelonPenalty          = newDrugFelonPenalty,
                                                                                 Recoupment                = recoupment,
                                                                                 LearnFarePenalty          = newLearnFarePenalty,
                                                                                 NonParticipationReduction = calculatedNonParticipationReduction,
                                                                                 VendorPayment             = vendorPayment,
                                                                                 CalculatedFinalPayment    = calculatedFinalPayment
                                                                             };

                                             _participantPaymentHistories.Add(participantPaymentHistory);

                                             var overUnderPayment = new OverUnderPayment
                                                                    {
                                                                        ParticipantId          = firstParticipant.Field<int?>("ParticipantId").GetValueOrDefault(),
                                                                        PinNumber              = firstParticipant.Field<decimal?>("PinNumber").GetValueOrDefault(),
                                                                        CaseNumber             = firstParticipant.Field<decimal?>("CaseNumber").GetValueOrDefault(),
                                                                        ParticipationBeginDate = firstParticipant.Field<DateTime?>("ParticipationBeginDate").GetValueOrDefault(),
                                                                        ParticipationEndDate   = firstParticipant.Field<DateTime?>("ParticipationEndDate").GetValueOrDefault(),
                                                                        FinalPayment           = finalPayment,
                                                                        RevisedPaymentAmount   = revisedPaymentAmount
                                                                    };

                                             _overUnderPayments.Add(overUnderPayment);
                                         });

            return  participationPeriodDetails;
        }

        private void UpdateParticipationPeriodSummary(DataTable dataTable)
        {
            var ppsXml = new XElement("ParticipationPeriodSummaries",
                                      dataTable.AsEnumerable()
                                               .Select(i => new XElement("ParticipationPeriodSummary",
                                                                         new XElement("CaseNumber",             i.Field<decimal?>("CaseNumber")),
                                                                         new XElement("ParticipantId",          i.Field<int?>("ParticipantId")),
                                                                         new XElement("ParticipationBeginDate", i.Field<DateTime?>("ParticipationBeginDate")),
                                                                         new XElement("ParticipationEndDate",   i.Field<DateTime?>("ParticipationEndDate")),
                                                                         new XElement("AppliedHours",           i.Field<decimal?>("CalculatedAppliedHours")),
                                                                         new XElement("UnAppliedHours",         i.Field<decimal?>("CalculatedUnAppliedHours")))))
                .ToString();

            var parameters = new[]
                             {
                                 new SqlParameter
                                 {
                                     ParameterName = "@XML",
                                     Direction     = ParameterDirection.Input,
                                     Value         = ppsXml,
                                     DbType        = DbType.Xml
                                 }
                             };

            _baseJob.RunSproc(Name, Sproc, parameters);
        }

        private void UpdateParticipantPaymentHistory()
        {
            Sproc = "wwp.USP_Update_ParticipantPaymentHistory";

            var pphXml = new XElement("ParticipationPaymentHistories",
                                      _participantPaymentHistories
                                          .Where(i => i.BaseW2Payment > 0)
                                          .Select(i => new XElement("ParticipationPaymentHistory",
                                                                    new XElement("CaseNumber",                i.CaseNumber),
                                                                    new XElement("ParticipationBeginDate",    i.ParticipationBeginDate),
                                                                    new XElement("ParticipationEndDate",      i.ParticipationEndDate),
                                                                    new XElement("BaseW2Payment",             i.BaseW2Payment),
                                                                    new XElement("DrugFelonPenalty",          i.DrugFelonPenalty),
                                                                    new XElement("Recoupment",                i.Recoupment),
                                                                    new XElement("LearnFarePenalty",          i.LearnFarePenalty),
                                                                    new XElement("NonParticipationReduction", i.NonParticipationReduction),
                                                                    new XElement("VendorPayment",             i.VendorPayment),
                                                                    new XElement("CalculatedFinalPayment",    i.CalculatedFinalPayment))))
                .ToString();

            var parameters = new[]
                             {
                                 new SqlParameter
                                 {
                                     ParameterName = "@XML",
                                     Direction     = ParameterDirection.Input,
                                     Value         = pphXml,
                                     DbType        = DbType.Xml
                                 }
                             };

            _baseJob.RunSproc(Name, Sproc, parameters);

            _participantPaymentHistories = null;
        }

        private void IssueOverOrUnderPayment(DataTable participationPeriodDetails)
        {
            var    batchReportEntries = new DataSet("BatchReportEntries");
            string bdXml;

            batchReportEntries.Tables.Add(participationPeriodDetails);
            participationPeriodDetails.TableName = "BatchReportEntry";
            Sproc                                = "wwp.USP_Issue_OverOrUnderPayment";

            var opXml = new XElement("OverUnderPayments",
                                     _overUnderPayments.Select(i => new XElement("OverUnderPayment",
                                                                                 new XElement("ParticipantId",          i.ParticipantId),
                                                                                 new XElement("PinNumber",              i.PinNumber),
                                                                                 new XElement("CaseNumber",             i.CaseNumber),
                                                                                 new XElement("ParticipationBeginDate", i.ParticipationBeginDate),
                                                                                 new XElement("ParticipationEndDate",   i.ParticipationEndDate),
                                                                                 new XElement("FinalPayment",           i.FinalPayment),
                                                                                 new XElement("RevisedPaymentAmount",   i.RevisedPaymentAmount))))
                .ToString();

            using (TextWriter writer = new StringWriter())
            {
                batchReportEntries.WriteXml(writer);
                bdXml = writer.ToString();
            }

            var parameters = new[]
                             {
                                 new SqlParameter
                                 {
                                     ParameterName = "@XML",
                                     Direction     = ParameterDirection.Input,
                                     Value         = opXml,
                                     DbType        = DbType.Xml
                                 },

                                 new SqlParameter
                                 {
                                     ParameterName = "@BDXML",
                                     Direction     = ParameterDirection.Input,
                                     Value         = bdXml,
                                     DbType        = DbType.Xml
                                 }
                             };

            _baseJob.RunSproc(Name, Sproc, parameters);

            var overUnderPayments = _baseJob.DataTable;
            var overUnderPaymentsWithoutWorker = overUnderPayments.ConvertDataTable<OverUnderPaymentResult>()
                                                                  .Where(i => i.WorkerId == null && i.RevisedPaymentAmount != 0)
                                                                  .ToList();

            if (overUnderPaymentsWithoutWorker.Any())
                _overUnderPaymentEmail.SendEmail(_baseJob.DbConfig.Catalog, overUnderPaymentsWithoutWorker);

            _overUnderPayments = null;
        }
    }

    public class ParticipantPaymentHistory
    {
        public decimal  CaseNumber                { get; set; }
        public DateTime ParticipationBeginDate    { get; set; }
        public DateTime ParticipationEndDate      { get; set; }
        public decimal  BaseW2Payment             { get; set; }
        public decimal  DrugFelonPenalty          { get; set; }
        public decimal  Recoupment                { get; set; }
        public decimal  LearnFarePenalty          { get; set; }
        public decimal  NonParticipationReduction { get; set; }
        public decimal  VendorPayment             { get; set; }
        public decimal  CalculatedFinalPayment    { get; set; }
    }

    public class OverUnderPayment
    {
        public int      ParticipantId          { get; set; }
        public decimal  PinNumber              { get; set; }
        public decimal  CaseNumber             { get; set; }
        public DateTime ParticipationBeginDate { get; set; }
        public DateTime ParticipationEndDate   { get; set; }
        public decimal  FinalPayment           { get; set; }
        public decimal  RevisedPaymentAmount   { get; set; }
    }

    public class OverUnderPaymentResult
    {
        public decimal  PinNumber            { get; set; }
        public decimal  CaseNumber           { get; set; }
        public int?     WorkerId             { get; set; }
        public DateTime BeginDate            { get; set; }
        public DateTime EndDate              { get; set; }
        public decimal  RevisedPaymentAmount { get; set; }
    }

    #endregion
}
