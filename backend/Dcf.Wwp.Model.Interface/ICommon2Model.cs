using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICommon2Model : ICommonDelModel
    {
        DateTime CreatedDate { get; set; }
    }
}