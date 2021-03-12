using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EnrolledProgramEPActivityTypeBridge : BaseCommonModel, IEnrolledProgramEPActivityTypeBridge
    {
        IActivityType IEnrolledProgramEPActivityTypeBridge.ActivityType
        {
            get { return ActivityType; }
            set { ActivityType = (ActivityType) value; }
        }

        IEnrolledProgram IEnrolledProgramEPActivityTypeBridge.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var a = new EnrolledProgramEPActivityTypeBridge
                    {
                        Id                  = Id,
                        EnrolledProgramId   = EnrolledProgramId,
                        ActivityTypeId      = ActivityTypeId,
                        IsSelfDirected      = IsSelfDirected,
                        IsDeleted           = IsDeleted,
                        IsUpfrontActivity   = IsUpfrontActivity,
                        IsSanctionable      = IsSanctionable,
                        IsAssessmentRelated = IsAssessmentRelated
                    };

            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            return other is EnrolledProgramEPActivityTypeBridge obj && Equals(obj);
        }

        public bool Equals(Activity other)
        {
            ////Check whether the compared object is null.
            //if (Object.ReferenceEquals(other, null)) return false;

            ////Check whether the compared object references the same data.
            //if (Object.ReferenceEquals(this, other)) return true;

            ////Check whether the products' properties are equal
            //if (!AdvEqual(Id, other.Id))
            //    return false;
            //if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
            //    return false;
            //if (!AdvEqual(ActivityTypeId, other.ActivityTypeId))
            //    return false;
            //if (!AdvEqual(Description, other.Description))
            //    return false;
            //if (!AdvEqual(ActivityLocationId, other.ActivityLocationId))
            //    return false;
            //if (!AdvEqual(Details, other.Details))
            //    return false;
            //if (!AdvEqual(IsDeleted, other.IsDeleted))
            //    return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
