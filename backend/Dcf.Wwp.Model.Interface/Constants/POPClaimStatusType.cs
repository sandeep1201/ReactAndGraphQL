using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Constants
{
    public static class POPClaimStatusType
    {
        public const string SubmitCd   = "SB";
        public const string ReviewCd   = "RV";
        public const string WithdrewCd = "WD";
        public const string ApproveCd  = "AP";
        public const string DeniedCd   = "DN";
        public const string ReturnCd   = "RT";
        public const string ValidateCd = "VD";
    }

    public static class POPClaimType
    {
        public const string JobAttainmentCd             = "JA";
        public const string JobRetentionCd              = "JR";
        public const string LongTermCd                  = "LPJA";
        public const string JobAttainmentWithHighWageCd = "JAHW";
        public const string VocationalTrainingCd        = "VTCI";
        public const string EducationalAttainmentCd     = "EAI";
    }

    public static class POPStatusToClaimMap
    {
        public static readonly Dictionary<Tuple<string, string>, string> POPStatusToClaimMaps = new Dictionary<Tuple<string, string>, string>
                                                                                                {
                                                                                                    // Educational Attainment
                                                                                                    { new Tuple<string, string>(POPClaimType.EducationalAttainmentCd, POPClaimStatusType.SubmitCd), WorkerTaskCategoryCodes.EAInitiatedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.EducationalAttainmentCd, POPClaimStatusType.ReturnCd), WorkerTaskCategoryCodes.EAReturnedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.EducationalAttainmentCd, POPClaimStatusType.DeniedCd), WorkerTaskCategoryCodes.EADeniedCode },

                                                                                                    // Vocational Training
                                                                                                    { new Tuple<string, string>(POPClaimType.VocationalTrainingCd, POPClaimStatusType.SubmitCd), WorkerTaskCategoryCodes.VTInitiatedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.VocationalTrainingCd, POPClaimStatusType.ReturnCd), WorkerTaskCategoryCodes.VTReturnedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.VocationalTrainingCd, POPClaimStatusType.DeniedCd), WorkerTaskCategoryCodes.VTDeniedCode },

                                                                                                    // Job Attainment
                                                                                                    { new Tuple<string, string>(POPClaimType.JobAttainmentCd, POPClaimStatusType.SubmitCd), WorkerTaskCategoryCodes.JAInitiatedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.JobAttainmentCd, POPClaimStatusType.DeniedCd), WorkerTaskCategoryCodes.JADeniedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.JobAttainmentCd, POPClaimStatusType.ReturnCd), WorkerTaskCategoryCodes.JAReturnedCode },

                                                                                                    // Job Retention
                                                                                                    { new Tuple<string, string>(POPClaimType.JobRetentionCd, POPClaimStatusType.SubmitCd), WorkerTaskCategoryCodes.JRInitiatedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.JobRetentionCd, POPClaimStatusType.DeniedCd), WorkerTaskCategoryCodes.JRDeniedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.JobRetentionCd, POPClaimStatusType.ReturnCd), WorkerTaskCategoryCodes.JRReturnedCode },

                                                                                                    // Long Term Job Attainment
                                                                                                    { new Tuple<string, string>(POPClaimType.LongTermCd, POPClaimStatusType.SubmitCd), WorkerTaskCategoryCodes.LTJAInitiatedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.LongTermCd, POPClaimStatusType.DeniedCd), WorkerTaskCategoryCodes.LTJADeniedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.LongTermCd, POPClaimStatusType.ReturnCd), WorkerTaskCategoryCodes.LTJAReturnedCode },

                                                                                                    // High Wage Job Attainment
                                                                                                    { new Tuple<string, string>(POPClaimType.JobAttainmentWithHighWageCd, POPClaimStatusType.SubmitCd), WorkerTaskCategoryCodes.JAHWInitiatedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.JobAttainmentWithHighWageCd, POPClaimStatusType.DeniedCd), WorkerTaskCategoryCodes.JAHWDeniedCode },
                                                                                                    { new Tuple<string, string>(POPClaimType.JobAttainmentWithHighWageCd, POPClaimStatusType.ReturnCd), WorkerTaskCategoryCodes.JAHWReturnedCode },
                                                                                                };
    }
}
