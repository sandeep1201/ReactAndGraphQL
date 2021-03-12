using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialCaretaker : ICommonModel, IHasDeleteReason, ICloneable
    {
        int    NonCustodialParentsSectionId            { get; set; }
        string FirstName                               { get; set; }
        bool   IsFirstNameUnknown                      { get; set; }
        string LastName                                { get; set; }
        bool   IsLastNameUnknown                       { get; set; }
        int?   NonCustodialParentRelationshipId        { get; set; }
        string RelationshipDetails                     { get; set; }
        int?   ContactIntervalId                       { get; set; }
        string ContactIntervalDetails                  { get; set; }
        bool?  IsRelationshipChangeRequested           { get; set; }
        string RelationshipChangeRequestedDetails      { get; set; }
        bool?  IsInterestedInRelationshipReferral      { get; set; }
        string InterestedInRelationshipReferralDetails { get; set; }

        IContactInterval                ContactInterval                { get; set; }
        INonCustodialParentRelationship NonCustodialParentRelationship { get; set; }
        INonCustodialParentsSection     NonCustodialParentsSection     { get; set; }
        ICollection<INonCustodialChild> NonCustodialChilds             { get; set; }
    }
}
