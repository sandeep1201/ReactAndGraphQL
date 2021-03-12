using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class YesOrNoRefusedContract
    {

        public YesOrNoRefusedContract(int? status, string statusName, string details)
        {
            this.Status = status;
            this.StatusName = statusName;
            this.Details = details;
        }

        [DataMember(Name = "status")]
        public int? Status { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }
    }
}