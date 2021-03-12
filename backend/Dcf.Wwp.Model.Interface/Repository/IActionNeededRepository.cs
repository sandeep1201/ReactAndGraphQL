using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IActionNeededRepository
    {
        IEnumerable<IActionNeeded> ActionNeededsByParticipantId(int participantId);

        IActionNeededTask NewActionNeededTask(IActionNeeded actionNeeded, string user);

        IActionNeededTask ActionNeededTaskById(int taskId);

        void ResetNoActionNeededs(int participantId);

        IActionNeeded ActionNeededById(int? id);

        IActionNeeded ActionNeededByParticipantIdAndPageCode(int participantId, string pageCode);

        IActionNeeded NewActionNeeded(int participantId, int pageId, string user);

        IEnumerable<IActionAssignee> ActionAssignees();

        IEnumerable<IActionPriority> ActionPriorities(bool orderDesc = false);

        IEnumerable<IActionNeededPage> ActionNeededPages();

        IActionNeededPage ActionNeededPageById(int id);

        IActionNeededPage ActionNeededPageByCode(string code);

        IEnumerable<IActionItem> ActionItemsForPage(string pageCode);

        /// <summary>
        /// Supplies all ActionNeededPages and ActionItems via the bridge table.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IActionNeededPageActionItemBridge> ActionNeededPageActionItems();
    }
}
