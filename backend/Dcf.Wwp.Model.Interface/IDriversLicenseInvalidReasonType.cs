
namespace Dcf.Wwp.Model.Interface
{
    public interface IDriversLicenseInvalidReasonType : ICommonDelModel
    {
        string Name { get; set; }
        int SortOrder { get; set; }
    }
}