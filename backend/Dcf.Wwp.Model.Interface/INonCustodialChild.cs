using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialChild : ICommonModel, IHasDeleteReason, ICloneable
    {
        int NonCustodialCaretakerId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Nullable<System.DateTime> DateOfBirth { get; set; }
        Nullable<bool> HasChildSupportOrder { get; set; }
        Nullable<bool> HasNameOnChildBirthRecord { get; set; }
        string ChildSupportOrderDetails { get; set; }
        Nullable<int> ContactIntervalId { get; set; }
        string ContactIntervalDetails { get; set; }
        Nullable<int> HasOtherAdultsYesNoUnknownLookupId { get; set; }
        string OtherAdultsDetails { get; set; }
        Nullable<bool> IsRelationshipChangeRequested { get; set; }
        string RelationshipChangeRequestedDetails { get; set; }
        Nullable<int> IsNeedOfServicesYesNoUnknownLookupId { get; set; }
        string NeedOfServicesDetails { get; set; }
        INonCustodialCaretaker NonCustodialCaretaker { get; set; }
        IContactInterval ContactInterval { get; set; }
        IYesNoUnknownLookup HasOtherAdultsYesNoUnknownLookup { get; set; }
        IYesNoUnknownLookup IsNeedOfServicesYesNoUnknownLookup { get; set; }
    }
}
