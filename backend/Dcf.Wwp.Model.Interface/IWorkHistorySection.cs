using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWorkHistorySection : ICloneable, ICommonModel
    {
        int?   EmploymentStatusTypeId   { get; set; }
        string NonFullTimeDetails       { get; set; }
        string PreventionFactors        { get; set; }
        bool?  HasVolunteered           { get; set; }
        string Notes                    { get; set; }
        int?   HasCareerAssessment      { get; set; }
        string HasCareerAssessmentNotes { get; set; }

        // Navigation Properties from the EDMX that we need.
        // Commented out ones are those we don't need.
        //ICollection<IEmploymentWorkHistoryBridge> EmploymentPreventionWorkHistoryBridges { get; set; }
        //ICollection<IEmploymentWorkHistoryBridge> AllEmploymentPreventionWorkHistoryBridges { get; set; }
        IEmploymentStatusType                                          EmploymentStatusType                              { get; set; }
        ICollection<IEmploymentInformation>                            EmploymentInformations                            { get; set; }
        ICollection<IWorkHistorySectionEmploymentPreventionTypeBridge> WorkHistorySectionEmploymentPreventionTypeBridges { get; set; }

        // These are convenience properties that we use:
        ICollection<IWorkHistorySectionEmploymentPreventionTypeBridge> AllWorkHistorySectionEmploymentPreventionTypeBridges { get; }
        IYesNoUnknownLookup                                            YesNoUnknownLookup                                   { get; set; }
    }
}
