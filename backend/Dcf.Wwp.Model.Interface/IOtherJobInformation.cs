using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IOtherJobInformation : ICommonModel, ICloneable
    {
        String                              ExpectedScheduleDetails { get; set; }
        Int32?                              JobSectorId             { get; set; }
        Int32?                              JobFoundMethodId        { get; set; }
        String                              WorkerId                { get; set; }
        String                              JobFoundMethodDetails   { get; set; }
        Int32?                              SortOrder               { get; set; }
        Boolean                             IsDeleted               { get; set; }
        int?                                WorkProgramId           { get; set; }
        ICollection<IEmploymentInformation> EmploymentInformations  { get; set; }
        IJobFoundMethod                     JobFoundMethod          { get; set; }
        IJobSector                          JobSector               { get; set; }
        IWorkProgram                        WorkProgram             { get; set; }
    }
}
