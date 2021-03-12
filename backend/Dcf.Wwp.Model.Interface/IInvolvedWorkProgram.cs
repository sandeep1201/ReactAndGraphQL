using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IInvolvedWorkProgram : ICommonDelModel
    {
        int? WorkProgramSectionId { get; set; }
        int? WorkProgramStatusId { get; set; }
        int? WorkProgramId { get; set; }
        int? CityId { get; set; }
        DateTime? StartMonth { get; set; }
        DateTime? EndMonth { get; set; }
        int? ContactId { get; set; }
        string ContactInfo { get; set; }
        string Details { get; set; }
        int? SortOrder { get; set; }
        ICity City { get; set; }
        IContact Contact { get; set; }
        IWorkProgram WorkProgram { get; set; }
        IWorkProgramSection WorkProgramSection { get; set; }
        IWorkProgramStatus WorkProgramStatus { get; set; }
    }
}