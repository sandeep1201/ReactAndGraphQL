using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public abstract class BaseCommonModel
    {
        #region Proeprties

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Timestamp, ConcurrencyCheck, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[] RowVersion { get; set; } = new byte[8];

        #endregion


        #region Partial ICloneable & Equals Support

        public override int GetHashCode()
        {
            int hash = 19;

            var model = this as ICommonModel;

            if (model != null)
            {
                unchecked
                {
                    // allow "wrap around" in the int
                    hash = hash * 31 + model.Id; // assuming integer
                }
            }

            return hash;
        }

        #endregion

        //public bool IsDeleted
        //{
        //    get
        //    {
        //        // NOTE: We can get some recursion errors if the types are not matched

        //        if (this is IHasDeleteReason)
        //        {
        //            var objDeleted = (IHasDeleteReason)this;
        //            // If we have any delete reason return as deleted. 
        //            if (objDeleted.DeleteReason?.Id != null)
        //                return true;
        //        }
        //        else
        //            if (this is IIsDeleted)
        //        {
        //            var objDeleted = (IIsDeleted)this;
        //            return objDeleted.IsDeleted;
        //        }

        //        // Defaults to not deleted.
        //        return false;
        //    }

        //    set
        //    {
        //        if (this is IIsDeleted objDeleted)
        //        {
        //            objDeleted.IsDeleted = value;
        //        }
        //    }
        //}

        public bool IsDeleted { get; set; }

        protected bool AreBothNotNull(object obj1, object obj2)
        {
            return (obj1 != null && obj2 != null);
        }

        protected bool EitherNotNull(object obj1, object obj2)
        {
            return (obj1 != null || obj2 != null);
        }

        protected bool AdvEqual(object obj1, object obj2)
        {
            if (obj1 == null ^ obj2 == null)
            {
                return false;
            }

            if (obj1 == null && obj2 == null)
            {
                return true;
            }

            if (obj1.Equals(obj2))
            {
                return true;
            }

            return false;
        }
    }
}
