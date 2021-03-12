using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class BenefitsOfferedAction : BaseCommonModel, IBenefitsOfferedAction, IEquatable<BenefitsOfferedAction>
    {
        //ICollection<IJobBenefitsOfferedActionBridge> IBenefitsOfferedAction.JobBenefitsOfferedActionBridges
        //{
        //    get { return JobBenefitsOfferedActionBridges.Cast<IJobBenefitsOfferedActionBridge>().ToList(); }

        //    set { JobBenefitsOfferedActionBridges = (ICollection<JobBenefitsOfferedActionBridge>)value; }
        //}

        #region ICloneable

        public object Clone()
        {
            var clone = new BenefitsOfferedAction
            {
                Id = this.Id,
                SortOrder = this.SortOrder,
                Name = this.Name,
                ActionType = this.ActionType,
                IsRequired = this.IsRequired
            };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as BenefitsOfferedAction;
            return obj != null && Equals(obj);
        }

        public bool Equals(BenefitsOfferedAction other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id) &&
                   Name.Equals(other.Name) &&
                   ActionType.Equals(other.ActionType) &&
                   IsRequired.Equals(other.IsRequired) &&
                   SortOrder.Equals(other.SortOrder);
        }

        #endregion IEquatable<T>
    }
}
