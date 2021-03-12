using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEnrolledProgramEPActivityTypeBridge : ICommonDelModel
    {
        #region Properties

        int?  EnrolledProgramId   { get; set; }
        int?  ActivityTypeId      { get; set; }
        bool? IsSelfDirected      { get; set; }
        bool? IsUpfrontActivity   { get; set; }
        bool? IsSanctionable      { get; set; }
        bool? IsAssessmentRelated { get; set; }

        #endregion

        #region Navigation Props

        IEnrolledProgram EnrolledProgram { get; set; }
        IActivityType    ActivityType    { get; set; }

        #endregion
    }
}
