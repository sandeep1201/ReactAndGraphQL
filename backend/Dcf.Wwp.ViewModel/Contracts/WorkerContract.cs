namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WorkerContract
    {
        public int?   Id            { get; set; }
        public string WamsId        { get; set; }
        public string WorkerId      { get; set; }
        public string FirstName     { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName      { get; set; }
        public string Organization  { get; set; }
        public bool?  IsActive      { get; set; }
        public string Wiuid         { get; set; }


        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(WamsId)    &&
                   string.IsNullOrWhiteSpace(WorkerId)  &&
                   string.IsNullOrWhiteSpace(Wiuid)     &&
                   string.IsNullOrWhiteSpace(FirstName) &&
                   string.IsNullOrWhiteSpace(LastName);
        }

        #endregion IIsEmpty
    }
}
