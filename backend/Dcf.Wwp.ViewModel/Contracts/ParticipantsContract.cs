using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ParticipantsContract
    {
        #region Properties

        //modified to get rid of privates
        public int                           Id                           { get; private set; }
        public string                        FirstName                    { get; private set; }
        public string                        MiddleInitialName            { get; set; }
        public string                        LastName                     { get; private set; }
        public string                        SuffixName                   { get; set; }
        public decimal?                      Pin                          { get; private set; }
        public DateTime?                     DateOfBirth                  { get; private set; }
        public bool?                         IsConfidentialCase           { get; set; }
        public bool?                         HasConfidentialAccess        { get; set; } // as in the Worker or Supervisor (auth user) has access.
        public string                        GenderIndicator              { get; private set; }
        public List<EnrolledProgramContract> Programs                     { get; private set; }
        public RefWorkerContract             RefWorkerContract            { get; set; }
        public string                        AgencyCode                   { get; private set; }
        public int?                          CountyOfResidenceId          { get; set; }
        public int?                          GroupOrder                   { get; set; }
        public long?                         SortOrder                    { get; set; }
        public DateTime?                     EnrolledDate                 { get; private set; }
        public DateTime?                     ReferralDate                 { get; set; }
        public DateTime?                     DisenrolledDate              { get; private set; }
        public bool?                         HasBeenThroughClientReg      { get; set; }
        public decimal?                      MciId                        { get; set; }
        public string                        CutOverDate                  { get; set; }
        public decimal?                      TotalLifeTimeSubsidizedHours { get; set; }
        public DateTime?                     TotalLifeTimeHoursDate       { get; set; }
        public List<EARequestContract>       EaRequests                   { get; set; }
        public bool?                         IsVerified                   { get; set; }

        #endregion

        #region Methods

        public ParticipantsContract()
        {
            // This forces us to use the Create factory method.
        }

        public static ParticipantsContract Create(
            int                           id,
            string                        firstName,
            string                        middleName,
            string                        lastName,
            string                        suffix,
            decimal?                      pin,
            DateTime?                     dateOfBirth,
            bool?                         isConfidential,
            bool                          hasAccess,
            string                        cutOverDate,
            bool?                         isVerified,
            string                        genderIndicator              = null,
            List<EnrolledProgramContract> enrolledPrograms             = null,
            int?                          groupOrder                   = null,
            long?                         sortOrder                    = null,
            RefWorkerContract             refWorkerContract            = null,
            int?                          countyOfResidenceId          = null,
            bool?                         hasBeenThroughClientReg      = null,
            decimal?                      mciId                        = null,
            decimal?                      totalLifeTimeSubsidizedHours = null,
            DateTime?                     totalLifeTimeHoursDate       = null,
            List<EARequestContract>       eaRequests                   = null
        )

        {
            var contract = new ParticipantsContract
                           {
                               Id                           = id,
                               FirstName                    = firstName.ToUpper(),
                               MiddleInitialName            = middleName?.ToUpper(),
                               LastName                     = lastName.ToUpper(),
                               SuffixName                   = suffix,
                               Pin                          = pin,
                               DateOfBirth                  = dateOfBirth,
                               IsConfidentialCase           = isConfidential,
                               HasConfidentialAccess        = hasAccess,
                               CutOverDate                  = cutOverDate,
                               IsVerified                   = isVerified,
                               GenderIndicator              = genderIndicator,
                               Programs                     = enrolledPrograms ?? new List<EnrolledProgramContract>(),
                               RefWorkerContract            = refWorkerContract,
                               CountyOfResidenceId          = countyOfResidenceId,
                               GroupOrder                   = groupOrder,
                               SortOrder                    = sortOrder,
                               HasBeenThroughClientReg      = hasBeenThroughClientReg,
                               MciId                        = mciId,
                               TotalLifeTimeSubsidizedHours = totalLifeTimeSubsidizedHours,
                               TotalLifeTimeHoursDate       = totalLifeTimeHoursDate,
                               EaRequests                   = eaRequests ?? new List<EARequestContract>()
                           };

            // Original contract properties that we need to still support until co-enrollment.
            var pep = enrolledPrograms?.FirstOrDefault(x => x.ProgramCode == "W-2");

            if (pep != null)
            {
                contract.EnrolledDate    = pep.EnrollmentDate;
                contract.DisenrolledDate = pep.DisenrollmentDate;
                contract.AgencyCode      = pep.AgencyCode;
            }

            return (contract);
        }

        #endregion
    }
}
