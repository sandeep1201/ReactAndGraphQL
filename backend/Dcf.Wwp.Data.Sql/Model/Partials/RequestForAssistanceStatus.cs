using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistanceStatus : BaseCommonModel, IRequestForAssistanceStatus
    {
        ICollection<IRequestForAssistance> IRequestForAssistanceStatus.RequestsForAssistance
        {
            get { return RequestsForAssistance.Cast<IRequestForAssistance>().ToList(); }

            set { RequestsForAssistance = value.Cast<RequestForAssistance>().ToList(); }
        }
    }
}
