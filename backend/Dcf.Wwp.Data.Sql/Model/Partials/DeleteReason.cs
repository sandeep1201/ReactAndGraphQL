using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DeleteReason : BaseCommonModel, IDeleteReason
    {
        ICollection<ITimeLimitExtension> IDeleteReason.TimeLimitExtensions
        {
            get { return TimeLimitExtensions.Cast<ITimeLimitExtension>().ToList(); }
            set { TimeLimitExtensions = value.Cast<TimeLimitExtension>().ToList(); }
        }

        ICollection<IDeleteReasonByRepeater> IDeleteReason.DeleteReasonByRepeaters
        {
            get { return DeleteReasonByRepeaters.Cast<IDeleteReasonByRepeater>().ToList(); }
            set { DeleteReasonByRepeaters = value.Cast<DeleteReasonByRepeater>().ToList(); }
        }
    }
}
