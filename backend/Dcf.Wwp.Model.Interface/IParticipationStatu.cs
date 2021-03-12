using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipationStatu : ICommonModelFinal, ICloneable
    {
        int Id { get; set; }
        int? ParticipantId { get; set; }
        int? StatusId { get; set; }
        DateTime? BeginDate { get; set; }
        DateTime? EndDate { get; set; }
        string Details { get; set; }
        bool? IsCurrent { get; set; }
         int? EnrolledProgramId { get; set; }

        IEnrolledProgram EnrolledProgram { get; set; }

        IParticipant Participant { get; set; }

        IParticipationStatusType ParticipationStatusType { get; set; }
    }
}
