using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITransaction : ICommonModelFinal, ICloneable
    {
        int      ParticipantId     { get; set; }
        int?     WorkerId          { get; set; }
        int      OfficeId          { get; set; }
        int      TransactionTypeId { get; set; }
        string   Description       { get; set; }
        DateTime EffectiveDate     { get; set; }
        DateTime CreatedDate       { get; set; }
    }
}
