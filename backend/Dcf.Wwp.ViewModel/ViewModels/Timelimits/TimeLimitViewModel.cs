using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Configuration;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Timelimts.Service;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class TimeLimitViewModel : BasePinViewModel
    {
        private readonly ITimelimitService _timelimitService;
        private readonly IDb2TimelimitService _db2TimelimitService;


        public TimeLimitViewModel(IRepository repository, IAuthUser authUser, ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService) : base(repository, authUser)
        {

            this._timelimitService = timelimitService;
            this._db2TimelimitService = db2TimelimitService;
        }

        public List<TimelineMonthContract> GeTimelineMonthHistory(DateTime date, String pin)
        {
            var result = new List<TimelineMonthContract>();
            var timelimitMonth = this.Repo.TimeLimitByDate(pin, date, true);

            if (timelimitMonth != null)
            {

                var results = this.Repo.TimeLimitsHistory(timelimitMonth.Id);
                foreach (var month in results)
                {
                    var contract = TimelineMonthContract.Create(month);
                    result.Add(contract);
                }
                if (!results.Any())
                {
                    result.Add(TimelineMonthContract.Create(timelimitMonth));
                }
            }

            return result;
        }

        public IEnumerable<UpsertResponse<TimelineMonthContract>> UpsertData(IEnumerable<TimelineMonthContract> models, string pin)
        {
            var responses = new List<UpsertResponse<TimelineMonthContract>>();
            var monthsToSave = new List<ITimeLimit>();
            if (this.Participant == null)
            {
                throw new UserFriendlyException("Pin not valid.");
            }
            if (models == null)
            {
                throw new UserFriendlyException("TimelineMonths data is missing.");
            }

            var timelineMonthContracts = models as IList<TimelineMonthContract> ?? models.ToList();
            var effectiveMonths = timelineMonthContracts.Select(x => x.EffectiveMonth.DateTime).ToList();
            var timelimitIds = timelineMonthContracts.Where(x => x.Id > 0).Select(x => x.Id);

            var monthsById = this.Repo.TimeLimitsByIds(timelimitIds).ToDictionary((i) => i.Id);
            var monthsByDate = this.Repo.TimeLimitsByDates(pin, effectiveMonths).ToDictionary(x => x.EffectiveMonth.GetValueOrDefault().ToString("YY-MM"));

            foreach (var model in timelineMonthContracts)
            {
                ITimeLimit timeLimit = null;
                if (model.Id > 0)
                {
                    timeLimit = monthsById[model.Id];
                }
                else if (monthsByDate.ContainsKey(model.EffectiveMonth.ToString("YY-MM")))
                {
                    timeLimit = monthsByDate[model.EffectiveMonth.ToString("YY-MM")];


                }
                else
                {

                    timeLimit = this.Repo.NewTimeLimit();
                }

                this.MapTimelimitContractToTimelimitModel(model, timeLimit);
                monthsToSave.Add(timeLimit);

            }
            WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = true;
            var executionStrategy = new DcfDbExecutionStrategy(5, TimeSpan.FromSeconds(30));

            executionStrategy.Execute(() =>
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    try
                    {
                        foreach (var model in monthsToSave)
                        {
                            this._db2TimelimitService.Upsert(model, this.Participant, this.AuthUser?.Username);
                            this.Repo.Save();

                        }
                        scope.Complete();
                        responses.AddRange(monthsToSave.Select(model => new UpsertResponse<TimelineMonthContract>() { UpdatedModel = TimelineMonthContract.Create(model) }));
                    }
                    catch (Exception ex)
                    {
                        responses.ForEach(x =>
                        {
                            x.HasConcurrencyError = ex is DBConcurrencyException;
                            x.ErrorMessage = ex.Message;
                        });
                        this.Logger.FatalException("Error performing UpsertData in TimelimitViewModels for {@Participant}, {@data}", ex, this.Participant.PinNumber, timelineMonthContracts);
                        throw;
                    }
                }
            });

            WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = false;

            return responses;
        }

        public UpsertResponse<TimelineMonthContract> UpsertData(TimelineMonthContract contract, string pin)
        {
            var response = new UpsertResponse<TimelineMonthContract>();
            if (this.Participant == null)
            {
                throw new UserFriendlyException("Pin not valid.");
            }

            if (contract == null)
            {
                throw new UserFriendlyException("TimelineMonth data is missing.");
            }

            ITimeLimit timeLimit;
            if (contract.Id == default(Int32))
            {

                // check if there is already a timelimit record for this month
                var extistingMonth = this.Repo.TimeLimitByDate(pin, contract.EffectiveMonth.DateTime, false);

                timeLimit = extistingMonth ?? this.Repo.NewTimeLimit();
            }
            else
            {
                timeLimit = this.Repo.TimeLimitById(contract.Id);
            }

            this.MapTimelimitContractToTimelimitModel(contract, timeLimit);

            WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = true;
            var executionStrategy = new DcfDbExecutionStrategy(5, TimeSpan.FromSeconds(30));

            executionStrategy.Execute(() =>
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    try
                    {
                        this._db2TimelimitService.Upsert(timeLimit, this.Participant, this.AuthUser.Username);
                        this.Repo.Save();
                        scope.Complete();
                        response.UpdatedModel = TimelineMonthContract.Create(timeLimit);
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessage = ex.Message;
                        this.Logger.ErrorException("Error performing UpsertData in TimelimitViewModel for {@Participant}, {@data}", ex, this.Participant.PinNumber, contract);
                        throw;
                    }

                }
            });

            WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = false;
            return response;
        }

        private void MapTimelimitContractToTimelimitModel(TimelineMonthContract contract, ITimeLimit timeLimit)
        {
            this.MapBaseContractToBaseModel(contract, timeLimit);

            timeLimit.ParticipantID = this.Participant.Id;
            timeLimit.ChangeReasonId = contract.ReasonForChangeId;
            timeLimit.EffectiveMonth = contract.EffectiveMonth.DateTime.StartOf(DateTimeUnit.Month);
            timeLimit.ChangeReasonDetails = contract.ChangeReasonDetails;
            timeLimit.FederalTimeLimit = contract.IsFederal;
            timeLimit.StateTimelimit = contract.IsState;
            timeLimit.TwentyFourMonthLimit = contract.IsPlacement;
            timeLimit.Notes = contract.Notes;
            timeLimit.StateId = contract.StateId;
            timeLimit.TimeLimitTypeId = contract.TimelimitTypeId;

        }

        public IEnumerable<TimeLimitStateContract> GetStates()
        {
            return this.Repo.TimeLimitStates(true).Select(model =>
            {
                var contract = new TimeLimitStateContract()
                {
                    Code = model.Code,
                    CountryId = model.CountryId,
                    Name = model.Name
                };
                BaseModelContract.SetBaseProperties(contract, model);
                return contract;
            });
        }

        public IEnumerable<ChangeReasonContract> GetChangeReasons()
        {
            return this.Repo.ChangeReasons().Select(x =>
            {
                var contract = new ChangeReasonContract()
                {
                    Name = x.Name,
                    IsRequired = x.IsRequired.GetValueOrDefault(),
                    Code = x.Code ?? ""


                };
                BaseModelContract.SetBaseProperties(contract, x);
                return contract;
            });
        }
    }
}