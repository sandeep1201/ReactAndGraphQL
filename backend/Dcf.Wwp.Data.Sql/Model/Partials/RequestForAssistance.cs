using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistance : BaseCommonModel, IRequestForAssistance
    {
        ICountyAndTribe IRequestForAssistance.CountyOfResidence
        {
            get { return CountyOfResidence; }
            set { CountyOfResidence = (CountyAndTribe) value; }
        }

        IEnrolledProgram IRequestForAssistance.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        IRequestForAssistanceStatus IRequestForAssistance.RequestForAssistanceStatus
        {
            get { return RequestForAssistanceStatus; }
            set { RequestForAssistanceStatus = (RequestForAssistanceStatus) value; }
        }

        IParticipant IRequestForAssistance.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IOffice IRequestForAssistance.Office
        {
            get { return Office; }
            set { Office = (Office) value; }
        }

        public bool IsTMJ => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.TmjProgramCode.Trim().ToLower();

        public bool IsTJ => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.TjProgramCode.Trim().ToLower();

        public bool IsCF   => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.CFProgramCode.Trim().ToLower();
        public bool IsFCDP => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.FCDPProgramCode.Trim().ToLower();

        public bool IsEnrolled => RequestForAssistanceStatusId == Wwp.Model.Interface.Constants.RequestForAssistanceStatus.Enrolled;

        public bool IsReferred => RequestForAssistanceStatusId == Wwp.Model.Interface.Constants.RequestForAssistanceStatus.Referred;

        public bool IsInProgress => RequestForAssistanceStatusId == Wwp.Model.Interface.Constants.RequestForAssistanceStatus.InProgress;

        ICollection<IRequestForAssistanceChild> IRequestForAssistance.RequestForAssistanceChilds
        {
            get { return RequestForAssistanceChilds.Where(x => !x.IsDeleted).Cast<IRequestForAssistanceChild>().ToList(); }
            set { RequestForAssistanceChilds = value.Cast<RequestForAssistanceChild>().ToList(); }
        }

        ICollection<IRequestForAssistanceChild> IRequestForAssistance.AllRequestForAssistanceChilds
        {
            get { return RequestForAssistanceChilds.Cast<IRequestForAssistanceChild>().ToList(); }
            set { RequestForAssistanceChilds = value.Cast<RequestForAssistanceChild>().ToList(); }
        }

        ICollection<IRequestForAssistancePopulationTypeBridge> IRequestForAssistance.RequestForAssistancePopulationTypeBridges
        {
            get { return RequestForAssistancePopulationTypeBridges.Cast<IRequestForAssistancePopulationTypeBridge>().ToList(); }
            set { RequestForAssistancePopulationTypeBridges = value.Cast<RequestForAssistancePopulationTypeBridge>().ToList(); }
        }

        ICollection<IParticipantEnrolledProgram> IRequestForAssistance.ParticipantEnrolledPrograms
        {
            get { return ParticipantEnrolledPrograms.Cast<IParticipantEnrolledProgram>().ToList(); }
            set { ParticipantEnrolledPrograms = value.Cast<ParticipantEnrolledProgram>().ToList(); }
        }

        ICollection<IRequestForAssistanceRuleReason> IRequestForAssistance.RequestForAssistanceRuleReasons
        {
            get { return RequestForAssistanceRuleReasons.Cast<IRequestForAssistanceRuleReason>().ToList(); }
            set { RequestForAssistanceRuleReasons = value.Cast<RequestForAssistanceRuleReason>().ToList(); }
        }

        ICollection<ICFRfaDetail> IRequestForAssistance.CFRfaDetails
        {
            get { return CFRfaDetails.Cast<ICFRfaDetail>().ToList(); }
            set { CFRfaDetails = value.Cast<CFRfaDetail>().ToList(); }
        }

        ICollection<ITJTMJRfaDetail> IRequestForAssistance.TJTMJRfaDetails
        {
            get { return TJTMJRfaDetails.Cast<ITJTMJRfaDetail>().ToList(); }
            set { TJTMJRfaDetails = value.Cast<TJTMJRfaDetail>().ToList(); }
        }

        ICollection<IFCDPRfaDetail> IRequestForAssistance.FCDPRfaDetails
        {
            get { return FCDPRfaDetails.Cast<IFCDPRfaDetail>().ToList(); }
            set { FCDPRfaDetails = value.Cast<FCDPRfaDetail>().ToList(); }
        }
    }
}
