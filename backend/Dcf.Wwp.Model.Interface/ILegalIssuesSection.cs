using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ILegalIssuesSection : ICommonDelModel, ICloneable
    {
        bool?    IsConvictedOfCrime            { get; set; }
        bool?    IsUnderCommunitySupervision   { get; set; }
        string   CommunitySupervisonDetails    { get; set; }
        bool?    HasPendingCharges             { get; set; }
        bool?    HasFamilyLegalIssues          { get; set; }
        string   FamilyLegalIssueNotes         { get; set; }
        bool?    HasCourtDates                 { get; set; }
        bool?    OrderedToPayChildSupport      { get; set; }
        decimal? MonthlyAmount                 { get; set; }
        bool?    IsUnknown                     { get; set; }
        bool?    OweAnyChildSupportBack        { get; set; }
        string   ChildSupportDetails           { get; set; }
        int?     CommunitySupervisonContactId  { get; set; }
        bool?    HasRestrainingOrders          { get; set; }
        string   RestrainingOrderNotes         { get; set; }
        bool?    HasRestrainingOrderToPrevent   { get; set; }
        string   RestrainingOrderToPreventNotes { get; set; }

        string                   Notes          { get; set; }
        ICollection<IConviction> Convictions    { get; set; }
        IContact                 Contact        { get; set; }
        ICollection<IConviction> AllConvictions { get; set; }
        ICollection<ICourtDate>  CourtDates     { get; set; }

        ICollection<ICourtDate>     AllCourtDates  { get; set; }
        ICollection<IPendingCharge> PendingCharges { get; set; }

        ICollection<IPendingCharge> AllPendingCharges { get; set; }
    }
}
