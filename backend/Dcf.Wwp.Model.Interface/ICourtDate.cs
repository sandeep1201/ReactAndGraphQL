using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICourtDate : ICommonModel, ICloneable
    {
        Int32?              LegalSectionId     { get; set; }
        String              Location           { get; set; }
        Boolean?            IsUnknown          { get; set; }
        DateTime?           Date               { get; set; }
        String              Details            { get; set; }
        Boolean             IsDeleted          { get; set; }
        ILegalIssuesSection LegalIssuesSection { get; set; }
    }
}
