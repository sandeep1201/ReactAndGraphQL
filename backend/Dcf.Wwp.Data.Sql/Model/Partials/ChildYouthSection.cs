using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Constants = Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChildYouthSection : BaseCommonModel, IChildYouthSection, IEquatable<ChildYouthSection>
    {
        IContact IChildYouthSection.Contact
        {
            get { return Contact; }
            set { Contact = (Contact) value; }
        }

        IYesNoUnknownLookup IChildYouthSection.YesNoUnknownLookup
        {
            get { return YesNoUnknownLookup; }
            set { YesNoUnknownLookup = (YesNoUnknownLookup) value; }
        }

        ICollection<IChildYouthSectionChild> IChildYouthSection.ChildYouthSectionChilds
        {
            get { return ChildYouthSectionChilds.Cast<IChildYouthSectionChild>().ToList(); }
            set { ChildYouthSectionChilds = value.Cast<ChildYouthSectionChild>().ToList(); }
        }

        ICollection<IChildYouthSectionChild> IChildYouthSection.PreTeens    => (from x in ChildYouthSectionChilds where x.AgeCategory?.AgeRange == Constants.AgeCategory.AgeRangeTwelveAndUnder && x.DeleteReasonId == null select x).Cast<IChildYouthSectionChild>().ToList();
        ICollection<IChildYouthSectionChild> IChildYouthSection.AllPreTeens => (from x in ChildYouthSectionChilds where x.AgeCategory?.AgeRange == Constants.AgeCategory.AgeRangeTwelveAndUnder select x).Cast<IChildYouthSectionChild>().ToList();


        ICollection<IChildYouthSectionChild> IChildYouthSection.Teens    => (from x in ChildYouthSectionChilds where x.AgeCategory?.AgeRange == Constants.AgeCategory.AgeRangeThirteenToEighteen && x.DeleteReasonId == null select x).Cast<IChildYouthSectionChild>().ToList();
        ICollection<IChildYouthSectionChild> IChildYouthSection.AllTeens => (from x in ChildYouthSectionChilds where x.AgeCategory?.AgeRange == Constants.AgeCategory.AgeRangeThirteenToEighteen select x).Cast<IChildYouthSectionChild>().ToList();


        #region ICloneable

        public object Clone()
        {
            var clone = new ChildYouthSection
                        {
                            ParticipantId                                    = this.ParticipantId,
                            HasChildren12OrUnder                             = this.HasChildren12OrUnder,
                            HasChildrenOver12WithDisabilityInNeedOfChildCare = this.HasChildrenOver12WithDisabilityInNeedOfChildCare,
                            HasFutureChildCareNeed                           = this.HasFutureChildCareNeed,
                            FutureChildCareNeedNotes                         = this.FutureChildCareNeedNotes,
                            HasChildWelfareWorker                            = this.HasChildWelfareWorker,
                            ChildWelfareWorkerChildren                       = this.ChildWelfareWorkerChildren,
                            ChildWelfareWorkerPlanOrRequirements             = this.ChildWelfareWorkerPlanOrRequirements,
                            HasWicBenefits                                   = this.HasWicBenefits,
                            IsInHeadStart                                    = this.IsInHeadStart,
                            IsInAfterSchoolOrSummerProgram                   = this.IsInAfterSchoolOrSummerProgram,
                            AfterSchoolProgramDetails                        = this.AfterSchoolProgramDetails,
                            IsInMentoringProgram                             = this.IsInMentoringProgram,
                            MentoringProgramDetails                          = this.MentoringProgramDetails,
                            DidOrWillAgeOutOfFosterCare                      = this.DidOrWillAgeOutOfFosterCare,
                            FosterCareDetails                                = this.FosterCareDetails,
                            Notes                                            = this.Notes,
                            ChildWelfareContactId                            = this.ChildWelfareContactId,
                            ChildYouthSectionChilds                          = this.ChildYouthSectionChilds.Select(x => (ChildYouthSectionChild) x.Clone()).ToList(),
                            IsSpecialNeedsProgramming                        = this.IsSpecialNeedsProgramming,
                            SpecialNeedsProgrammingDetails                   = this.SpecialNeedsProgrammingDetails
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ChildYouthSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(ChildYouthSection other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(HasChildren12OrUnder, other.HasChildren12OrUnder))
                return false;
            if (!AdvEqual(HasChildrenOver12WithDisabilityInNeedOfChildCare, other.HasChildrenOver12WithDisabilityInNeedOfChildCare))
                return false;
            if (!AdvEqual(HasFutureChildCareNeed, other.HasFutureChildCareNeed))
                return false;
            if (!AdvEqual(FutureChildCareNeedNotes, other.FutureChildCareNeedNotes))
                return false;
            if (!AdvEqual(HasChildWelfareWorker, other.HasChildWelfareWorker))
                return false;
            if (!AdvEqual(ChildWelfareWorkerChildren, other.ChildWelfareWorkerChildren))
                return false;
            if (!AdvEqual(ChildWelfareWorkerPlanOrRequirements, other.ChildWelfareWorkerPlanOrRequirements))
                return false;
            if (!AdvEqual(HasWicBenefits, other.HasWicBenefits))
                return false;
            if (!AdvEqual(IsInHeadStart, other.IsInHeadStart))
                return false;
            if (!AdvEqual(IsInAfterSchoolOrSummerProgram, other.IsInAfterSchoolOrSummerProgram))
                return false;
            if (!AdvEqual(AfterSchoolProgramDetails, other.AfterSchoolProgramDetails))
                return false;
            if (!AdvEqual(IsInMentoringProgram, other.IsInMentoringProgram))
                return false;
            if (!AdvEqual(MentoringProgramDetails, other.MentoringProgramDetails))
                return false;
            if (!AdvEqual(DidOrWillAgeOutOfFosterCare, other.DidOrWillAgeOutOfFosterCare))
                return false;
            if (!AdvEqual(FosterCareDetails, other.FosterCareDetails))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(ChildWelfareContactId, other.ChildWelfareContactId))
                return false;
            if (!AdvEqual(IsSpecialNeedsProgramming, other.IsSpecialNeedsProgramming))
                return false;
            if (!AdvEqual(SpecialNeedsProgrammingDetails, other.SpecialNeedsProgrammingDetails))
                return false;

            if (AreBothNotNull(ChildYouthSectionChilds, other.ChildYouthSectionChilds) && !(ChildYouthSectionChilds.OrderBy(x => x.Id).SequenceEqual(other.ChildYouthSectionChilds.OrderBy(x => x.Id))))
                return false;

            return true;
        }

        #endregion IEquatable<T>

        #region IComplexModel

        public void SetModifiedOnComplexProperties<T>(T cloned, string user, DateTime modDate)
            where T : class, ICloneable, ICommonModel
        {
            // We don't need to set modified on null objects.
            if (cloned == null) return;

            Debug.Assert(cloned is IChildYouthSection, "cloned is not IChildYouthSection");

            var clone = (IChildYouthSection) cloned;

            if (AreBothNotNull(ChildYouthSectionChilds, clone.ChildYouthSectionChilds))
            {
                var first  = ChildYouthSectionChilds.OrderBy(x => x.Id).ToList();
                var second = clone.ChildYouthSectionChilds.OrderBy(x => x.Id).ToList();

                int i = 0;
                foreach (var cysc1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (cysc1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (i < second.Count)
                        {
                            var cysc2 = second[i];

                            if (!cysc1.Equals(cysc2))
                            {
                                cysc1.ModifiedBy   = user;
                                cysc1.ModifiedDate = modDate;

                                // Now check the Child property
                                if (cysc1.Child != null && cysc2.Child != null && !cysc1.Child.Equals(cysc2.Child))
                                {
                                    cysc1.Child.ModifiedBy   = user;
                                    cysc1.Child.ModifiedDate = modDate;
                                }
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            cysc1.ModifiedBy   = user;
                            cysc1.ModifiedDate = modDate;
                        }
                    }

                    i++;
                }
            }
        }

        #endregion IComplexModel
    }
}
