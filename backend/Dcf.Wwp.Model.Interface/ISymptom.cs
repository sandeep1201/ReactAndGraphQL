using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISymptom:ICommonDelModel
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        ICollection<IFormalAssessment> FormalAssessments { get; set; }
    }
}