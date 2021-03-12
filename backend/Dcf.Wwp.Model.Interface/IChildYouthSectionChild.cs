using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IChildYouthSectionChild : ICommonModel, ICloneable, IComplexModel, IHasDeleteReason
    {
        Int32? ChildYouthSectionId { get; set; }
        Int32? ChildId { get; set; }
        Int32? CareArrangementId { get; set; }
        Int32? AgeCategoryId { get; set; }
        Boolean? IsSpecialNeeds { get; set; }
        String Details { get; set; }

        IAgeCategory AgeCategory { get; set; }
        IChild Child { get; set; }
        IChildCareArrangement ChildCareArrangement { get; set; }

        // This parent objects should not be used for cloning as it will cause recursive
        // calls (bad).  We do need it though since a whole new object graph could be
        // created and we don't yet have an ID.
        IChildYouthSection ChildYouthSection { get; set; }
    }
}