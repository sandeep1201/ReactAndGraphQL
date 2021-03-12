using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantEnrolledProgram : BaseCommonModel, IParticipantEnrolledProgram
    {
        IParticipant IParticipantEnrolledProgram.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IEnrolledProgram IParticipantEnrolledProgram.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        IOffice IParticipantEnrolledProgram.Office
        {
            get { return Office; }
            set { Office = (Office) value; }
        }

        IRequestForAssistance IParticipantEnrolledProgram.RequestForAssistance
        {
            get { return RequestForAssistance; }
            set { RequestForAssistance = (RequestForAssistance) value; }
        }

        IEnrolledProgramStatusCode IParticipantEnrolledProgram.StatusCode
        {
            get { return EnrolledProgramStatusCode; }
            set { EnrolledProgramStatusCode = (EnrolledProgramStatusCode) value; }
        }

        IWorker IParticipantEnrolledProgram.Worker
        {
            get { return Worker; }
            set { Worker = (Worker) value; }
        }

        IWorker IParticipantEnrolledProgram.LFFEP
        {
            get => LFFEP;
            set => LFFEP = (Worker) value;
        }

        ICollection<IOfficeTransfer> IParticipantEnrolledProgram.OfficeTransfers
        {
            get { return OfficeTransfers.Cast<IOfficeTransfer>().ToList(); }

            set { OfficeTransfers = value.Cast<OfficeTransfer>().ToList(); }
        }

        ICollection<IPEPOtherInformation> IParticipantEnrolledProgram.PEPOtherInformations
        {
            get { return PEPOtherInformations.Cast<IPEPOtherInformation>().ToList(); }

            set { PEPOtherInformations = value.Cast<PEPOtherInformation>().ToList(); }
        }

        ICollection<IEmployabilityPlan> IParticipantEnrolledProgram.EmployabilityPlans
        {
            get { return EmployabilityPlans.Cast<IEmployabilityPlan>().ToList(); }

            set { EmployabilityPlans = value.Cast<EmployabilityPlan>().ToList(); }
        }

        [NotMapped]
        public bool IsDisenrolled => EnrolledProgramStatusCodeId == Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.DisenrolledId;

        [NotMapped]
        public bool IsEnrolled => EnrolledProgramStatusCodeId == Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;

        [NotMapped]
        public bool IsReferred => EnrolledProgramStatusCodeId == Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId;

        [NotMapped]
        public bool IsW2 => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsTmj => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.TmjProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsTJ => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.TjProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsCF => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.CFProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsFCDP => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.FCDPProgramCode.Trim().ToLower();

        [NotMapped]
        public bool IsLF => EnrolledProgram?.ProgramCode?.Trim().ToLower() == Wwp.Model.Interface.Constants.EnrolledProgram.LFProgramCode.Trim().ToLower();

        public bool CanTransferContractAreas(string originContractArea, string destinationContractArea)
        {
            if (string.IsNullOrWhiteSpace(originContractArea) || string.IsNullOrWhiteSpace(destinationContractArea))
                return true;

            // Origin - Destination - can transfer
            var availableTransferContractAreas = new Dictionary<Tuple<string, string>, bool>
                                                 {
                                                     // UMOS NE to UMOS SE Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.NEUMS, Wwp.Model.Interface.Constants.ContractArea.SEUMS), false },
                                                     // UMOS NE to UMOS UBR Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.NEUMS, Wwp.Model.Interface.Constants.ContractArea.UBR), false },
                                                     // UMOS NE to UMOS USR Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.NEUMS, Wwp.Model.Interface.Constants.ContractArea.USR), false },
                                                     // UMOS SE to UMOS NE Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.SEUMS, Wwp.Model.Interface.Constants.ContractArea.NEUMS), false },
                                                     // UMOS SE to UMOS Central Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.SEUMS, Wwp.Model.Interface.Constants.ContractArea.CUMS), false },
                                                     // UMOS SE to UMOS UBR Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.SEUMS, Wwp.Model.Interface.Constants.ContractArea.UBR), false },
                                                     // UMOS SE to UMOS USR Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.SEUMS, Wwp.Model.Interface.Constants.ContractArea.USR), false },
                                                     // UMOS Central to UMOS SouthEast Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.CUMS, Wwp.Model.Interface.Constants.ContractArea.SEUMS), false },
                                                     // UMOS Central to UMOS UBR Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.CUMS, Wwp.Model.Interface.Constants.ContractArea.UBR), false },
                                                     // UMOS Central to UMOS USR Not Allowed.
                                                     { new Tuple<string, string>(Wwp.Model.Interface.Constants.ContractArea.CUMS, Wwp.Model.Interface.Constants.ContractArea.USR), false },
                                                     // UMOS UBR to UMOS NE Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.UBR, Wwp.Model.Interface.Constants.ContractArea.NEUMS), false },
                                                     // UMOS UBR to UMOS SE Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.UBR, Wwp.Model.Interface.Constants.ContractArea.SEUMS), false },
                                                     // UMOS UBR to UMOS Central Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.UBR, Wwp.Model.Interface.Constants.ContractArea.CUMS), false },
                                                     // UMOS UBR to UMOS USR Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.UBR, Wwp.Model.Interface.Constants.ContractArea.USR), false },
                                                     // UMOS USR to UMOS NE Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.USR, Wwp.Model.Interface.Constants.ContractArea.NEUMS), false },
                                                     // UMOS USR to UMOS SE Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.USR, Wwp.Model.Interface.Constants.ContractArea.SEUMS), false },
                                                     // UMOS USR to UMOS Central Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.USR, Wwp.Model.Interface.Constants.ContractArea.CUMS), false },
                                                     // UMOS USR to UMOS UBR Not Allowed.
                                                     { new Tuple<string, string >(Wwp.Model.Interface.Constants.ContractArea.USR, Wwp.Model.Interface.Constants.ContractArea.UBR), false }
                                                 };

            var transfer = new Tuple<string, string>(originContractArea, destinationContractArea);

            return !availableTransferContractAreas.ContainsKey(transfer);
        }

        // Balance of the state is any office that is not in Milwaukee.
        [NotMapped]
        public bool IsInBalanceOfState => !IsInMilwaukee;

        [NotMapped]
        public bool IsInMilwaukee => Office.CountyandTribeId == Wwp.Model.Interface.Constants.County.Milwaukee;
    }
}
