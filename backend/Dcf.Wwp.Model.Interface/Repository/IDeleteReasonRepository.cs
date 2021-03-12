namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IDeleteReasonRepository
    {
        IDeleteReason DeleteReasonByName(string name);
        IDeleteReason DeleteReasonById(int id);
    }
}
