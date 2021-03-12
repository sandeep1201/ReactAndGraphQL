using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISpecialInitiative
    {
        int       Id             { get; set; }
        string    ParameterName  { get; set; }
        string    ParameterValue { get; set; }
        DateTime? ModifiedDate   { get; set; }
    }
}
