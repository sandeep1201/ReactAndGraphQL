using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Symptom : BaseCommonModel, ISymptom
    {
        ICollection<IFormalAssessment> ISymptom.FormalAssessments
        {
            get => FormalAssessments.Cast<IFormalAssessment>().ToList();
            set => FormalAssessments = value.Cast<FormalAssessment>().ToList();
        }
    }
}
