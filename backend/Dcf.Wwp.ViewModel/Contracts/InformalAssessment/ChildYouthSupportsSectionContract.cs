using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Contracts.Cww;


namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class ChildYouthSupportsSectionContract : BaseInformalAssessmentContract
    {
        public bool?                   HasChildren                             { get; set; }
        public List<ChildCareContract> Children                                { get; set; }
        public List<ChildCareContract> DeletedChildren                         { get; set; }
        public bool?                   HasTeensWithDisabilityInNeedOfChildCare { get; set; }
        public List<TeenCareContract>  Teens                                   { get; set; }
        public List<TeenCareContract>  DeletedTeens                            { get; set; }
        public bool?                   HasWicBenefits                          { get; set; }
        public bool?                   IsInHeadStart                           { get; set; }
        public bool?                   IsInAfterSchoolOrSummerProgram          { get; set; }
        public string                  AfterSchoolOrSummerProgramNotes         { get; set; }
        public bool?                   IsInMentoringProgram                    { get; set; }
        public string                  MentoringProgramNotes                   { get; set; }
        public int?                    HasChildWelfareWorkerId                 { get; set; }
        public string                  HasChildWelfareWorkerName               { get; set; }
        public string                  ChildWelfareWorkerPlan                  { get; set; }
        public string                  ChildWelfareWorkerChildren              { get; set; }
        public int?                    ChildWelfareWorkerContactId             { get; set; }
        public bool?                   DidOrWillAgeOutOfFosterCare             { get; set; }
        public string                  FosterCareNotes                         { get; set; }
        public bool?                   HasFutureChildCareChanges               { get; set; }
        public string                  FutureChildCareChangesNotes             { get; set; }
        public ActionNeededContract    ActionNeeded                            { get; set; }
        public string                  Notes                                   { get; set; }
        public List<Child>             CwwChildren                             { get; set; }
        public ChildCareEligibility    CwwEligibility                          { get; set; }
        public bool?                   IsSpecialNeedsProgramming               { get; set; }
        public string                  SpecialNeedsProgrammingDetails          { get; set; }

        #region Constructor

        public ChildYouthSupportsSectionContract()
        {
        }

        #endregion

        #region Business Rules

        // NOTE: Be sure to keep this logic in sync with the user interface logic (Angular code).

        public bool HasChildUnder5
        {
            get
            {
                if (HasChildren.HasValue && HasChildren.Value && Children != null && Children.Count > 0)
                {
                    foreach (var cc in Children)
                    {
                        var age = cc.AgeInYears;

                        if (age.HasValue && age < 5)
                            return true;
                    }
                }

                if (CwwChildren.Count > 0)
                {
                    foreach (var cc in CwwChildren)
                    {
                        var age = cc.Age;

                        if (age.HasValue && age < 5)
                            return true;
                    }
                }

                return false;
            }
        }

        public bool HasChild5OrOver
        {
            get
            {
                if (HasChildren.HasValue && HasChildren.Value && Children != null && Children.Count > 0)
                {
                    foreach (var cc in Children)
                    {
                        var age = cc.AgeInYears;

                        //if (age.HasValue && age >= 5 && age <= 12)
                        if (age.HasValue && age >= 5 ) // biz req. 6.5.3 & 6.5.4 - only says '5 or older'
                            return true;
                    }
                }

                if (CwwChildren.Count > 0)
                {
                    foreach (var cc in CwwChildren)
                    {
                        var age = cc.Age;

                        //if (age.HasValue && age >= 5 && age <= 12)
                        if (age.HasValue && age >= 5) // biz req. 6.5.3 & 6.5.4 - only says '5 or older'
                            return true;
                    }
                }

                if (HasTeensWithDisabilityInNeedOfChildCare.HasValue && HasTeensWithDisabilityInNeedOfChildCare.Value && Teens != null && Teens.Count > 0)
                {
                    foreach (var tc in Teens)
                    {
                        var age = tc.AgeInYears;

                        if (age.HasValue && age < 19)
                            return true;
                    }
                }

                return false;
            }
        }

        #endregion Business Rules
    }
}
