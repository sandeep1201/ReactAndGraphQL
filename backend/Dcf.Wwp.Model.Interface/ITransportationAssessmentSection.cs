using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITransportationAssessmentSection : ICommonDelModel, ICloneable
    {
        Boolean? ReviewCompleted { get; set; }
    }
}