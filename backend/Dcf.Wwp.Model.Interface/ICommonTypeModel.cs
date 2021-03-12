namespace Dcf.Wwp.Model.Interface
{
    public interface ICommonTypeModel : ICommonDelModel
    {
        int    SortOrder { get; set; }
        string Name      { get; set; }
    }

    public interface ICommonTypeModelFinal : ICommonModelFinal
    {
        int    SortOrder { get; set; }
        string Name      { get; set; }
    }
}
