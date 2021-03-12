using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DeleteReasonByRepeater : BaseCommonModel, IDeleteReasonByRepeater
    {
        IDeleteReason IDeleteReasonByRepeater.DeleteReason
        {
            get { return DeleteReason; }
            set { DeleteReason = (DeleteReason) value; }
        }
    }
}
