using System;

namespace Dcf.Wwp.Model.Interface.Constants
{
    public class JobType
    {
        // Keep in Sync with EmployeeProgramType table in datbase!!!
        public const string Volunteer                          = @"Volunteer";
        public const string TMJSubsidized                      = @"TMJ (Subsidized)";
        public const string TJSubsidized                       = @"TJ (Subsidized)";
        public const string TMJUnSubsidized                    = @"TMJ (Unsubsidized)";
        public const string TJUnSubsidized                     = @"TJ (Unsubsidized)";
        public const string UnSubsidized                       = @"Unsubsidized";
        public const string StaffingAgency                     = @"Staffing Agency";
        public const string SelfEmployed                       = @"Self-Employed";
        public const string TempNonCustodialParentUnsubsidized = @"TEMP Non-Custodial Parent (Unsubsidized)";
        public const string TempNonCustodialParentSubsidized   = @"TEMP Non-Custodial Parent (Subsidized)";
        public const string TempCustodialParentUnsubsidized    = @"TEMP Custodial Parent (Unsubsidized)";
        public const string TempCustodialParentSubsidized      = @"TEMP Custodial Parent (Subsidized)";
    }
}
