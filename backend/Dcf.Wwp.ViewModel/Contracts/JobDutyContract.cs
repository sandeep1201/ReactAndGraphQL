using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class JobDutyContract : IIsEmpty
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(Details);
        }

        #endregion IIsEmpty
    }
}