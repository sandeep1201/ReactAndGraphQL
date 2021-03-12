using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FCDPRfaDetail : BaseCommonModel, IFCDPRfaDetail
    {
        ICountyAndTribe IFCDPRfaDetail.CountyAndTribe
        {
            get { return CountyAndTribe; }
            set { CountyAndTribe = (CountyAndTribe) value; }
        }

        IRequestForAssistance IFCDPRfaDetail.RequestForAssistance
        {
            get { return RequestForAssistance; }
            set { RequestForAssistance = (RequestForAssistance) value; }
        }
    }
}
