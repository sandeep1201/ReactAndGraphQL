using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialReferralParent : ICommonModel, IHasDeleteReason, ICloneable
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        Nullable<bool> IsAvailableOrWorking { get; set; }
        string AvailableOrWorkingDetails { get; set; }
        Nullable<bool> IsInterestedInWorkProgram { get; set; }
        string InterestedInWorkProgramDetails { get; set; }
        Nullable<bool> IsContactKnownWithParent { get; set; }
        Nullable<int> ContactId { get; set; }

        ICollection<INonCustodialReferralChild> NonCustodialReferralChilds { get; set; }
    }
}
