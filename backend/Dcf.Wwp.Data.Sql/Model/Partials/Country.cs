using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class Country : BaseCommonModel, ICountry, IEquatable<Country>
    {
        ICollection<IState> ICountry.States
        {
            get { return States.Cast<IState>().ToList(); }
            set { States = value.Cast<State>().ToList(); }
        }

        ICollection<ICity> ICountry.Cities
        {
            get { return Cities.Cast<ICity>().ToList(); }
            set { Cities = value.Cast<City>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var ct = new Country();

            ct.Id   = this.Id;
            ct.Name = this.Name;
            return ct;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Country;
            return obj != null && Equals(obj);
        }

        public bool Equals(Country other)
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

            return true;
        }

        #endregion IEquatable<T>
    }
}
