using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActionNeededPageActionItemBridge : BaseCommonModel, IActionNeededPageActionItemBridge
    {
        IActionItem IActionNeededPageActionItemBridge.ActionItem
        {
            get { return ActionItem; }
            set { ActionItem = (ActionItem) value; }
        }

        IActionNeededPage IActionNeededPageActionItemBridge.ActionNeededPage
        {
            get { return ActionNeededPage; }
            set { ActionNeededPage = (ActionNeededPage) value; }
        }
    }
}
