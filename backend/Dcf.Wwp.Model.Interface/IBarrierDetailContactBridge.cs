using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierDetailContactBridge : ICommonDelModel,ICloneable
    {
        Int32? BarrierDetailId { get; set; }
        Int32? ContactId { get; set; }
        IBarrierDetail BarrierDetail { get; set; }
        IContact Contact { get; set; }


    }
}
