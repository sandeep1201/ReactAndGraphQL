namespace Dcf.Wwp.Model.Interface
{
    public interface IHasDeleteReason
    {
        int? DeleteReasonId { get; set; }
        IDeleteReason DeleteReason { get; set; }
    }
}
