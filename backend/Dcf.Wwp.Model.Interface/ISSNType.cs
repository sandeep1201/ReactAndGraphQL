using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISSNType : ICommonDelModel
    {
        string            Name              { get; set; }
        bool?             IsDetailsRequired { get; set; }
        ICollection<IAKA> AKAs              { get; set; }
    }
}
