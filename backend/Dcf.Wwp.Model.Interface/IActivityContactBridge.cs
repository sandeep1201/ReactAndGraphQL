using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IActivityContactBridge : ICommonDelModel, ICloneable
    {
        Int32?    ActivityId { get; set; }
        Int32?    ContactId  { get; set; }
        IActivity Activity   { get; set; }
        IContact  Contact    { get; set; }
    }
}