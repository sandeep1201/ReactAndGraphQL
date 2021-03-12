using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class BarrierDetailContract
    {
        public BarrierDetailContract()
        {
            Contacts = new List<int?>();
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "rowVersion")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "barrierTypeId")]
        public int? BarrierTypeId { get; set; }

        [DataMember(Name = "barrierTypeName")]
        public string BarrierTypeName { get; set; }

        [DataMember(Name = "barrierSubType")]
        public BarrierSubTypeContract BarrierSubType { get; set; }

        [DataMember(Name = "onsetDate")]
        public string OnsetDate { get; set; }

        [DataMember(Name = "endDate")]
        public string EndDate { get; set; }

        [DataMember(Name = "wasClosedAtDisenrollment")]
        public bool WasClosedAtDisenrollment { get; set; }

        [DataMember(Name = "isAccommodationNeeded")]
        public bool? IsAccommodationNeeded { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }

        [DataMember(Name = "contacts")]
        public List<int?> Contacts { get; set; }

        [DataMember(Name = "formalAssessments")]
        public List<FormalAssessmentContract> FormalAssessments { get; set; }   // these should be ICollections<>

        [DataMember(Name = "deletedFormalAssessments")]
        public List<FormalAssessmentContract> DeletedFormalAssessments { get; set; }

        [DataMember(Name = "barrierAccommodations")]
        public List<BarrierAccommodationContract> BarrierAccommodations { get; set; }

        [DataMember(Name = "deletedBarrierAccommodations")]
        public List<BarrierAccommodationContract> DeletedBarrierAccommodations { get; set; }

        [DataMember(Name = "isConverted")]
        public bool? IsConverted { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool? IsDeleted { get; set; }

        [DataMember(Name = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [DataMember(Name = "modifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }
}
