using System;

namespace Dcf.Wwp.Model.Interface
{
    /// <summary>
    /// An extension of the Common Model interface that adds the IsDeleted column.
    /// </summary>
    public interface ICommonDelModel : ICommonModel, IIsDeleted
    {
    }
}
