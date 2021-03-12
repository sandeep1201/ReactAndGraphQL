using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CFRfaDetail : BaseCommonModel, ICFRfaDetail
    {
        ICountyAndTribe ICFRfaDetail.CountyAndTribe
        {
            get => CountyAndTribe;
            set => CountyAndTribe = (CountyAndTribe) value;
        }

        IRequestForAssistance ICFRfaDetail.RequestForAssistance
        {
            get => RequestForAssistance;
            set => RequestForAssistance = (RequestForAssistance) value;
        }
    }
}
