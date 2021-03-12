using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlan : BaseCommonModel, IEmployabilityPlan, IEquatable<EmployabilityPlan>
    {
        ICollection<IEmployabilityPlanActivityBridge> IEmployabilityPlan.EmployabilityPlanActivityBridges
        {
            get => EmployabilityPlanActivityBridges.Cast<IEmployabilityPlanActivityBridge>().ToList();
            set => EmployabilityPlanActivityBridges = (ICollection<EmployabilityPlanActivityBridge>) value;
        }

        IEnrolledProgram IEmployabilityPlan.EnrolledProgram
        {
            get => EnrolledProgram;
            set => EnrolledProgram = (EnrolledProgram) value;
        }

        IParticipant IEmployabilityPlan.Participant
        {
            get => Participant;
            set => Participant = (Participant) value;
        }

        IParticipantEnrolledProgram IEmployabilityPlan.ParticipantEnrolledProgram
        {
            get => ParticipantEnrolledProgram;
            set => ParticipantEnrolledProgram = (ParticipantEnrolledProgram) value;
        }

        ICollection<IEmployabilityPlanGoalBridge> IEmployabilityPlan.EmployabilityPlanGoalBridges
        {
            get => EmployabilityPlanGoalBridges.Cast<IEmployabilityPlanGoalBridge>().ToList();
            set => EmployabilityPlanGoalBridges = (ICollection<EmployabilityPlanGoalBridge>) value;
        }

        //ICollection<IGoal> IEmployabilityPlan.Goals
        //{
        //    get { return Goals.Cast<IGoal>().ToList(); }
        //    set { Goals = (ICollection<Goal>) value; }
        //}

        ICollection<ISupportiveService> IEmployabilityPlan.SupportiveServices
        {
            get => SupportiveServices.Cast<ISupportiveService>().ToList();
            set => SupportiveServices = (ICollection<SupportiveService>) value;
        }

        ICollection<IEPEIBridge> IEmployabilityPlan.EPEIBridges
        {
            get { return EPEIBridges.Cast<IEPEIBridge>().ToList(); }
            set { EPEIBridges = (ICollection<EPEIBridge>)value; }
        }

        IOrganization IEmployabilityPlan.Organization
        {
            get { return Organization; }
            set { Organization = (Organization)value; }
        }

        IEmployabilityPlanStatusType IEmployabilityPlan.EmployabilityPlanStatusType
        {
            get { return EmployabilityPlanStatusType; }
            set { EmployabilityPlanStatusType = (EmployabilityPlanStatusType)value; }
        }

        #region ICloneable

        public object Clone()
        {
            var ep = new EmployabilityPlan();

            ep.Id                            = Id;
            ep.ParticipantId                 = ParticipantId;
            ep.EnrolledProgramId             = EnrolledProgramId;
            ep.BeginDate                     = BeginDate;
            ep.EndDate                       = EndDate;
            ep.IsDeleted                     = IsDeleted;
            ep.CreatedDate                   = CreatedDate;
            ep.Notes                         = Notes;
            ep.ParticipantEnrolledProgramId  = ParticipantEnrolledProgramId;
            ep.CanSaveWithoutActivity        = CanSaveWithoutActivity;
            ep.CanSaveWithoutActivityDetails = CanSaveWithoutActivityDetails;

            return ep;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as EmployabilityPlan;

            return obj != null && Equals(obj);
        }

        public bool Equals(EmployabilityPlan other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(ParticipantId, other.ParticipantId))
            {
                return false;
            }

            if (!AdvEqual(EnrolledProgramId, other.EnrolledProgramId))
            {
                return false;
            }

            if (!AdvEqual(BeginDate, other.BeginDate))
            {
                return false;
            }

            if (!AdvEqual(EndDate, other.EndDate))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            if (!AdvEqual(CreatedDate, other.CreatedDate))
            {
                return false;
            }

            if (!AdvEqual(Notes, other.Notes))
            {
                return false;
            }

            if (!AdvEqual(ParticipantEnrolledProgramId, other.ParticipantEnrolledProgramId))
            {
                return false;
            }

            if (!AdvEqual(CanSaveWithoutActivity, other.CanSaveWithoutActivity))
            {
                return false;
            }

            if (!AdvEqual(CanSaveWithoutActivityDetails, other.CanSaveWithoutActivityDetails))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
