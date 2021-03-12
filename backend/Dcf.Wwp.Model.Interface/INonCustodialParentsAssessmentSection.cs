using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialParentsAssessmentSection : ICommonDelModel, ICloneable
    {
        Nullable<bool> ReviewCompleted { get; set; }
    }
}
