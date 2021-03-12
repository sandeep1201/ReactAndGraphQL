namespace Dcf.Wwp.Model.Interface
{
    public interface IWorkHistorySectionEmploymentPreventionTypeBridge : ICommonDelModel
    {
        int WorkHistorySectionId { get; set; }
        int EmploymentPreventionTypeId { get; set; }

        IEmploymentPreventionType EmploymentPreventionType { get; set; }
        IWorkHistorySection WorkHistorySection { get; set; }
    }
}