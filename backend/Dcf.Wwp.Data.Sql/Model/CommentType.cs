using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CommentType
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
