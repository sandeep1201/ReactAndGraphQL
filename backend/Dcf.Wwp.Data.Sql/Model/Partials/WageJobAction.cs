using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageAction : BaseCommonModel, IWageAction, IEquatable<WageAction>
    {

        #region ICloneable

        public object Clone()
        {
            var wa = new WageAction
            {
                Id = this.Id,
                Name = this.Name,
                ActionType = this.ActionType,
                IsRequired = this.IsRequired,
                SortOrder = this.SortOrder,
                IsDeleted = this.IsDeleted
            };

            return wa;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WageAction;
            return obj != null && Equals(obj);
        }

        public bool Equals(WageAction other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(ActionType, other.ActionType))
                return false;
            if (!AdvEqual(IsRequired, other.IsRequired))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
