namespace Dcf.Wwp.Model.Interface.Constants
{
    public class ActionNeededPage
    {
        // These strings are used for lookups in Action Needed and match the Code
        // values in the ActionNeededPage table.
        public const string Housing               = @"housing";
        public const string Transportation        = @"transportation";
        public const string LegalIssues           = @"legal-issues";
        public const string ChildAndYouthSupports = @"child-youth-supports";
        public const string FamilyBarriers        = @"family-barriers";
        public const string NonCustodialParents   = @"non-custodial-parents";
        public const string JobReadiness          = @"job-readiness";
        public const string Other                 = @"other";

        // Keep in Sync with ActionNeededPage table in database!!!
        public const int OtherId = 7;
    }
}
