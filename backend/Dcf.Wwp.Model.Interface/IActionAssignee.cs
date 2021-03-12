namespace Dcf.Wwp.Model.Interface
{
    public interface IActionAssignee : ICommonDelModel
    {
        string Name { get; set; }
        int SortOrder { get; set; }
    }
}