namespace Dcf.Wwp.Model.Interface
{
    public interface IActionNeededPageActionItemBridge : ICommonDelModel
    {
        int ActionNeededPageId { get; set; }
        int ActionItemId { get; set; }
        int SortOrder { get; set; }

        IActionItem ActionItem { get; set; }
        IActionNeededPage ActionNeededPage { get; set; }
    }
}