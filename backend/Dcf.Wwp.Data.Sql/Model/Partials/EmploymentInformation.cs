using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentInformation : BaseCommonModel, IEmploymentInformation, IEquatable<EmploymentInformation>
    {
        ICollection<IAbsence> IEmploymentInformation.Absences
        {
            get { return Absences.Where(i => i.IsDeleted == false).Select(i => i).Cast<IAbsence>().ToList(); }
            set => Absences = value.Cast<Absence>().ToList();
        }

        ICollection<IAbsence> IEmploymentInformation.AllAbsences
        {
            get { return Absences.Cast<IAbsence>().ToList(); }
            set { Absences = value.Cast<Absence>().ToList(); }
        }

        ICity IEmploymentInformation.City
        {
            get { return City; }
            set { City = (City) value; }
        }

        IContact IEmploymentInformation.Contact
        {
            get { return Contact; }
            set { Contact = (Contact) value; }
        }

        IJobType IEmploymentInformation.JobType
        {
            get { return JobType; }
            set { JobType = (JobType) value; }
        }

        ILeavingReason IEmploymentInformation.LeavingReason
        {
            get { return LeavingReason; }
            set { LeavingReason = (LeavingReason) value; }
        }

        IOtherJobInformation IEmploymentInformation.OtherJobInformation
        {
            get { return OtherJobInformation; }
            set { OtherJobInformation = (OtherJobInformation) value; }
        }

        IParticipant IEmploymentInformation.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IWageHour IEmploymentInformation.WageHour
        {
            get { return WageHour; }
            set { WageHour = (WageHour) value; }
        }

        IWorkHistorySection IEmploymentInformation.WorkHistorySection
        {
            get { return WorkHistorySection; }
            set { WorkHistorySection = (WorkHistorySection) value; }
        }

        IEmploymentProgramType IEmploymentInformation.EmploymentProgramType
        {
            get { return EmploymentProgramType; }
            set { EmploymentProgramType = (EmploymentProgramType) value; }
        }

        ICollection<IEmploymentInformationBenefitsOfferedTypeBridge> IEmploymentInformation.EmploymentInformationBenefitsOfferedTypeBridges
        {
            get { return (from x in EmploymentInformationBenefitsOfferedTypeBridges where x.IsDeleted == false select x).Cast<IEmploymentInformationBenefitsOfferedTypeBridge>().ToList(); }
            set { EmploymentInformationBenefitsOfferedTypeBridges = value.Cast<EmploymentInformationBenefitsOfferedTypeBridge>().ToList(); }
        }

        ICollection<IEmploymentInformationBenefitsOfferedTypeBridge> IEmploymentInformation.AllEmploymentInformationBenefitsOfferedTypeBridges
        {
            get { return EmploymentInformationBenefitsOfferedTypeBridges.Cast<IEmploymentInformationBenefitsOfferedTypeBridge>().ToList(); }
            set { EmploymentInformationBenefitsOfferedTypeBridges = value.Cast<EmploymentInformationBenefitsOfferedTypeBridge>().ToList(); }
        }

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }
            set { DeleteReason = (DeleteReason) value; }
        }

        ICollection<IEmploymentInformationJobDutiesDetailsBridge> IEmploymentInformation.EmploymentInformationJobDutiesDetailsBridges
        {
            get { return (from x in EmploymentInformationJobDutiesDetailsBridges where x.IsDeleted == false select x).Cast<IEmploymentInformationJobDutiesDetailsBridge>().ToList(); }
            set { EmploymentInformationJobDutiesDetailsBridges = value.Cast<EmploymentInformationJobDutiesDetailsBridge>().ToList(); }
        }

        ICollection<IEmploymentInformationJobDutiesDetailsBridge> IEmploymentInformation.AllEmploymentInformationJobDutiesDetailsBridges
        {
            get { return EmploymentInformationJobDutiesDetailsBridges.Cast<IEmploymentInformationJobDutiesDetailsBridge>().ToList(); }
            set { EmploymentInformationJobDutiesDetailsBridges = value.Cast<EmploymentInformationJobDutiesDetailsBridge>().ToList(); }
        }

        IEmployerOfRecordType IEmploymentInformation.EmployerOfRecordType
        {
            get { return EmployerOfRecordType; }
            set { EmployerOfRecordType = (EmployerOfRecordType) value; }
        }

        ICollection<IEmployerOfRecordInformation> IEmploymentInformation.EmployerOfRecordInformations
        {
            get { return EmployerOfRecordInformations.Cast<IEmployerOfRecordInformation>().ToList(); }
            set { EmployerOfRecordInformations = value.Cast<EmployerOfRecordInformation>().ToList(); }
        }

        ICollection<IEPEIBridge> IEmploymentInformation.EPEIBridges
        {
            get { return EPEIBridges.Where(i => i.IsDeleted == false).Select(i => i).Cast<IEPEIBridge>().ToList(); }
            set => EPEIBridges = value.Cast<EPEIBridge>().ToList();
        }

        ICollection<IWeeklyHoursWorked> IEmploymentInformation.WeeklyHoursWorkedEntries
        {
            get { return WeeklyHoursWorkedEntries.Where(i => i.IsDeleted == false).Select(i => i).Cast<IWeeklyHoursWorked>().ToList(); }
            set => WeeklyHoursWorkedEntries = value.Cast<WeeklyHoursWorked>().ToList();
        }

        ICollection<IEmploymentVerification> IEmploymentInformation.EmploymentVerifications
        {
            get { return EmploymentVerifications.Where(i => i.IsDeleted == false).Select(i => i).Cast<IEmploymentVerification>().ToList(); }
            set => EmploymentVerifications = value.Cast<EmploymentVerification>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var em = new EmploymentInformation();

            em.Id                                              = this.Id;
            em.ParticipantId                                   = this.ParticipantId;
            em.WorkHistorySectionId                            = this.WorkHistorySectionId;
            em.JobTypeId                                       = this.JobTypeId;
            em.Notes                                           = this.Notes;
            em.EmploymentProgramtypeId                         = this.EmploymentProgramtypeId;
            em.JobBeginDate                                    = this.JobBeginDate;
            em.JobEndDate                                      = this.JobEndDate;
            em.JobPosition                                     = this.JobPosition;
            em.CompanyName                                     = this.CompanyName;
            em.Fein                                            = this.Fein;
            em.StreetAddress                                   = this.StreetAddress;
            em.IsCurrentlyEmployed                             = this.IsCurrentlyEmployed;
            em.ZipAddress                                      = this.ZipAddress;
            em.CityId                                          = this.CityId;
            em.ContactId                                       = this.ContactId;
            em.JobDutiesId                                     = this.JobDutiesId;
            em.LeavingReasonId                                 = this.LeavingReasonId;
            em.LeavingReasonDetails                            = this.LeavingReasonDetails;
            em.OtherJobInformationId                           = this.OtherJobInformationId;
            em.WageHoursId                                     = this.WageHoursId;
            em.Notes                                           = this.Notes;
            em.City                                            = (City) this.City?.Clone();
            em.Absences                                        = this.Absences.Select(x => (Absence) x.Clone()).ToList();
            em.JobType                                         = (JobType) this.JobType?.Clone();
            em.LeavingReason                                   = (LeavingReason) this.LeavingReason?.Clone();
            em.OtherJobInformation                             = (OtherJobInformation) this.OtherJobInformation?.Clone();
            em.WageHour                                        = (WageHour) this.WageHour?.Clone();
            em.EmploymentProgramType                           = (EmploymentProgramType) this.EmploymentProgramType?.Clone();
            em.EmploymentInformationBenefitsOfferedTypeBridges = this.EmploymentInformationBenefitsOfferedTypeBridges?.Select(x => (EmploymentInformationBenefitsOfferedTypeBridge) x.Clone()).ToList();
            em.EmploymentInformationJobDutiesDetailsBridges    = this.EmploymentInformationJobDutiesDetailsBridges?.Select(x => (EmploymentInformationJobDutiesDetailsBridge) x.Clone()).ToList();
            em.EmployerOfRecordInformations                    = this.EmployerOfRecordInformations?.Select(x => (EmployerOfRecordInformation) x.Clone()).ToList();
            em.DeleteReasonId                                  = this.DeleteReasonId;
            em.IsConverted                                     = this.IsConverted;
            em.EmploymentSequenceNumber                        = this.EmploymentSequenceNumber;
            em.OriginalOfficeNumber                            = this.OriginalOfficeNumber;
            em.Contact                                         = (Contact) this.Contact?.Clone();
            em.EmployerOfRecordTypeId                          = this.EmployerOfRecordTypeId;
            em.ModifiedBy                                      = this.ModifiedBy;
            em.ModifiedDate                                    = this.ModifiedDate;
            em.IsCurrentJobAtCreation                          = this.IsCurrentJobAtCreation;
            em.WeeklyHoursWorkedEntries                        = this.WeeklyHoursWorkedEntries?.Select(x => (WeeklyHoursWorked) x.Clone()).ToList();
            em.EmploymentVerifications                         = this.EmploymentVerifications?.Select(x => (EmploymentVerification) x.Clone()).ToList();

            return em;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmploymentInformation;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmploymentInformation other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal

            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(WorkHistorySectionId, other.WorkHistorySectionId))
                return false;
            if (!AdvEqual(JobTypeId, other.JobTypeId))
                return false;
            if (!AdvEqual(EmploymentProgramtypeId, other.EmploymentProgramtypeId))
                return false;
            if (!AdvEqual(JobBeginDate, other.JobBeginDate))
                return false;
            if (!AdvEqual(JobEndDate, other.JobEndDate))
                return false;
            if (!AdvEqual(IsCurrentlyEmployed, other.IsCurrentlyEmployed))
                return false;
            if (!AdvEqual(JobPosition, other.JobPosition))
                return false;
            if (!AdvEqual(CompanyName, other.CompanyName))
                return false;
            if (!AdvEqual(Fein, other.Fein))
                return false;
            if (!AdvEqual(StreetAddress, other.StreetAddress))
                return false;
            if (!AdvEqual(CityId, other.CityId))
                return false;
            if (!AdvEqual(ContactId, other.ContactId))
                return false;
            if (!AdvEqual(JobDutiesId, other.JobDutiesId))
                return false;
            if (!AdvEqual(LeavingReasonId, other.LeavingReasonId))
                return false;
            if (!AdvEqual(OtherJobInformationId, other.OtherJobInformationId))
                return false;
            if (!AdvEqual(WageHoursId, other.WageHoursId))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(LeavingReasonDetails, other.LeavingReasonDetails))
                return false;
            if (AreBothNotNull(Absences, other.Absences) && !(Absences.OrderBy(x => x.Id).SequenceEqual(other.Absences.OrderBy(x => x.Id))))
                return false;
            if (!AdvEqual(WorkHistorySection, other.WorkHistorySection))
                return false;
            if (!AdvEqual(JobType, other.JobType))
                return false;
            if (!AdvEqual(WageHour, other.WageHour))
                return false;
            if (!AdvEqual(City, other.City))
                return false;
            if (!AdvEqual(Contact, other.Contact))
                return false;
            if (!AdvEqual(LeavingReason, other.LeavingReason))
                return false;
            if (!AdvEqual(OtherJobInformation, other.OtherJobInformation))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;
            if (!AdvEqual(EmployerOfRecordTypeId, other.EmployerOfRecordTypeId))
                return false;
            if (!AdvEqual(EmployerOfRecordType, other.EmployerOfRecordType))
                return false;
            if (!AdvEqual(EmploymentInformationBenefitsOfferedTypeBridges, other.EmploymentInformationBenefitsOfferedTypeBridges) && !EmploymentInformationBenefitsOfferedTypeBridges.OrderBy(x => x.Id).SequenceEqual(other.EmploymentInformationBenefitsOfferedTypeBridges.OrderBy(x => x.Id)))
                return false;
            if (AreBothNotNull(EmploymentInformationJobDutiesDetailsBridges, other.EmploymentInformationJobDutiesDetailsBridges) && !EmploymentInformationJobDutiesDetailsBridges.OrderBy(x => x.Id).SequenceEqual(other.EmploymentInformationJobDutiesDetailsBridges.OrderBy(x => x.Id)))
                return false;
            if (!AdvEqual(IsConverted, other.IsConverted))
                return false;
            if (!AdvEqual(EmploymentSequenceNumber, other.EmploymentSequenceNumber))
                return false;
            if (!AdvEqual(OriginalOfficeNumber, other.OriginalOfficeNumber))
                return false;
            if (!AdvEqual(IsCurrentJobAtCreation, other.IsCurrentJobAtCreation))
                return false;
            if (!AdvEqual(WeeklyHoursWorkedEntries, other.WeeklyHoursWorkedEntries))
                return false;
            if (!AdvEqual(EmploymentVerifications, other.EmploymentVerifications) && !EmploymentVerifications.OrderBy(i => i.Id).SequenceEqual(other.EmploymentVerifications.OrderBy(i => i.Id)))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
