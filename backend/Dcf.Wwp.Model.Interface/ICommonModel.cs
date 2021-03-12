using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICommonModel : IHasId
    {
        string    ModifiedBy   { get; set; }
        DateTime? ModifiedDate { get; set; }
        byte[]    RowVersion   { get; set; }
    }

    public interface ICommonModelFinal : IHasId, IIsDeleted
    {
        string   ModifiedBy   { get; set; }
        DateTime ModifiedDate { get; set; }
        byte[]   RowVersion   { get; set; }
    }
}
