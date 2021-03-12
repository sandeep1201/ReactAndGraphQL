using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public abstract class BaseEntity
    {
        #region Properties

        // properties common to all Entities - 'ModifiedBy' & 'ModifiedDate'
        // not included. This is because even though *most* tables have them
        // they are not consistently defined (some are VARCHAR(20), VARCHAR(40), etc)

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Timestamp, ConcurrencyCheck, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[] RowVersion { get; set; } = new byte[8];

        #endregion

        #region Methods

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

        #endregion
    }
}
