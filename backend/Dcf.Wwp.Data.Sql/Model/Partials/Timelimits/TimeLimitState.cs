using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitState : BaseCommonModel, ITimeLimitState
    {
        [NotMapped]
        public DateTime? CreatedDate { get; set; }
    }
}
