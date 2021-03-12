namespace Dcf.Wwp.Api.Library.Contracts
{
    public abstract class BaseRepeaterContract : BaseContract
    {
        // For repeater contract items, we just need the row version... not the
        // modified by/date, etc.
        public byte[] RowVersion { get; set; }
    }
}
