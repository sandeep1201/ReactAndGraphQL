using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPendingCharge : ICommonModel, ICloneable
    {
        Int32?              LegalSectionId     { get; set; }
        Int32?              ConvictionTypeID   { get; set; }
        DateTime?           ChargeDate         { get; set; }
        Boolean?            IsUnknown          { get; set; }
        String              Details            { get; set; }
        Boolean             IsDeleted          { get; set; }
        IConvictionType     ConvictionType     { get; set; }
        ILegalIssuesSection LegalIssuesSection { get; set; }
    }
}
