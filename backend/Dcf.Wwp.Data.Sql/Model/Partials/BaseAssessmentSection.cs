using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public abstract class BaseAssessmentSection : BaseCommonModel
    {
        protected void CloneAssessmentSectionProperites(ICommonAssessmentSection clone)
        {
            var cas = this as ICommonAssessmentSection;
            if (cas != null)
            {
                clone.Id              = cas.Id;
                clone.ReviewCompleted = cas.ReviewCompleted;
                clone.IsDeleted       = cas.IsDeleted;
            }
        }

        protected bool AssessmentSectionProperitesEqual(ICommonAssessmentSection other)
        {
            var cas = this as ICommonAssessmentSection;
            if (cas == null)
                throw new InvalidOperationException("AssessmentSectionProperitesEqual called with improper class interface.");

            if (!AdvEqual(cas.Id, other.Id))
                return false;
            if (!AdvEqual(cas.ReviewCompleted, other.ReviewCompleted))
                return false;
            if (!AdvEqual(cas.IsDeleted, other.IsDeleted))
                return false;

            return true;
        }
    }
}
