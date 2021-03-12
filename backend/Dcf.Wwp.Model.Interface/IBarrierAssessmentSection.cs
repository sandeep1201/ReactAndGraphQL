using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierAssessmentSection : ICommonAssessmentSection, ICloneable
    {
        ICollection<IInformalAssessment> InformalAssessments { get; set; }
    }
}
