using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistancePopulationTypeBridge : BaseCommonModel, IRequestForAssistancePopulationTypeBridge
    {
        IRequestForAssistance IRequestForAssistancePopulationTypeBridge.RequestForAssistance
        {
            get { return RequestForAssistance; }
            set { RequestForAssistance = (RequestForAssistance) value; }
        }

        IPopulationType IRequestForAssistancePopulationTypeBridge.PopulationType
        {
            get { return PopulationType; }
            set { PopulationType = (PopulationType) value; }
        }
    }
}
