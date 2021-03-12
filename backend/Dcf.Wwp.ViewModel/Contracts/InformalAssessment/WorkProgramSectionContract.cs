using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts.Cww;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class WorkProgramSectionContract : BaseInformalAssessmentContract
    {
        public bool? IsInOtherPrograms { get; set; }

        public string Notes { get; set; }

        public List<WorkProgramContract> WorkPrograms { get; set; }

        public FsetStatus CwwFsetStatus { get; set; }

        public WorkProgramSectionContract()
        {
        }
    }
}
