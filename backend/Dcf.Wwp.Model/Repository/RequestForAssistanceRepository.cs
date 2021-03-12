using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.ConnectedServices.GoogleApi;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    // ReSharper disable once RedundantExtendsListEntry
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Repository : IRequestForAssistanceRepository
    {
        public IRequestForAssistance GetRfa(string pin, int rfaId)
        {
            var dPin = decimal.Parse(pin);

            var rfa = _db.RequestsForAssistance.FirstOrDefault(i => i.Participant.PinNumber == dPin &&
                                                                    !i.Participant.IsDeleted        &&
                                                                    i.Id == rfaId                   && !i.IsDeleted);

            return rfa;
        }

        public ICFRfaDetail GetCfRfaDetail(int rfaId)
        {
            var cfRfaDetail = _db.CFRfaDetails.FirstOrDefault(i => i.RequestForAssistanceId == rfaId);

            return cfRfaDetail;
        }

        public ITJTMJRfaDetail GetTjTmjRfaDetail(int rfaId)
        {
            var tjtmjRfaDetail = _db.TJTMJRfaDetails.FirstOrDefault(i => i.RequestForAssistanceId == rfaId);

            return tjtmjRfaDetail;
        }

        public IFCDPRfaDetail GetFcdpRfaDetail(int rfaId)
        {
            var fcdpRfaDetail = _db.FCDPRfaDetails.FirstOrDefault(i => i.RequestForAssistanceId == rfaId);

            return fcdpRfaDetail;
        }

        public IEnumerable<IRequestForAssistance> GetRfasForPin(string pin)
        {
            var dPin = decimal.Parse(pin);

            var rfas = _db.RequestsForAssistance.Where(i => i.Participant.PinNumber == dPin).ToList();

            return rfas;
        }

        public IEnumerable<ISP_DB2_RFAs_Result> GetOldRfasForPin(string pin)
        {
            var dPin    = decimal.Parse(pin);
            var oldRfas = _db.SP_DB2_RFAs(dPin, Database);

            return oldRfas;
        }

        public bool SaveRfa(int rfaId, int progId, int countyOfResId, int cfCountyId, DateTime cfEffDate, int cfOfficeId, string userId)
        {
            var result = false;

            var rfa = _db.RequestsForAssistance.FirstOrDefault(i => i.Id == rfaId);

            if (rfa == null)
            {
                rfa = new RequestForAssistance
                      {
                          ParticipantId                = 11021,
                          RequestForAssistanceStatusId = 2,
                          EnrolledProgramId            = progId,
                          ModifiedBy                   = userId,
                          ModifiedDate                 = DateTime.Now
                      };
            }

            try
            {
                _db.RequestsForAssistance.Add(rfa);
                _db.SaveChanges();
                result = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                {
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }

            return (result);
        }

        public IRequestForAssistance NewRfa(IParticipant participant, string user)
        {
            var rfa = new RequestForAssistance
                      {
                          ParticipantId                  = participant.Id,                                            // Participant will always exist at this point.
                          RequestForAssistanceStatusId   = Interface.Constants.RequestForAssistanceStatus.InProgress, // By default, new RFA's are set to In Progress
                          RequestForAssistanceStatusDate = _authUser.CDODate ?? DateTime.Now,
                          ModifiedDate                   = DateTime.Now,
                          ModifiedBy                     = user
                      };

            _db.RequestsForAssistance.Add(rfa);

            return rfa;
        }

        public ICFRfaDetail NewCfRfaDetail(IRequestForAssistance rfa, string user, DateTime updateDate)
        {
            var cfRfaDetail = new CFRfaDetail
                              {
                                  RequestForAssistanceId = rfa.Id,
                                  ModifiedDate           = updateDate,
                                  ModifiedBy             = user
                              };

            _db.CFRfaDetails.Add(cfRfaDetail);

            return cfRfaDetail;
        }

        public ITJTMJRfaDetail NewTjTmjRfaDetail(IRequestForAssistance rfa, string user, DateTime updateDate)
        {
            var tjtmjRfaDetail = new TJTMJRfaDetail
                                 {
                                     RequestForAssistanceId = rfa.Id,
                                     ModifiedDate           = updateDate,
                                     ModifiedBy             = user
                                 };

            _db.TJTMJRfaDetails.Add(tjtmjRfaDetail);

            return tjtmjRfaDetail;
        }

        public IFCDPRfaDetail NewFcdpRfaDetail(IRequestForAssistance rfa, string user, DateTime updateDate)
        {
            var fcdpRfaDetail = new FCDPRfaDetail
                                {
                                    RequestForAssistanceId = rfa.Id,
                                    ModifiedDate           = updateDate,
                                    ModifiedBy             = user
                                };

            _db.FCDPRfaDetails.Add(fcdpRfaDetail);

            return fcdpRfaDetail;
        }

        public IRequestForAssistanceChild NewRequestForAssistanceChild(IRequestForAssistance parentRfa, DateTime date, string user)
        {
            var rfa = new RequestForAssistanceChild();

            rfa.RequestForAssistance = parentRfa as RequestForAssistance;
            rfa.ModifiedDate         = date;
            rfa.ModifiedBy           = user;

            _db.RequestForAssistanceChilds.Add(rfa);

            return rfa;
        }

        public bool HasAnyRfasInStatus(decimal pin, string programCode, string[] rfaStatuses)
        {
            var r = _db.RequestsForAssistance
                       .Any(i =>
                                i.Participant.PinNumber       == pin &&
                                i.EnrolledProgram.ProgramCode == programCode
                                && rfaStatuses.Contains(i.RequestForAssistanceStatus.Name));

            return r;
        }

        public bool HasAnyActiveProgramRfas(decimal pin, string programCode)
        {
            // TODO: Rework - this is just to get the demo going

            var rfaStatuses = new List<int> { 1, 2, 3 };

            var r = _db.RequestsForAssistance
                       .Any(i =>
                                i.Participant.PinNumber       == pin &&
                                i.EnrolledProgram.ProgramCode == programCode
                                && rfaStatuses.Contains(i.RequestForAssistanceStatusId));

            return r;
        }

        public DateTime? AddBusinessDays(DateTime? fromDate, int daysForward = 10)
        {
            if (!fromDate.HasValue)
            {
                return null;
            }

            return _db.USP_GetComputedBusniessDays(fromDate, daysForward).FirstOrDefault();
        }

        public decimal GenerateRFANumberFromDB2(IRequestForAssistance rfa, string mainframeUserId)
        {
            if (rfa.Participant?.PinNumber == null)
            {
                throw new ArgumentNullException("Participant or PinNumber cannot be null", "rfa.Participant");
            }

            var cfRfaDetail = rfa.CFRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId == rfa.Id);

            var             countyOfResidence    = GetCountyOrTribe(i => i.Id == rfa.CountyOfResidenceId);
            ICountyAndTribe cfCourtOrderedCounty = null;
            if (cfRfaDetail != null)
                cfCourtOrderedCounty = GetCountyOrTribe(i => i.Id == cfRfaDetail.CourtOrderedCountyId);

            // These all have to be strings, in this format, with these default values for DB2
            var ep              = WhereEnrolledPrograms(i => i.Id == rfa.EnrolledProgramId).FirstOrDefault();
            var enrolledProgram = (null                       != ep ? ep.ProgramCode.Substring(0, 2) : "  ");
            var pin             = (rfa.Participant?.PinNumber != null ? $"{rfa.Participant?.PinNumber:00}" : " ");
            var firstName       = rfa.Participant?.FirstName         ?? " ";
            var lastName        = rfa.Participant?.LastName          ?? " ";
            var midInitial      = rfa.Participant?.MiddleInitialName ?? " ";
            var suffix          = rfa.Participant?.SuffixName        ?? " ";

            var countyOfResidenceNumber = "00";
            var courtOrderCountyNumber  = "00";

            if (enrolledProgram == Interface.Constants.EnrolledProgram.CFProgramCode) // and it's one or the other
            {
                courtOrderCountyNumber = (cfCourtOrderedCounty?.CountyNumber ?? 0).ToString("00");
            }

            countyOfResidenceNumber = (countyOfResidence.CountyNumber ?? 0).ToString("00"); // pass the county of Res. for now because we don't have the Office yet.

            var courtOrdereffectiveDate = (cfRfaDetail?.CourtOrderEffectiveDate != null ? $"{cfRfaDetail?.CourtOrderEffectiveDate:yyyy-MM-dd}" : "9999-12-31");
            var cdoDate                 = _authUser.CDODate != null ? _authUser.CDODate.Value.ToString("yyyy-MM-dd-HH\\.mm\\.ss.fffffff") : DateTime.Now.ToString("yyyy-MM-dd-HH\\.mm\\.ss.fffffff");
            var RFATimestamp            = cdoDate;
            var appStatusReasonCd       = " ";
            var RFAStatusChangeDate     = "9999-12-31";
            var inputRFAnumber          = "0";
            var languageIndicator       = (rfa.Participant?.OtherDemographics?.FirstOrDefault()?.Language?.MFLanguageCode) ?? " ";

            //---------------------
            // GoogleAddress code

            var    streetNum  = string.Empty;
            var    streetName = string.Empty;
            string addressLine2  ;
            string cityAddress   ;
            string stateAddress  ;
            string zipAddress    ;
            string phoneNumber   ;

            var pa = rfa.Participant?.ParticipantContactInfoes.FirstOrDefault();

            //Get AlternateMailingAddress if IsHouseHoldMailingAddressSame = false or HomelessIndicator = true
            if (((pa?.IsHouseHoldMailingAddressSame != null && pa.IsHouseHoldMailingAddressSame == false) || pa?.HomelessIndicator == true) && pa.AlternateMailingAddress != null)
            {
                var space = string.IsNullOrEmpty(pa.AlternateMailingAddress?.AddressLine2) ? "" : " ";
                addressLine2 = $"{pa.AlternateMailingAddress?.AddressLine1}{space}{pa.AlternateMailingAddress?.AddressLine2}";
                cityAddress  = pa.AlternateMailingAddress?.City?.Name?.PadRight(50, ' ').Substring(0, 50).Trim();
                stateAddress = pa.AlternateMailingAddress?.City?.State?.Code?.PadRight(50, ' ').Substring(0, 50).Trim();
                zipAddress   = pa.AlternateMailingAddress?.ZipCode?.PadRight(10, ' ').Substring(0, 10).Trim();
                phoneNumber  = (pa.PrimaryPhoneNumber != null) ? pa.PrimaryPhoneNumber?.PadRight(10, ' ').Substring(0, 10).Trim() : " ";
            }
            else
            {
                var space = string.IsNullOrEmpty(pa?.AddressLine2) ? "" : " ";
                addressLine2 = $"{pa?.AddressLine1}{space}{pa?.AddressLine2}";
                cityAddress  = (pa?.City?.Name?.PadRight(50, ' ').Substring(0, 50).Trim())        ?? " ";
                stateAddress = (pa?.City?.State?.Code?.PadRight(50, ' ').Substring(0, 50).Trim()) ?? " ";
                zipAddress   = (pa?.ZipCode?.PadRight(10, ' ').Substring(0, 10).Trim())           ?? " ";
                phoneNumber  = (pa?.PrimaryPhoneNumber != null) ? pa.PrimaryPhoneNumber?.PadRight(10, ' ').Substring(0, 10).Trim() : " ";
            }

            var db2Res = _db.SP_DB2_CreateRFA(
                                              Database,
                                              pin,
                                              enrolledProgram,
                                              "A",
                                              firstName,
                                              lastName,
                                              midInitial,
                                              suffix,
                                              languageIndicator,
                                              countyOfResidenceNumber,
                                              courtOrderCountyNumber,
                                              courtOrdereffectiveDate,
                                              RFATimestamp,
                                              streetNum,
                                              streetName,
                                              addressLine2,
                                              cityAddress,
                                              stateAddress,
                                              zipAddress,
                                              phoneNumber,
                                              appStatusReasonCd,
                                              RFAStatusChangeDate,
                                              mainframeUserId,
                                              inputRFAnumber
                                             );

            var t = (db2Res.ToList()).FirstOrDefault();
            decimal.TryParse(t.RFANumber, out var r);

            return (r);
        }

        public void DenyRFAInDB2(IRequestForAssistance rfa, string mainframeUserId)
        {
            if (rfa.Participant == null || rfa.Participant.PinNumber == null)
            {
                throw new ArgumentNullException("Participant or PinNumber cannot be null", "rfa.Participant");
            }

            if (rfa.RfaNumber == null)
            {
                throw new ArgumentNullException("RFANumber cannot be null", "rfa.RfaNumber");
            }

            // These all have to be strings, in this format, with these default values for DB2
            var pin     = (rfa.Participant?.PinNumber != null ? $"{rfa.Participant?.PinNumber:00}" : " ");
            var cdoDate = _authUser.CDODate != null ? _authUser.CDODate.Value.ToString("yyyy-MM-dd-HH\\.mm\\.ss.fffffff") : DateTime.Now.ToString("yyyy-MM-dd-HH\\.mm\\.ss.fffffff");

            _db.SP_DB2_CreateRFA(
                                 Database, pin, " ", "D", " ", " ", " ", " ", " ", "0", "0", "9999-12-31", cdoDate,
                                 " ", " ", " ", " ", " ", " ", " ", "P00", cdoDate, mainframeUserId,
                                 rfa.RfaNumber.ToString()
                                );
        }

        public void WriteBackReferralToDb2(IRequestForAssistance rfa, DateTime effectiveDate, string userId)
        {
            var pin         = rfa.Participant.PinNumber;
            var programCode = rfa.EnrolledProgram.ProgramCode;

            var referralRegCode = GetReferralRegCode(pin, programCode);
            var cfRfaDetail     = rfa.CFRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId    == rfa.Id);
            var tjtmjRfaDetail  = rfa.TJTMJRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId == rfa.Id);

            _db.SP_DB2_Referral_Update(
                                       pin,
                                       rfa.RfaNumber,
                                       effectiveDate,
                                       rfa.Office?.CountyAndTribe?.CountyNumber,
                                       (short?) rfa.Office?.OfficeNumber,
                                       cfRfaDetail?.CountyAndTribe?.CountyNumber,
                                       cfRfaDetail?.CourtOrderEffectiveDate,
                                       userId,
                                       programCode,
                                       rfa.EnrolledProgram.SubProgramCode ?? " ",
                                       referralRegCode,
                                       Database,
                                       tjtmjRfaDetail?.ApplicationDate
                                      );
        }

        public string GetReferralRegCode(decimal? pin, string programCode)
        {
            var recentStatus      = new ObjectParameter("RecentStatus",      typeof(string));
            var referralDate      = new ObjectParameter("ReferralDate",      typeof(DateTime));
            var enrollmentDate    = new ObjectParameter("EnrollmentDate",    typeof(DateTime));
            var disEnrollmemtDate = new ObjectParameter("DisEnrollmemtDate", typeof(DateTime));
            var enrolledProgramId = new ObjectParameter("EnrolledProgramId", typeof(int));
            var mostRecentPep = (IEnumerable<IUSP_ProgramStatus_Recent_Result>) _db.USP_ProgramStatus_Recent(
                                                                                                             pin,
                                                                                                             Database,
                                                                                                             true,
                                                                                                             null,
                                                                                                             recentStatus,
                                                                                                             referralDate,
                                                                                                             enrollmentDate,
                                                                                                             disEnrollmemtDate,
                                                                                                             enrolledProgramId
                                                                                                            ).ToList();

            string referralRegCode;

            if (programCode.Trim() == "CF")
            {
                var isW2CfCoEnrolled = mostRecentPep?.Any(x => x.ProgramName.Trim() == "WW" && (x.RecentStatus == "Referred" || x.RecentStatus == "Enrolled"));

                if (isW2CfCoEnrolled == true)
                {
                    referralRegCode = mostRecentPep?.Where(x => x.ProgramName.Trim() == "WW").Select(y => y.ReferralRegistrationCode).FirstOrDefault();
                }
                else
                {
                    referralRegCode = "V";
                }
            }
            else
            {
                referralRegCode = "V";
            }

            return referralRegCode;
        }
    }
}
