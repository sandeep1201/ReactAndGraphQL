using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CompletionReason : BaseCommonModel, ICompletionReason
    {
        IEnrolledProgram ICompletionReason.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        ICollection<IParticipantEnrolledProgram> ICompletionReason.ParticipantEnrolledPrograms
        {
            get { return ParticipantEnrolledPrograms.Cast<IParticipantEnrolledProgram>().ToList(); }

            set { ParticipantEnrolledPrograms = value.Cast<ParticipantEnrolledProgram>().ToList(); }
        }
    }
}
