using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PEPOtherInformation : BaseCommonModel, IPEPOtherInformation
    {
        IParticipantEnrolledProgram IPEPOtherInformation.ParticipantEnrolledProgram
        {
            get { return ParticipantEnrolledProgram; }
            set { ParticipantEnrolledProgram = (ParticipantEnrolledProgram) value; }
        }
    }
}
