using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChildYouthSection
    {
        #region Properties

        public int?      ParticipantId                                    { get; set; }
        public bool?     HasChildren12OrUnder                             { get; set; }
        public bool?     HasChildrenOver12WithDisabilityInNeedOfChildCare { get; set; }
        public bool?     HasFutureChildCareNeed                           { get; set; }
        public string    FutureChildCareNeedNotes                         { get; set; }
        public int?      HasChildWelfareWorker                            { get; set; }
        public string    ChildWelfareWorkerChildren                       { get; set; }
        public string    ChildWelfareWorkerPlanOrRequirements             { get; set; }
        public int?      ChildWelfareContactId                            { get; set; }
        public bool?     HasWicBenefits                                   { get; set; }
        public bool?     IsInHeadStart                                    { get; set; }
        public bool?     IsInAfterSchoolOrSummerProgram                   { get; set; }
        public string    AfterSchoolProgramDetails                        { get; set; }
        public bool?     IsInMentoringProgram                             { get; set; }
        public string    MentoringProgramDetails                          { get; set; }
        public bool?     DidOrWillAgeOutOfFosterCare                      { get; set; }
        public string    FosterCareDetails                                { get; set; }
        public bool?     IsSpecialNeedsProgramming                        { get; set; }
        public string    SpecialNeedsProgrammingDetails                   { get; set; }
        public string    Notes                                            { get; set; }
        public bool      IsDeleted                                        { get; set; }
        public string    ModifiedBy                                       { get; set; }
        public DateTime? ModifiedDate                                     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ChildYouthSectionChild> ChildYouthSectionChilds { get; set; } = new List<ChildYouthSectionChild>();
        public virtual Contact                             Contact                 { get; set; }
        public virtual Participant                         Participant             { get; set; }
        public virtual YesNoUnknownLookup                  YesNoUnknownLookup      { get; set; }

        #endregion
    }
}
