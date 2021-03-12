using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class WeeklyHoursWorked : BaseCommonModel, IWeeklyHoursWorked, IEquatable<WeeklyHoursWorked>
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new WeeklyHoursWorked
                        {
                            Id                      = Id,
                            EmploymentInformationId = EmploymentInformationId,
                            StartDate               = StartDate,
                            Hours                   = Hours,
                            Details                 = Details,
                            TotalSubsidyAmount      = TotalSubsidyAmount,
                            TotalWorkSiteAmount     = TotalWorkSiteAmount,
                            IsDeleted               = IsDeleted
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as WeeklyHoursWorked;

            return obj != null && Equals(obj);
        }

        public bool Equals(WeeklyHoursWorked other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(EmploymentInformationId, other.EmploymentInformationId))
            {
                return false;
            }

            if (!AdvEqual(StartDate, other.StartDate))
            {
                return false;
            }

            if (!AdvEqual(Hours, other.Hours))
            {
                return false;
            }

            if (!AdvEqual(Details, other.Details))
            {
                return false;
            }

            if (!AdvEqual(TotalSubsidyAmount, other.TotalSubsidyAmount))
            {
                return false;
            }
            if (!AdvEqual(TotalWorkSiteAmount, other.TotalWorkSiteAmount))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
