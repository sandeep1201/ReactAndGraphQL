using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class MilitaryRank : BaseCommonModel, IMilitaryRank, IEquatable<MilitaryRank>
    {
        ICollection<IMilitaryTrainingSection> IMilitaryRank.IMilitaryTrainingSections
        {
            get { return MilitaryTrainingSections.Cast<IMilitaryTrainingSection>().ToList(); }
            set { MilitaryTrainingSections = value.Cast<MilitaryTrainingSection>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var m = new MilitaryRank();

            m.Id   = this.Id;
            m.Name = this.Name;
            return m;
        }

        #endregion ICloneable


        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as MilitaryRank;
            return obj != null && Equals(obj);
        }

        public bool Equals(MilitaryRank other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
