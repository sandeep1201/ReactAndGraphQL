using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ApprovalReason : BaseCommonModel, IApprovalReason
    {
        ICollection<ITimeLimitExtension> IExtensionReason.TimeLimitExtensions
        {
            get { return this.TimeLimitExtensions.Cast<ITimeLimitExtension>().ToList(); }
            set { this.TimeLimitExtensions = value as ICollection<TimeLimitExtension>; }
        }

        ICollection<ITimeLimitType> IExtensionReason.TimeLimitTypes
        {
            get { return this.TimeLimitTypes.Cast<ITimeLimitType>().ToList();  }
            set { this.TimeLimitTypes = value as ICollection<TimeLimitType>; }
        }
    }

    public partial class DenialReason : BaseCommonModel, IDenialReason
    {
        ICollection<ITimeLimitExtension> IExtensionReason.TimeLimitExtensions
        {
            get { return this.TimeLimitExtensions.Cast<ITimeLimitExtension>().ToList(); }
            set { this.TimeLimitExtensions = value as ICollection<TimeLimitExtension>; }
        }

        ICollection<ITimeLimitType> IExtensionReason.TimeLimitTypes
        {
            get { return this.TimeLimitTypes.Cast<ITimeLimitType>().ToList(); }
            set { this.TimeLimitTypes = value as ICollection<TimeLimitType>; }
        }
    }
}
