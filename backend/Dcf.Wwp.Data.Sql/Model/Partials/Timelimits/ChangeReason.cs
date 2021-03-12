using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChangeReason : BaseEntity, IChangeReason
    {
        ICollection<ITimeLimit> IChangeReason.TimeLimits
        {
            get => TimeLimits.Cast<ITimeLimit>().ToList();
            set => TimeLimits = value as ICollection<TimeLimit>;
        }
    }
}
