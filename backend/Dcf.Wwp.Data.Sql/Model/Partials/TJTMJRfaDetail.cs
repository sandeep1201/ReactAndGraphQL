using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TJTMJRfaDetail : BaseCommonModel, ITJTMJRfaDetail
    {
        IOrganization ITJTMJRfaDetail.Organization
        {
            get { return Organization; }
            set { Organization = (Organization) value; }
        }

        IRequestForAssistance ITJTMJRfaDetail.RequestForAssistance
        {
            get { return RequestForAssistance; }
            set { RequestForAssistance = (RequestForAssistance) value; }
        }
    }
}
