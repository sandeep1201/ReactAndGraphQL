using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class GoalDomain : IGoalDomain
    {
        #region Properties

        private readonly IGoalRepository                        _goalRepository;
        private readonly IGoalStepRepository                    _goalstepRepository;
        private readonly IEmployabilityPlanGoalBridgeRepository _epgBridgeRepository;
        private readonly IUnitOfWork                            _unitOfWork;
        private readonly IAuthUser                              _authUser;

        private readonly Func<string, string> _convertWIUIdToName; // this is sneaky, don't do this at home.

        #endregion

        #region Methods

        public GoalDomain(IGoalRepository                        goalRepository,
                          IGoalStepRepository                    goalstepRepository,
                          IEmployabilityPlanGoalBridgeRepository epgBridgeRepository,
                          IWorkerRepository                      workerRepo,
                          IUnitOfWork                            unitOfWork, IAuthUser authUser)
        {
            _goalRepository      = goalRepository;
            _goalstepRepository  = goalstepRepository;
            _epgBridgeRepository = epgBridgeRepository;
            _unitOfWork          = unitOfWork;
            _authUser            = authUser;

            // inline Func<> to lookup/convert WIUID -> Name...
            _convertWIUIdToName = (wiuId) =>
                                  {
                                      var wo = workerRepo.GetAsQueryable()
                                                         .Where(i => i.WIUId == wiuId)
                                                         .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                         .FirstOrDefault();

                                      var wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");

                                      return (wn);
                                  };
        }

        public List<GoalContract> GetGoalsForEP(int epId)
        {
            var contracts = new List<GoalContract>();

            var goals = _epgBridgeRepository.GetAsQueryable()
                                            .Where(i => i.EmployabilityPlanId == epId && i.IsDeleted == false)
                                            .Select(i => i.Goal)
                                            .OrderBy(i => i.GoalType.SortOrder)
                                            .ToList();

            goals.ForEach(g => contracts.Add(
                                             new GoalContract
                                             {
                                                 Id               = g.Id,
                                                 Name             = g.Name,
                                                 GoalTypeId       = g.GoalTypeId,
                                                 GoalTypeName     = g.GoalType?.Name,
                                                 BeginDate        = g.BeginDate?.ToString("MM/dd/yyyy"),
                                                 Details          = g.Details,
                                                 EndDate          = g.EndDate?.ToString("MM/dd/yyyy"),
                                                 EndReasonId      = g.GoalEndReasonId,
                                                 EndReasonName    = g.GoalEndReason?.Name,
                                                 EndReasonDetails = g.EndReasonDetails,
                                                 IsGoalEnded      = g.IsGoalEnded,
                                                 ModifiedBy       = _convertWIUIdToName(g.ModifiedBy),
                                                 ModifiedDate     = g.ModifiedDate,
                                                 GoalSteps = g.GoalSteps.Select(gs => new GoalStepContract
                                                                                      {
                                                                                          Details = gs.Details,
                                                                                          IsGoalStepCompleted = gs.IsGoalStepCompleted
                                                                                      }).ToList()
                                             }));

            return (contracts);
        }

        public GoalContract GetGoal(int id)
        {
            GoalContract contract = null;

            var goal = _goalRepository.Get(g => g.Id == id);

            if (goal != null)
            {
                contract = new GoalContract
                           {
                               Id               = goal.Id,
                               BeginDate        = goal.BeginDate?.ToString("MM/dd/yyyy"),
                               Details          = goal.Details,
                               GoalTypeId       = goal.GoalType?.Id,
                               GoalTypeName     = goal.GoalType?.Name,
                               Name             = goal.Name,
                               EndDate          = goal.EndDate?.ToString("MM/dd/yyyy"),
                               EndReasonId      = goal.GoalEndReasonId,
                               EndReasonName    = goal.GoalEndReason?.Name,
                               EndReasonDetails = goal.EndReasonDetails,
                               IsGoalEnded      = goal.IsGoalEnded,
                               ModifiedBy       = _convertWIUIdToName(goal.ModifiedBy),
                               ModifiedDate     = goal.ModifiedDate,
                               GoalSteps        = new List<GoalStepContract>()
                           };

                var goalSteps = goal.GoalSteps?.Where(i => !i.IsDeleted).ToList();
                goalSteps?.ForEach(gs => contract.GoalSteps.Add(new GoalStepContract { Id = gs.Id, Details = gs.Details, IsGoalStepCompleted = gs.IsGoalStepCompleted }));
            }

            return (contract);
        }

        public List<GoalContract> GetGoalsForPin(string pin)
        {
            var decimalPin = decimal.Parse(pin);
            var contracts  = new List<GoalContract>();
            var goals = _epgBridgeRepository.GetAsQueryable()
                                            .Where(i => i.EmployabilityPlan.Participant.PinNumber == decimalPin && i.IsDeleted == false)
                                            .Select(i => i.Goal)
                                            .Distinct()
                                            .OrderByDescending(i => i.GoalEndReasonId == null)
                                            .ThenByDescending(i => i.BeginDate)
                                            .ToList();

            goals.ForEach(g => contracts.Add(
                                             new GoalContract
                                             {
                                                 Id               = g.Id,
                                                 BeginDate        = g.BeginDate?.ToString("MM/dd/yyyy"),
                                                 Details          = g.Details,
                                                 GoalTypeId       = g.GoalType?.Id,
                                                 GoalTypeName     = g.GoalType?.Name,
                                                 Name             = g.Name,
                                                 EndDate          = g.EndDate?.ToString("MM/dd/yyyy"),
                                                 EndReasonId      = g.GoalEndReasonId,
                                                 EndReasonName    = g.GoalEndReason?.Name,
                                                 EndReasonDetails = g.EndReasonDetails,
                                                 IsGoalEnded      = g.IsGoalEnded,
                                                 ModifiedBy       = _convertWIUIdToName(g.ModifiedBy),
                                                 ModifiedDate     = g.ModifiedDate,
                                                 Program          = g.EmployabilityPlanGoalBridges.FirstOrDefault()?.EmployabilityPlan.EnrolledProgram.ShortName,
                                                 EmployabilityPlanId = g.EmployabilityPlanGoalBridges
                                                                        .OrderByDescending(i => i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                                                        .ThenByDescending(i => i.EmployabilityPlan.BeginDate)
                                                                        .Where(i => i.EmployabilityPlan.EmployabilityPlanStatusTypeId    == EmployabilityPlanStatus.SubmittedId
                                                                                    || i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.EndedId
                                                                                    || i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                                                        .Select(i => i.EmployabilityPlanId)
                                                                        .FirstOrDefault()
                                             }));

            return (contracts);
        }

        public bool DeleteGoal(int goalId, int epId, bool canCommit = true)
        {
            var goalBridgeCount = _epgBridgeRepository.GetMany(g => g.GoalId == goalId).Count();

            if (goalBridgeCount == 1)
                _goalRepository.Delete(g => g.Id == goalId);
            else
                if (goalBridgeCount > 1)
                    _epgBridgeRepository.Delete(epgb => epgb.EmployabilityPlanId == epId && epgb.GoalId == goalId);

            var i = 0;

            if (canCommit)
                i = _unitOfWork.Commit();

            return !canCommit || i > 0;
        }

        public GoalContract UpsertGoal(GoalContract goalContract, string pin, int epId)
        {
            GoalContract contract = null;

            if (goalContract == null)
            {
                throw new ArgumentNullException(nameof(goalContract));
            }

            var updateTime = DateTime.Now;

            var goal = goalContract.Id != 0 ? _goalRepository.Get(g => g.Id == goalContract.Id && g.IsDeleted == false) : _goalRepository.New();

            if (goalContract.Id != 0)
            {
                if (goal != null)
                {
                    goal.GoalTypeId       = goalContract.GoalTypeId;
                    goal.BeginDate        = goalContract.BeginDate.ToDateTimeMonthDayYear();
                    goal.Name             = goalContract.Name;
                    goal.Details          = goalContract.Details;
                    goal.EndDate          = goalContract.EndDate.ToDateTimeMonthDayYear();
                    goal.GoalEndReasonId  = goalContract.EndReasonId;
                    goal.EndReasonDetails = goalContract.EndReasonDetails;
                    goal.IsGoalEnded      = goalContract.EndReasonId != null;
                    goal.IsDeleted        = false;
                    goal.ModifiedBy       = _authUser.WIUID;
                    goal.ModifiedDate     = updateTime;

                    if (goal.GoalSteps != null)
                    {
                        var allIds        = goal.GoalSteps?.Select(gs => gs.Id).ToList();
                        var contractIds   = goalContract.GoalSteps?.Select(gs => gs.Id).ToList();
                        var idsToDelete   = allIds.Except(contractIds.AsNotNull()).ToList();
                        var stepsToDelete = goal.GoalSteps.Where(i => idsToDelete.Contains(i.Id)).Select(i => i).ToList();
                        var stepsToUpdate = goalContract.GoalSteps?.Where(i => i.Id != 0).Select(i => i).ToList();
                        var stepsToAdd    = goalContract.GoalSteps?.Where(i => i.Id == 0).Select(i => i).ToList();

                        stepsToDelete.ForEach(step => _goalstepRepository.Delete(step));

                        foreach (var stepFromContract in stepsToUpdate.AsNotNull())
                        {
                            var step = goal.GoalSteps.FirstOrDefault(gs => gs.Id == stepFromContract.Id);

                            if (step != null)
                            {
                                // let AutoMapper do this in tech debt
                                step.GoalId              = goal.Id;
                                step.Details             = stepFromContract.Details;
                                step.IsGoalStepCompleted = stepFromContract.IsGoalStepCompleted;
                                step.ModifiedBy          = _authUser.WIUID;
                                step.ModifiedDate        = updateTime;
                            }
                        }

                        foreach (var stepFromContract in stepsToAdd.AsNotNull())
                        {
                            var step = new GoalStep
                                       {
                                           Details             = stepFromContract.Details,
                                           IsDeleted           = false,
                                           IsGoalStepCompleted = stepFromContract.IsGoalStepCompleted,
                                           ModifiedBy          = _authUser.WIUID,
                                           ModifiedDate        = updateTime
                                       };

                            // let AutoMapper do this in tech debt
                            goal.GoalSteps.Add(step);
                        }
                    }

                    if (goalContract.EndReasonId != null)
                    {
                        _epgBridgeRepository.Delete(i => i.EmployabilityPlanId == epId && i.GoalId == goalContract.Id);
                    }

                    _goalRepository.Update(goal);
                    _unitOfWork.Commit();
                    contract = GetGoal(goal.Id);
                }
            }
            else
            {
                goal.GoalTypeId           = goalContract.GoalTypeId;
                goal.BeginDate            = goalContract.BeginDate.ToDateTimeMonthDayYear();
                goal.Name                 = goalContract.Name;
                goal.Details              = goalContract.Details;
                goal.EndDate              = goalContract.EndDate.ToDateTimeMonthDayYear();
                goal.GoalEndReasonId      = goalContract.EndReasonId;
                goal.EndReasonDetails     = goalContract.EndReasonDetails;
                goal.IsGoalEnded          = goalContract.EndReasonId != null;
                goal.IsDeleted            = false;
                goal.ModifiedBy           = _authUser.WIUID;
                goal.ModifiedDate         = updateTime;
                goalContract.ModifiedBy   = _authUser.WIUID;
                goalContract.ModifiedDate = updateTime;

                var epgb = new EmployabilityPlanGoalBridge
                           {
                               EmployabilityPlanId = epId,
                               IsDeleted           = false,
                               ModifiedBy          = _authUser.WIUID,
                               ModifiedDate        = updateTime
                           };
                goal.EmployabilityPlanGoalBridges.Add(epgb);

                foreach (var goalStep in goalContract.GoalSteps.AsNotNull())
                {
                    var gs = new GoalStep
                             {
                                 Details             = goalStep.Details,
                                 IsGoalStepCompleted = goalStep.IsGoalStepCompleted,
                                 IsDeleted           = false,
                                 ModifiedBy          = _authUser.WIUID,
                                 ModifiedDate        = updateTime
                             };
                    goal.GoalSteps.Add(gs);
                }

                _goalRepository.Add(goal);
                _unitOfWork.Commit();
                contract = GetGoal(goal.Id);
            }

            return (contract);
        }

        #endregion
    }
}
