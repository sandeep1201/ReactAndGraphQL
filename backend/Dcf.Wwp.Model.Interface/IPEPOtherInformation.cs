using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPEPOtherInformation : ICommonModel
    {
        #region Properties
        int Id { get; set; }
        int?  PEPId { get; set; }
        string CompletionReasonDetails { get; set; }

#endregion

        #region Navigation Properties

         IParticipantEnrolledProgram ParticipantEnrolledProgram { get; set; }

#endregion

    }
}
