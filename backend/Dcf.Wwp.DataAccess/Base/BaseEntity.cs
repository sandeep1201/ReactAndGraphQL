using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.DataAccess.Base
{
    public abstract class BaseEntity // : IBaseEntity
    {
        #region Properties

        // properties common to all Entities

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required, Timestamp, ConcurrencyCheck, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[] RowVersion { get; set; }

        #endregion

        #region Methods

        public BaseEntity ()
        {
            RowVersion = new byte[8];
        }

        #endregion
    }
}
