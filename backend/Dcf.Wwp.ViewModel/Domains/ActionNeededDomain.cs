using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;


namespace Dcf.Wwp.Api.Library.Domains
{
    public class ActionNeededDomain : IActionNeededDomain
    {
        #region Properties

        private readonly IActionNeededRepository     _actionNeededRepository;
        private readonly IActionNeededPageRepository _actionNeededPageRepository;

        #endregion

        #region Methods

        public ActionNeededDomain(IActionNeededRepository     actionNeededRepository,
                                  IActionNeededPageRepository actionNeededPageRepository)
        {
            _actionNeededRepository     = actionNeededRepository;
            _actionNeededPageRepository = actionNeededPageRepository;
        }

        public ActionNeededContract GetActionNeededContract(Participant participant, string page)
        {
            if (participant == null)
            {
                throw new InvalidOperationException("PIN is invalid.");
            }

            ActionNeededContract contract;

            var actionNeeded = _actionNeededRepository.GetAsQueryable()
                                                      .Where(i => i.ParticipantId == participant.Id && i.ActionNeededPage.Code == page && i.IsDeleted == false)
                                                      .OrderBy(i => i.Id)
                                                      .FirstOrDefault();

            if (actionNeeded == null)
            {
                // If we don't have anything filled in yet, we'll create an empty contract.
                var actionNeededPage = _actionNeededPageRepository.GetAsQueryable().SingleOrDefault(i => i.Code == page && i.IsDeleted == false);

                if (actionNeededPage == null)
                {
                    throw new InvalidOperationException("Action Needed Page is invalid.");
                }

                contract = ActionNeededContract.Create(default(int), actionNeededPage.Id, actionNeededPage.Name, default(bool), string.Empty, null, null);
            }
            else
            {
                contract = ActionNeededContract.Create(actionNeeded.Id, actionNeeded.ActionNeededPageId, actionNeeded.ActionNeededPage.Name, actionNeeded.IsNoActionNeeded,
                                                       actionNeeded.ModifiedBy, actionNeeded.ModifiedDate, actionNeeded.RowVersion);

                actionNeeded.ActionNeededTasks.Where(i => i.IsDeleted == false).ForEach(ant =>
                                                                                        {
                                                                                            var antContract = new ActionNeededTaskContract
                                                                                                              {
                                                                                                                  Id                 = ant.Id,
                                                                                                                  ActionNeededId     = ant.ActionNeededId,
                                                                                                                  AssigneeId         = ant.ActionAssigneeId,
                                                                                                                  AssigneeName       = ant.ActionAssignee?.Name,
                                                                                                                  PageId             = actionNeeded.ActionNeededPageId,
                                                                                                                  PageName           = actionNeeded.ActionNeededPage?.Name,
                                                                                                                  ActionItemId       = ant.ActionItemId,
                                                                                                                  ActionItemName     = ant.ActionItem?.Name,
                                                                                                                  PriorityId         = ant.ActionPriorityId,
                                                                                                                  PriorityName       = ant.ActionPriority?.Name,
                                                                                                                  FollowUpTask       = ant.FollowUpTask,
                                                                                                                  DueDate            = ant.DueDate,
                                                                                                                  IsNoDueDate        = ant.IsNoDueDate,
                                                                                                                  CompletionDate     = ant.CompletionDate,
                                                                                                                  IsNoCompletionDate = ant.IsNoCompletionDate,
                                                                                                                  Details            = ant.Details,
                                                                                                                  CreatedDate        = ant.CreatedDate,
                                                                                                                  ModifiedBy         = ant.ModifiedBy,
                                                                                                                  ModifiedDate       = ant.ModifiedDate,
                                                                                                                  RowVersion         = ant.RowVersion
                                                                                                              };

                                                                                            contract.Tasks.Add(antContract);
                                                                                        });
            }

            return contract;
        }

        #endregion
    }
}
