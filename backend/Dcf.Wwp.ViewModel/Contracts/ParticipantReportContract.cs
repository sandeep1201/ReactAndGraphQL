using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ParticipantReportContract
    {
        #region Properties
        //modified to get rid of privates
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitialName { get; set; }
        public string LastName { get; set; }
        public string SuffixName { get; set; }
        public decimal? Pin { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsConfidentialCase { get; set; }
        public bool? HasConfidentialAccess { get; set; } // as in the Worker or Supervisor (auth user) has access.
        public List<EnrolledProgramContract> Programs { get; set; }
        public RefWorkerContract RefWorkerContract { get; set; }
        public string AgencyCode { get; set; }
        public int? CountyOfResidenceId { get; set; }
        public int? GroupOrder { get; set; }
        public long? SortOrder { get; set; }
        public DateTime? EnrolledDate { get; set; }
        public DateTime? ReferralDate { get; set; }
        public DateTime? DisenrolledDate { get; set; }
        public bool? HasBeenThroughClientReg { get; set; }
        public decimal? MciId { get; set; }

        #endregion

        #region Methods

        public ParticipantReportContract()
        {
            // This forces us to use the Create factory method.
        }

        #endregion
    }
}
