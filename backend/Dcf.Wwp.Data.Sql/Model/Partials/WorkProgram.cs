using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class WorkProgram : BaseCommonModel, IWorkProgram, IEquatable<WorkProgram>
    {
        #region ICloneable

        public object Clone()
        {
            var wp = new WorkProgram();

            wp.Id   = this.Id;
            wp.Name = this.Name;

            return wp;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WorkProgram;
            return obj != null && Equals(obj);
        }

        public bool Equals(WorkProgram other)
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
