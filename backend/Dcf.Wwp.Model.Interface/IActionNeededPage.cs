namespace Dcf.Wwp.Model.Interface
{
    public interface IActionNeededPage : ICommonDelModel
    {
        string Code { get; set; }
        string Name { get; set; }
        int SortOrder { get; set; }
    }
}