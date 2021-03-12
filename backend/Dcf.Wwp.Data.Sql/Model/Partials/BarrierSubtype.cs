﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierSubtype : BaseCommonModel, IBarrierSubtype, IEquatable<BarrierSubtype>
    {
        ICollection<IBarrierTypeBarrierSubTypeBridge> IBarrierSubtype.BarrierTypeBarrierSubTypeBridges
        {
            get => BarrierTypeBarrierSubTypeBridges.Cast<IBarrierTypeBarrierSubTypeBridge>().ToList();
            set => BarrierTypeBarrierSubTypeBridges = value.Cast<BarrierTypeBarrierSubTypeBridge>().ToList();
        }

        IBarrierType IBarrierSubtype.BarrierType
        {
            get => BarrierType;
            set => BarrierType = (BarrierType) value;
        }

        #region ICloneable

        public object Clone()
        {
            var bst = new BarrierSubtype();

            bst.Id        = Id;
            bst.SortOrder = SortOrder;
            bst.Name      = Name;

            return bst;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as BarrierSubtype;

            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierSubtype other)
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

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(SortOrder, other.SortOrder))
            {
                return false;
            }

            if (!AdvEqual(Name, other.Name))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
