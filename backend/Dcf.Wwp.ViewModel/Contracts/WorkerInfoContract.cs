namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WorkerInfoContract
    {
        public int?    Id          { get; set; }
        public decimal PhoneNumber { get; set; }
        public string  Email       { get; set; }
        public int     WorkerId    { get; set; }


        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return (WorkerId    == 0) &&
                   (PhoneNumber == 0) &&
                   string.IsNullOrWhiteSpace(Email);
        }

        #endregion IIsEmpty
    }
}
