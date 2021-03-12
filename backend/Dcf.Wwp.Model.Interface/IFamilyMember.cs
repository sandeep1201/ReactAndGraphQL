using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFamilyMember : ICommonModel, IHasDeleteReason, ICloneable
    {
        Int32? FamilyBarriersSectionId { get; set; }
        Int32? RelationshipId { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
        String Details { get; set; }

        IFamilyBarriersSection FamilyBarriersSection { get; set; }
        IRelationship Relationship { get; set; }
    }
}