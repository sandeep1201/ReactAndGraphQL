using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWageHourHistoryRepository
    {
        IWageHourHistory NewWageHourHistory(IWageHour parentAssessment, String user);
    }
}
