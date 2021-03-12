namespace Dcf.Wwp.Api.Library.Contracts
{
    public class RefWorkerContract
    {
        public string MFUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountyName { get; set; }
        public string AgencyName { get; set; }
        public bool IsActive { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(MFUserId) &&
                   string.IsNullOrWhiteSpace(FirstName) &&
                   string.IsNullOrWhiteSpace(LastName);
        }

        #endregion
    }
}
