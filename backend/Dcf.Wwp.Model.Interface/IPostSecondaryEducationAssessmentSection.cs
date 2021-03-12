using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPostSecondaryEducationAssessmentSection : ICommonDelModel, ICloneable
    {
        Boolean? ReviewCompleted { get; set; }
        String ActionDetails { get; set; }
        ICollection<IInformalAssessment> InformalAssessments { get; set; }

    }
}