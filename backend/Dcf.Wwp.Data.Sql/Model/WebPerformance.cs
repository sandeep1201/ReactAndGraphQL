using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WebPerformance
    {
        #region Properties

        // this entity does not have a 'RowVersion', so it does not inherit from 'BaseEntity'.cs

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string   MethodName { get; set; }
        public DateTime StartTime  { get; set; }
        public DateTime StopTime   { get; set; }
        public TimeSpan Elapsed    { get; set; }
        public int      Cached     { get; set; }
        public int      Web        { get; set; }
        public int      Retries    { get; set; }
        public int      Total      { get; set; }
        public string   UserId     { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
