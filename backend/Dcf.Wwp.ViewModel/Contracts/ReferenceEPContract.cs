using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class ReferenceEPContract : IFieldData
    {
        #region Properties

        [DataMember(Name = "id")]
        public int? Id { get; protected set; }

        [DataMember(Name = "enrolledProgamId")]
        public int? EnrolledProgramId { get; protected set; }

        [DataMember(Name = "maxDaysCanBackDate")]
        public int? MaxDaysCanBackDate { get; protected set; }

        [DataMember(Name = "maxDaysInProgressStatus")]
        public int? MaxDaysInProgressStatus { get; protected set; }

        [DataMember(Name = "maxDaysCanBackDatePS")]
        public int? MaxDaysCanBackDatePS { get; protected set; }

        #endregion

        #region Methods

        public static ReferenceEPContract Create(int? id, int? enrolledProgramId, int? maxBackDays, int? maxInProgressDays, int? maxBackDaysForPS)
        {
            var r = new ReferenceEPContract { Id = id, EnrolledProgramId = enrolledProgramId, MaxDaysCanBackDate = maxBackDays, MaxDaysInProgressStatus = maxInProgressDays, MaxDaysCanBackDatePS = maxBackDaysForPS };

            return (r);
        }

        #endregion
    }
}
