using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFeatureURL : ICommonModelFinal, ICloneable
    {
        String  Feature   { get; set; }
        String  URL       { get; set; }
        Boolean IsDeleted { get; set; }
    }
}
