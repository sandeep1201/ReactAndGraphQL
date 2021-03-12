using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IChildYouthSection : ICommonDelModel, ICloneable, IComplexModel
    {
        Int32?              ParticipantId                                    { get; set; }
        Boolean?            HasChildren12OrUnder                             { get; set; }
        Boolean?            HasChildrenOver12WithDisabilityInNeedOfChildCare { get; set; }
        Boolean?            HasFutureChildCareNeed                           { get; set; }
        String              FutureChildCareNeedNotes                         { get; set; }
        int?                HasChildWelfareWorker                            { get; set; }
        IYesNoUnknownLookup YesNoUnknownLookup                               { get; set; }
        String              ChildWelfareWorkerChildren                       { get; set; }
        String              ChildWelfareWorkerPlanOrRequirements             { get; set; }
        Boolean?            HasWicBenefits                                   { get; set; }
        Boolean?            IsInHeadStart                                    { get; set; }
        Boolean?            IsInAfterSchoolOrSummerProgram                   { get; set; }
        String              AfterSchoolProgramDetails                        { get; set; }
        Boolean?            IsInMentoringProgram                             { get; set; }
        String              MentoringProgramDetails                          { get; set; }
        Boolean?            DidOrWillAgeOutOfFosterCare                      { get; set; }
        String              FosterCareDetails                                { get; set; }
        String              Notes                                            { get; set; }
        Int32?              ChildWelfareContactId                            { get; set; }
        IContact            Contact                                          { get; set; }
        Boolean?            IsSpecialNeedsProgramming                        { get; set; }
        String              SpecialNeedsProgrammingDetails                   { get; set; }

        /// <summary>
        /// Contains all ChildYouthSectionChilds including IsDeleted items.
        /// </summary>
        ICollection<IChildYouthSectionChild> ChildYouthSectionChilds { get; set; }

        /// <summary>
        /// PreTeens contains all the (non-deleted) ChildYouthSectionChilds that are 12 and under.
        /// </summary>
        ICollection<IChildYouthSectionChild> PreTeens { get; }

        ICollection<IChildYouthSectionChild> AllPreTeens { get; }


        /// <summary>
        /// PreTeens contains all the (non-deleted) ChildYouthSectionChilds that are 13 and over.
        /// </summary>
        ICollection<IChildYouthSectionChild> Teens { get; }

        ICollection<IChildYouthSectionChild> AllTeens { get; }
    }
}
