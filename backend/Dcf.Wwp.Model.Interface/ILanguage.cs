namespace Dcf.Wwp.Model.Interface
{
    public interface ILanguage : ICommonModel
    {
        int?   SortOrder      { set; get; }
        string Code           { get; set; }
        string Name           { get; set; }
        string MFLanguageCode { get; set; }
    }
}
