using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ActionNeededViewModel : BasePinViewModel
    {
        private readonly IAuthUser _authUser;

        public ActionNeededViewModel(IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
            _authUser = authUser;
        }

        public List<ActionNeededTaskContract> GetActionNeeded()
        {
            var participant = Participant;

            if (participant == null)
            {
                throw new InvalidOperationException("PIN is invalid");
            }

            var contract = new List<ActionNeededTaskContract>();

            var actionNeededs = Repo.ActionNeededsByParticipantId(participant.Id);

            foreach (var an in actionNeededs)
            {
                foreach (var ant in an.ActionNeededTasks)
                {
                    var task = new ActionNeededTaskContract
                               {
                                   Id                 = ant.Id,
                                   ActionNeededId     = ant.ActionNeededId,
                                   AssigneeId         = ant.ActionAssigneeId,
                                   AssigneeName       = ant.ActionAssignee?.Name,
                                   PageId             = an.ActionNeededPageId,
                                   PageName           = an.ActionNeededPage?.Name,
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
                                   RowVersion         = ant.RowVersion,
                                   ModifiedByName     = Repo.GetWorkerNameByWamsId(ant.ModifiedBy)
                               };

                    contract.Add(task);
                }
            }

            return contract;
        }

        public ActionNeededContract GetActionNeededForPage(string page) => GetActionNeededContract(Participant, Repo, page);

        public static ActionNeededContract GetActionNeededContract(IParticipant participant, IRepository repo, string page)
        {
            if (participant == null)
            {
                throw new InvalidOperationException("PIN is invalid.");
            }

            ActionNeededContract contract = null;

            var actionNeeded = repo.ActionNeededByParticipantIdAndPageCode(participant.Id, page);

            if (actionNeeded == null)
            {
                // If we don't have anything filled in yet, we'll create an empty contract.
                var actionNeededPage = repo.ActionNeededPageByCode(page);

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

                foreach (var ant in actionNeeded.ActionNeededTasks)
                {
                    var task = new ActionNeededTaskContract
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

                    contract.Tasks.Add(task);
                }
            }

            return contract;
        }

        public int UpsertActionNeeded(string page, ActionNeededContract contract, string user)
        {
            if (contract == null)
            {
                throw new InvalidOperationException("Action Needed data is missing.");
            }

            // Only if this is a new task will the page be allowed to be set...
            // i.e. it cannot be changed.

            var actionNeeded = Repo.ActionNeededByParticipantIdAndPageCode(Participant.Id, page);

            if (actionNeeded == null)
            {
                actionNeeded = Repo.NewActionNeeded(Participant.Id, contract.PageId, user);
            }

            Repo.StartChangeTracking(actionNeeded);

            // We're not going to track change tracking for Action Needed items... instead
            // it will be the last one wins.
            //var userRowVersion = contract.RowVersion;

            actionNeeded.IsNoActionNeeded = contract.IsNoActionNeeded;

            var hasAnyChanged = false;

            // If no action is needed, we need to potentially delete any previous tasks.
            if (actionNeeded.IsNoActionNeeded)
            {
                foreach (var ant in actionNeeded.ActionNeededTasks)
                {
                    Repo.StartChangeTracking(ant);

                    // We should only delete those that don't have a completion date.  Really
                    // the way the app works at this time, we should never have any items to
                    // be deleted that aren't completed.  The UI enforces this, but we'll
                    // support deleting them in case the UI changes.
                    if (!ant.CompletionDate.HasValue)
                    {
                        ant.IsDeleted = true;
                    }

                    // If we haven't seen a change yet, check this task.
                    if (!hasAnyChanged)
                    {
                        hasAnyChanged = Repo.HasChanged(ant);
                    }
                }
            }

            if (hasAnyChanged)
            {
                actionNeeded.ModifiedDate = DateTime.Now;
                actionNeeded.ModifiedBy   = user;
                Repo.Save();
            }
            else
            {
                Repo.SaveIfChanged(actionNeeded, user);
            }

            return actionNeeded.Id;
        }

        public ActionNeededTaskContract GetActionNeededTaskForPinById(string pin, int taskId)
        {
            var participant = Repo.GetParticipant(pin);

            if (participant == null)
            {
                throw new InvalidOperationException("PIN is invalid");
            }

            var ant = Repo.ActionNeededTaskById(taskId);

            if (ant == null)
            {
                return null;
            }

            if (ant.ActionNeeded == null)
            {
                throw new InvalidOperationException("ActionNeeded is invalid");
            }

            if (ant.ActionNeeded.ParticipantId != participant.Id)
            {
                throw new InvalidOperationException("PIN is not tied to Action Needed task.");
            }

            var contract = new ActionNeededTaskContract
                           {
                               Id                 = ant.Id,
                               ActionNeededId     = ant.ActionNeededId,
                               AssigneeId         = ant.ActionAssigneeId,
                               AssigneeName       = ant.ActionAssignee?.Name,
                               PageId             = ant.ActionNeeded.ActionNeededPageId,
                               PageName           = ant.ActionNeeded.ActionNeededPage?.Name,
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

            return contract;
        }

        /// <summary>
        ///     Inserts/Updates the Action Needed Task.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="contract"></param>
        /// <param name="user"></param>
        /// <returns>The ID of the Action Needed Task record.</returns>
        public int UpsertActionNeededTask(string pin, ActionNeededTaskContract contract, string user)
        {
            if (contract == null)
            {
                throw new InvalidOperationException("Action Needed Task data is missing.");
            }

            var participant = Repo.GetParticipant(pin);

            if (participant == null)
            {
                throw new InvalidOperationException("PIN is invalid.");
            }

            var actionNeededTask = Repo.ActionNeededTaskById(contract.Id);

            IActionNeeded actionNeeded = null;

            var sectionChange = false; // BAND-AID: user is changing task to a different section...

            if (actionNeededTask == null)
            {
                // Create a new one... but in order to do so we have to find the
                // related objects.
                var page = Repo.ActionNeededPageById(contract.PageId);

                if (page == null)
                {
                    throw new InvalidOperationException("Page is invalid.");
                }

                if (contract.ActionNeededId > 0)
                {
                    actionNeeded = Repo.ActionNeededById(contract.ActionNeededId);
                }
                else
                {
                    // Look up the Action Needed by page or create a new one.
                    actionNeeded = Repo.ActionNeededByParticipantIdAndPageCode(participant.Id, page.Code) ?? Repo.NewActionNeeded(participant.Id, page.Id, user);
                }

                //Repo.StartChangeTracking(actionNeeded);

                // We allow the front end to change the page and task.
                actionNeeded.ActionNeededPageId = page.Id;

                // Business Rule:  If a new action item task is created (like from the List view), then
                // the Is No Action Needed flag should automatically be cleared.
                actionNeeded.IsNoActionNeeded = false;

                actionNeededTask = Repo.NewActionNeededTask(actionNeeded, user);
            }
            else
            {
                actionNeeded = Repo.ActionNeededById(contract.ActionNeededId);

                if (actionNeeded.ActionNeededPageId != contract.PageId)
                {
                    // aha! user changed section task - BAND-AID

                    if (actionNeeded.ActionNeededTasks.Count > 1)
                    {
                        actionNeeded = Repo.NewActionNeeded(participant.Id, contract.PageId, user);
                        //actionNeeded.ActionNeededTasks.Add(actionNeededTask); // can't do this because of the use of interfaces... it doesn't do what you think it does...
                        sectionChange = true;

                        var antl = actionNeeded.ActionNeededTasks;
                        antl.Add(actionNeededTask);
                        actionNeeded.ActionNeededTasks = antl;
                    }
                    else
                    {
                        actionNeeded.ActionNeededPageId = contract.PageId;
                    }
                }
                else
                {
                    if (actionNeeded != null)
                    {
                        //Repo.StartChangeTracking(actionNeeded);

                        // We allow the front end to change the page and task.
                        actionNeeded.ActionNeededPageId = contract.PageId;
                    }
                }
            }

            Repo.StartChangeTracking(actionNeeded);
            Repo.StartChangeTracking(actionNeededTask);

            // We're not going to track change tracking for Action Needed items... instead
            // it will be the last one wins.
            //var userRowVersion = contract.RowVersion;

            actionNeededTask.ActionAssigneeId = contract.AssigneeId;
            actionNeededTask.ActionItemId     = actionNeeded?.ActionNeededPageId == ActionNeededPage.OtherId ? null : contract.ActionItemId;
            actionNeededTask.ActionItemId     = contract.ActionItemId;
            actionNeededTask.ActionPriorityId = contract.PriorityId;
            actionNeededTask.FollowUpTask     = contract.FollowUpTask;
            actionNeededTask.IsNoDueDate      = contract.IsNoDueDate;
            actionNeededTask.DueDate          = contract.IsNoDueDate ? null : contract.DueDate;

            // When the front end sends us a Is Not Completion Date value of true and the value
            // in the database is false, we will set the completion date to now.
            if (contract.IsNoCompletionDate && !actionNeededTask.IsNoCompletionDate)
            {
                actionNeededTask.IsNoCompletionDate = true;
                actionNeededTask.CompletionDate     = _authUser.CDODate ?? DateTime.Now;
            }
            else
            {
                if (contract.IsNoCompletionDate && actionNeededTask.IsNoCompletionDate)
                {
                    // In this case, the front end is set to Is Not Completion Date and the databse
                    // already is set that way... we don't change anything.
                }
                else
                {
                    actionNeededTask.IsNoCompletionDate = contract.IsNoCompletionDate;
                    actionNeededTask.CompletionDate     = contract.IsNoCompletionDate ? null : contract.CompletionDate;
                }
            }

            actionNeededTask.Details = contract.Details;

            if (!Repo.SaveIfChanged(actionNeeded, user))
            {
                Repo.SaveIfChanged(actionNeededTask, user);
            }

            return actionNeededTask.Id;
        }

        public void DeleteActionNeededTask(string pin, int taskId, string user)
        {
            var participant = Repo.GetParticipant(pin);

            if (participant == null)
            {
                throw new InvalidOperationException("PIN is invalid.");
            }

            var ant = Repo.ActionNeededTaskById(taskId);

            if (ant == null)
            {
                throw new InvalidOperationException("Action Needed Task is invalid.");
            }

            ant.IsDeleted    = true;
            ant.ModifiedDate = DateTime.Now;
            ant.ModifiedBy   = user;

            Repo.Save();
        }
    }
}
