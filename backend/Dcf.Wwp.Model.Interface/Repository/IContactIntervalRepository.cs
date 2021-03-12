using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IContactIntervalRepository
    {
        IEnumerable<IContactInterval> ContactIntervals();
    }
}
