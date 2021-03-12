using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IActionNeededRepository
    {
        public void ResetNoActionNeededs(int participantId)
        {
            var ans = from x in _db.ActionNeededs where x.ParticipantId == participantId && !x.IsDeleted && x.IsNoActionNeeded select x;

            foreach (var actionNeeded in ans)
            {
                actionNeeded.IsNoActionNeeded = false;
            }

            ////better
            //var actions = _db.ActionNeededs.Where(i => i.ParticipantId == participantId && !i.IsDeleted && i.IsNoActionNeeded).Select(i => i);
            //actions.ForEach(i => i.IsNoActionNeeded = false);

            ////best ;) - sbv
            //_db.ActionNeededs.Where(i => i.ParticipantId == participantId && !i.IsDeleted && i.IsNoActionNeeded).Select(i => i).ForEach(i => i.IsNoActionNeeded = false);
        }

        public IActionNeeded ActionNeededById(int? actionNeededId)
        {
            var r = _db.ActionNeededs.FirstOrDefault(i => i.Id == actionNeededId);

            return (r);
        }

        public IActionNeeded ActionNeededByParticipantIdAndPageCode(int participantId, string pageCode)
        {
            // There should only be one record returned here, but we'll order the data such that the
            // first one by it's Id is chosen if there is more than one.
            return (from x in _db.ActionNeededs where x.ParticipantId == participantId && x.ActionNeededPage.Code == pageCode orderby x.Id select x).FirstOrDefault();
        }

        public IActionNeeded NewActionNeeded(int participantId, int pageId, string user)
        {
            var obj = new ActionNeeded
                      {
                          ParticipantId      = participantId,
                          ActionNeededPageId = pageId,
                          ModifiedBy         = user,
                          ModifiedDate       = DateTime.Now,
                          CreatedDate        = _authUser.CDODate ?? DateTime.Now
                      };

            _db.ActionNeededs.Add(obj);

            return obj;
        }

        public IActionNeededPage ActionNeededPageById(int id)
        {
            return (from x in _db.ActionNeededPages
                    where x.Id == id
                    select x).SingleOrDefault();
        }

        public IActionNeededPage ActionNeededPageByCode(string code)
        {
            return (from x in _db.ActionNeededPages
                    where x.Code == code
                    select x).SingleOrDefault();
        }

        public IEnumerable<IActionNeeded> ActionNeededsByParticipantId(int participantId)
        {
            return from x in _db.ActionNeededs
                   where x.ParticipantId == participantId && !x.IsDeleted
                   orderby x.ActionNeededPage.SortOrder
                   select x;
        }

        public IActionNeededTask NewActionNeededTask(IActionNeeded actionNeeded, string user)
        {
            var obj = new ActionNeededTask
                      {
                          ActionNeeded = (ActionNeeded) actionNeeded,
                          ModifiedBy   = user,
                          ModifiedDate = DateTime.Now,
                          CreatedDate  = _authUser.CDODate ?? DateTime.Now
                      };

            _db.ActionNeededTasks.Add(obj);

            return obj;
        }

        public IActionNeededTask ActionNeededTaskById(int taskId)
        {
            return (from x in _db.ActionNeededTasks where x.Id == taskId select x).SingleOrDefault();
        }


        public IEnumerable<IActionAssignee> ActionAssignees()
        {
            return from x in _db.ActionAssignees
                   where !x.IsDeleted
                   orderby x.SortOrder
                   select x;
        }

        public IEnumerable<IActionPriority> ActionPriorities(bool orderDesc = false)
        {
            var priorities = _db.ActionPriorities.Where(i => !i.IsDeleted);

            if (orderDesc)
                return priorities.OrderByDescending(i => i.SortOrder)
                                 .Select(i => i);

            return priorities.OrderBy(i => i.SortOrder)
                             .Select(i => i);
        }

        public IEnumerable<IActionNeededPage> ActionNeededPages()
        {
            return from x in _db.ActionNeededPages
                   where !x.IsDeleted
                   orderby x.SortOrder
                   select x;
        }

        public IEnumerable<IActionNeededPageActionItemBridge> ActionNeededPageActionItems()
        {
            return from x in _db.ActionNeededPageActionItemBridges
                   where !x.IsDeleted
                   orderby x.SortOrder
                   select x;
        }

        public IEnumerable<IActionItem> ActionItemsForPage(string pageCode)
        {
            return from x in _db.ActionNeededPageActionItemBridges
                   where !x.IsDeleted && !x.ActionItem.IsDeleted
                                      && x.ActionNeededPage.Code == pageCode
                   orderby x.SortOrder
                   select x.ActionItem;
        }
    }
}
