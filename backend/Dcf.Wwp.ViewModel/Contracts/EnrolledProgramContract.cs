using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EnrolledProgramContract
    {
        public EnrolledProgramContract()
        {
        }


        /// <summary>
        ///  Map Entity to Contract
        /// </summary>
        /// <param name="pep"></param>
        public EnrolledProgramContract(IParticipantEnrolledProgram pep)
        {
            var associatedAgencies = pep.Office?.ContractArea?.AssociatedOrganizations?.Select(i => i.Organization).ToList();
            var associatedAgencyCodes = associatedAgencies?.Where(i => (i.InActivatedDate == null || i.InActivatedDate >= DateTime.Today) && i.ActivatedDate <= DateTime.Today)
                                                          .Select(i => i.EntsecAgencyCode).ToList();
            var associatedAgencyNames = associatedAgencies?.Where(i => (i.InActivatedDate == null || i.InActivatedDate >= DateTime.Today) && i.ActivatedDate <= DateTime.Today)
                                                          .Select(i => i.AgencyName).ToList();

            Id                    = pep.Id;
            ParticipantId         = pep.ParticipantId;
            EnrolledProgramId     = pep.EnrolledProgramId;
            RfaNumber             = pep.RequestForAssistance?.RfaNumber;
            ProgramCode           = pep.EnrolledProgram?.ProgramCode;
            ProgramCd             = pep.EnrolledProgram?.ProgramCode;
            SubProgramCode        = pep.EnrolledProgram?.SubProgramCode;
            EnrollmentDate        = pep.EnrollmentDate;
            DisenrollmentDate     = pep.DisenrollmentDate;
            CompletionReasonId    = pep.CompletionReasonId;
            ReferralDate          = pep.ReferralDate;
            Status                = pep.StatusCode?.StatusCode;
            OfficeCounty          = pep.Office?.CountyAndTribe?.CountyName;
            AgencyCode            = pep.Office?.ContractArea?.Organization?.EntsecAgencyCode;
            AgencyName            = pep.Office?.ContractArea?.Organization?.AgencyName;
            AssociatedAgencyCodes = associatedAgencyCodes;
            AssociatedAgencyNames = associatedAgencyNames;
            OfficeId              = pep.Office?.Id;
            OfficeNumber          = pep.Office?.OfficeNumber;
            CourtOrderedDate = pep.IsCF
                                   ? pep.RequestForAssistance?.CFRfaDetails?
                                        .FirstOrDefault(i => i.RequestForAssistanceId == pep.RequestForAssistance?.Id)
                                        ?.CourtOrderEffectiveDate
                                   : pep.RequestForAssistance?.FCDPRfaDetails?
                                        .FirstOrDefault(i => i.RequestForAssistanceId == pep.RequestForAssistance?.Id)
                                        ?.CourtOrderEffectiveDate;
            ContractorId   = pep.Office?.ContractArea?.Organization?.Id;
            ContractorName = pep.Office?.ContractArea?.Organization?.AgencyName;
            IsVoluntary = pep.RequestForAssistance?.FCDPRfaDetails?
                             .FirstOrDefault(i => i.RequestForAssistanceId == pep.RequestForAssistance?.Id)
                             ?.IsVoluntary
                          ?? false;
            CaseNumber = pep.CASENumber;
        }


        public int?           Id                      { get; set; }
        public int?           ParticipantId           { get; set; }
        public int?           EnrolledProgramId       { get; set; }
        public decimal?       RfaNumber               { get; set; }
        public string         ProgramCode             { get; set; }
        public string         ProgramCd               { get; set; }
        public string         SubProgramCode          { get; set; }
        public DateTime?      EnrollmentDate          { get; set; }
        public DateTime?      DisenrollmentDate       { get; set; }
        public DateTime?      ReferralDate            { get; set; }
        public string         Status                  { get; set; }
        public bool?          IsTransfer              { get; set; }
        public DateTime?      StatusDate              { get; set; }
        public bool?          CanDisenroll            { get; set; }
        public string         OfficeCounty            { get; set; }
        public string         AgencyCode              { get; set; }
        public string         AgencyName              { get; set; }
        public List<string>   AssociatedAgencyCodes   { get; set; }
        public List<string>   AssociatedAgencyNames   { get; set; }
        public int?           OfficeId                { get; set; }
        public int?           OfficeNumber            { get; set; }
        public WorkerContract AssignedWorker          { get; set; }
        public DateTime?      CourtOrderedDate        { get; set; }
        public string         CourtOrderedCounty      { get; set; }
        public int?           CompletionReasonId      { get; set; }
        public string         CompletionReasonDetails { get; set; }
        public int?           ContractorId            { get; set; }
        public string         ContractorName          { get; set; }
        public bool           IsVoluntary             { get; set; }
        public WorkerContract LearnFareFEP            { get; set; }
        public decimal?       CaseNumber              { get; set; }

        [JsonIgnore]
        public bool IsTJ => (EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId);

        [JsonIgnore]
        public bool IsCF => (EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.ChildrenFirstId);

        [JsonIgnore]
        public bool IsFCDP => (EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.FCDPId);

        [JsonIgnore]
        public bool IsLF => (EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.LearnFareId);

        [JsonIgnore]
        public bool IsTmj => (EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId);

        [JsonIgnore]
        public bool IsW2 => (EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWC ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWJ ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWL ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWM ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWN ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWP ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWX ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WWZ ||
                             EnrolledProgramId == Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.WW);
    }
}
