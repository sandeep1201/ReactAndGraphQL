using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Common.Tasks;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using EnumsNET;
using Nito.AsyncEx;
using Serilog;

namespace DCF.Timelimits.Processors
{
    [BatchTaskProcess(Priority = 0)]
    public class AuxillaryPaymentsProcessor : BatchTaskProcessBase<ProcessAuxillaryContext, ProcessAuxillaryResult>
    {
        private readonly IRepository _repo;
        private readonly ITimelimitService _timelimitService;
        private readonly ApplicationContext _appContext;
        private DateTimeRange _no24DateRange;

        public AuxillaryPaymentsProcessor(IRepository repo, ITimelimitService _timelimitService, ApplicationContext appContext)
        {
            this._repo = repo;
            this._timelimitService = _timelimitService;
            this._appContext = appContext;
            this._no24DateRange = new DateTimeRange(new DateTime(2009, 11, 01), new DateTime(2011, 12, 31));
        }

        public override async Task<ProcessAuxillaryResult> Handle(ProcessAuxillaryContext context, CancellationToken token)
        {

            ProcessAuxillaryResult result = new ProcessAuxillaryResult();

            var timelimits = _repo.TimeLimitsByPin(context.PinNumber.ToString()).ToDictionary(x => x.EffectiveMonth);
            foreach (var auxPayment in context.AuxiliaryPayments)
                {
                    var month = DateTime.ParseExact(auxPayment.BENEFIT_MM.Value.ToString(CultureInfo.InvariantCulture), "yyyyMM", CultureInfo.InvariantCulture);
                    ITimeLimit timelimit;
                    timelimits.TryGetValue(month, out timelimit);

                    if (timelimit == null)
                    {
                        timelimit = this._appContext.IsSimulation ? new TimeLimit() : _repo.NewTimeLimit();
                    }
                    else if (timelimit.TimeLimitTypeId == (Int32)ClockTypes.None || timelimit.TimeLimitTypeId == (Int32)ClockTypes.OTF)
                    {
                        this._logger.Info($"Empty \"{(ClockTypes)timelimit.TimeLimitTypeId.Value}\" Timelimit record found. Overwriting! ");

                    }
                    else if (timelimit.ModifiedDate.HasValue)
                    {
                        this._logger.Info($"Edited Timelimit record found with ClockType: \"{(ClockTypes)timelimit.TimeLimitTypeId.Value}\". skipping! ");
                        continue;
                    }
                    else
                    {
                        this._logger.Info($"Batch Timelimit record found with ClockType: \"{(ClockTypes)timelimit.TimeLimitTypeId.Value}\". Skipping! ");
                        continue;
                    }

                    ClockTypes clockType;
                    if (!Enum.TryParse(auxPayment.CLOCK_TYPE_CD, out clockType))
                    {
                        this._logger.Info($" Unable to parse clocktype with CLOCK_TYPE_CD: \"{auxPayment.CLOCK_TYPE_CD}\". Skipping! ");
                    }

                    timelimit.ParticipantID = context.Participant.Id;
                    timelimit.EffectiveMonth = month.StartOf(DateTimeUnit.Month);
                    timelimit.TimeLimitTypeId = (Int32)clockType;
                    timelimit.TwentyFourMonthLimit = clockType.HasAnyFlags(ClockTypes.PlacementTypes) && !_no24DateRange.Contains(timelimit.EffectiveMonth.Value);
                    timelimit.StateTimelimit = true;
                    timelimit.FederalTimeLimit = auxPayment.FED_CLOCK_IND == "Y";
                    timelimit.CreatedDate = auxPayment.UPDATED_DT;
                    timelimit.ModifiedBy = "WWP Batch";
                    timelimit.Notes = $"Imported missing month created by:{auxPayment.CRE_TRAN_CD}, Comments from old WP application: {auxPayment.COMMENT_TXT}";
                    timelimit.IsDeleted = auxPayment.OVERRIDE_REASON_CD?.ToUpper().StartsWith("S") ?? false;

                    // Update AUX so it doesn't import again
                    auxPayment.PinNumber = context.PinNumber;
                    auxPayment.EffectiveMonth = timelimit.EffectiveMonth;
                    auxPayment.TimeLimitTypeId = timelimit.TimeLimitTypeId;
                    auxPayment.StateTimelimit = timelimit.StateTimelimit;
                    auxPayment.FederalTimeLimit = timelimit.FederalTimeLimit;
                    auxPayment.TwentyFourMonthLimit = timelimit.TwentyFourMonthLimit;
                    auxPayment.CreatedDateFromCARES = auxPayment.UPDATED_DT;
                    auxPayment.ModifiedBy = timelimit.ModifiedBy;
                    auxPayment.ModifiedDate = timelimit.ModifiedDate;

                    if (!this._appContext.IsSimulation)
                    {
                        await _repo.SaveAsync(token);
                        await this._timelimitService.SaveEntityAsync(auxPayment, token);
                    }
                    result.ProcessedPayments.Add(auxPayment);
                }

            return result;
        }
    }
}