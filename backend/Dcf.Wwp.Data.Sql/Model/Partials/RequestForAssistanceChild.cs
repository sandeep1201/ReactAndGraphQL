using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistanceChild : BaseCommonModel, IRequestForAssistanceChild
    {
        IChild IRequestForAssistanceChild.Child
        {
            get { return Child; }
            set { Child = (Child) value; }
        }

        // This parent objects should not be used for cloning as it will cause recursive
        // calls (bad).  We do need it though since a whole new object graph could be
        // created and we don't yet have an ID.
        IRequestForAssistance IRequestForAssistanceChild.RequestForAssistance
        {
            get { return RequestForAssistance; }
            set { RequestForAssistance = (RequestForAssistance) value; }
        }
    }
}
