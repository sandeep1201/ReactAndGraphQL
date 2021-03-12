using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitType : BaseCommonModel, ITimeLimitType
    {
        ICollection<ITimeLimitExtension> ITimeLimitType.TimeLimitExtensions
        {
            get { return this.TimeLimitExtensions.Cast<ITimeLimitExtension>().ToList(); }

            set { this.TimeLimitExtensions = value as ICollection<TimeLimitExtension>; }
        }

        ICollection<ITimeLimit> ITimeLimitType.TimeLimits
        {
            get { return this.TimeLimits.Cast<ITimeLimit>().ToList() ; }

            set { this.TimeLimits = value as ICollection<TimeLimit>; }
        }
    }
}
