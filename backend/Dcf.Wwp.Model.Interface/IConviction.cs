using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IConviction : ICommonModel, ICloneable, IHasDeleteReason
    {
        Int32? LegalSectionId { get; set; }
        Int32? ConvictionTypeID { get; set; }
        Boolean? IsUnknown { get; set; }
        DateTime? DateConvicted { get; set; }
        String Details { get; set; }
        IConvictionType ConvictionType { get; set; }
        ILegalIssuesSection LegalIssuesSection { get; set; }
    }
}