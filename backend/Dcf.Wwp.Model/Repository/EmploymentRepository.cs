using System;
using System.Data;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEmploymentRepository
    {
        public IEmploymentInformation EmploymentByIdAsNoTracking(int? employmentId)
        {
            var eint = _db.EmploymentInformations.AsNoTracking().FirstOrDefault(i => i.Id == employmentId && i.DeleteReasonId == null);

            return (eint);
        }

        public IEmploymentInformation EmploymentById(int? employmentId)
        {
            var ei = _db.EmploymentInformations.FirstOrDefault(i => i.Id == employmentId && i.DeleteReasonId == null);

            return (ei);
        }

        public IEmploymentInformation NewEmploymentInfo(IParticipant participant, string user)
        {
            var seqNo = participant.AllEmploymentInformations?.Max(i => i.EmploymentSequenceNumber) + 1;
            seqNo = seqNo ?? 1;

            var mostRecentPrograms       = GetRecentPEPForPin(participant.PinNumber)?.OrderByDescending(x => x.RecentStatusDate).ToList();
            var mostRecentOfficeId       = mostRecentPrograms?.FirstOrDefault()?.OfficeId;
            var secondMostRecentOfficeId = mostRecentPrograms?.Skip(1).Take(1).FirstOrDefault()?.OfficeId;
            var fcdpOfficeIds            = _db.Offices.Where(i => i.ContractArea.EnrolledProgramId == EnrolledProgram.FCDPId).Select(i => i.Id).ToList();
            var origOfficeNumber = mostRecentOfficeId != null
                                       ? fcdpOfficeIds.Contains((int) mostRecentOfficeId)
                                             ? secondMostRecentOfficeId != null
                                                   ? GetOfficeById((int) secondMostRecentOfficeId).OfficeNumber
                                                   : 9999
                                             : GetOfficeById((int) mostRecentOfficeId).OfficeNumber
                                       : 9999;


            var employmentInfo = new EmploymentInformation
                                 {
                                     ParticipantId            = participant.Id,
                                     ModifiedDate             = DateTime.Now,
                                     ModifiedBy               = user,
                                     IsDeleted                = false,
                                     EmploymentSequenceNumber = (short) seqNo,
                                     OriginalOfficeNumber     = (short) origOfficeNumber
                                 };

            _db.EmploymentInformations.Add(employmentInfo);

            return (employmentInfo);
        }

        public IEnumerable<IUSP_ProgramStatus_Recent_Result> GetRecentPEPForPin(decimal? pin)
        {
            var recentStatus      = new ObjectParameter("RecentStatus",      typeof(string));
            var referralDate      = new ObjectParameter("ReferralDate",      typeof(DateTime));
            var enrollmentDate    = new ObjectParameter("EnrollmentDate",    typeof(DateTime));
            var disEnrollmemtDate = new ObjectParameter("DisEnrollmemtDate", typeof(DateTime));
            var enrolledProgramId = new ObjectParameter("EnrolledProgramId", typeof(int));

            var r = (IEnumerable<IUSP_ProgramStatus_Recent_Result>) _db.USP_ProgramStatus_Recent(
                                                                                                 pin,
                                                                                                 Database,
                                                                                                 false,
                                                                                                 null,
                                                                                                 recentStatus,
                                                                                                 referralDate,
                                                                                                 enrollmentDate,
                                                                                                 disEnrollmemtDate,
                                                                                                 enrolledProgramId
                                                                                                ).ToList();

            return r;
        }

        public void SP_Work_History_WriteBack(int? participantId, short? eSeqNo, string mFUserId, bool isDeletedEmployment, bool isNewEmployment, string computedDB2WageRateValue)
        {
            _db.SP_Work_History_WriteBack(Database, participantId, eSeqNo, mFUserId, isDeletedEmployment, isNewEmployment, computedDB2WageRateValue);
        }

        public void DeleteOnFailure(IEmploymentInformation employmentInformation)
        {
            //var empInfo                = _db.EmploymentInformations.FirstOrDefault(i => i.Id == employmentInformation.Id);
            //var empInfoBenefitsOffered = _db.EmploymentInformationBenefitsOfferedTypeBridges
            //                                .Where(i => i.EmploymentInformationId == employmentInformation.Id).ToList();
            //var empInfoJobDuty = _db.EmploymentInformationJobDutiesDetailsBridges
            //                        .Where(i => i.EmploymentInformationId == employmentInformation.Id).ToList();

            employmentInformation.DeleteReasonId           = 7;
            employmentInformation.EmploymentSequenceNumber = 0;

            if (employmentInformation.OtherJobInformation != null)
                employmentInformation.OtherJobInformation.IsDeleted = true;

            if (employmentInformation.WageHour != null)
                employmentInformation.WageHour.IsDeleted = true;


            foreach (var whh in employmentInformation.WageHour.WageHourHistories)
            {
                whh.IsDeleted = true;
            }

            foreach (var eibo in employmentInformation.EmploymentInformationBenefitsOfferedTypeBridges)
            {
                eibo.IsDeleted = true;
            }

            foreach (var eijd in employmentInformation.EmploymentInformationJobDutiesDetailsBridges)
            {
                eijd.IsDeleted = true;
            }

            foreach (var ab in employmentInformation.Absences)
            {
                ab.IsDeleted = true;
            }

            Save();
        }

        public bool EmploymentInfoTransactionalSave(IEmploymentInformation employmentInfo, string user, string mFUserId, string computedDB2WageRateUnit, string computedDB2WageRateValue)
        {
            using (var tx = _sqlDb.BeginTransaction())
            {
                try
                {
                    var isNewEmployment = employmentInfo.Id == 0;
                    computedDB2WageRateValue = !string.IsNullOrWhiteSpace(computedDB2WageRateUnit) && computedDB2WageRateUnit.ToLower() != "day" && computedDB2WageRateUnit.ToLower() != "irregular" && computedDB2WageRateUnit.ToLower() != "year"
                                                   ? computedDB2WageRateValue
                                                   : "0.0";
                    var success = SaveIfChanged(employmentInfo, user);
                    if (success)
                    {
                        if (employmentInfo.OriginalOfficeNumber != 9999)
                            SP_Work_History_WriteBack(employmentInfo.ParticipantId, employmentInfo.EmploymentSequenceNumber, mFUserId, false, isNewEmployment, computedDB2WageRateValue);
                    }

                    tx.Commit();
                }
                catch (DBConcurrencyException)
                {
                    return true;
                }
                catch (Exception ex)
                {
                    tx.Dispose();
                    throw new DCFApplicationException("Failed due to mainframe issue.  Please try again.", ex);
                }

                tx.Dispose();
            }

            return false;
        }

        public ISP_DB2_PreCheck_POP_Claim_Result PreCheckPop(string pin, short? seqNo) => _db.SP_DB2_PreCheck_POP_Claim(Database, pin, seqNo.ToString()).FirstOrDefault();


        public void EmploymentInfoTransactionalDelete(IEmploymentInformation employmentInfo, string mFUserId, IEPEIBridge epei)
        {
            using (var tr = _sqlDb.BeginTransaction())
            {
                try
                {
                    if (epei != null)
                        _db.EPEIBridges.Remove(epei as EPEIBridge);
                    Save();

                    if (employmentInfo.OriginalOfficeNumber != 9999)
                        SP_Work_History_WriteBack(employmentInfo.ParticipantId, employmentInfo.EmploymentSequenceNumber, mFUserId, true, false, "");
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Dispose();
                    throw new DCFApplicationException("Failed due to mainframe issue.  Please try again.", ex);
                }
            }
        }
    }
}
