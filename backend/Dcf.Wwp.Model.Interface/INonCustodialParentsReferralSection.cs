using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialParentsReferralSection : ICommonDelModel, ICloneable, IComplexModel
    {
        int ParticipantId { get; set; }
        Nullable<int> HasChildrenId { get; set; }
        string Notes { get; set; }

        IYesNoSkipLookup YesNoSkipLookup { get; set; }
        ICollection<INonCustodialReferralParent> AllNonCustodialReferralParents { get; set; }
        ICollection<INonCustodialReferralParent> NonCustodialReferralParents { get; set; }
    }
}