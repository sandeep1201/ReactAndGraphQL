using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class WorkHistorySection : BaseCommonModel, IWorkHistorySection, IEquatable<WorkHistorySection>
    {
        IEmploymentStatusType IWorkHistorySection.EmploymentStatusType
        {
            get => EmploymentStatusType;
            set => EmploymentStatusType = (EmploymentStatusType) value;
        }

        ICollection<IWorkHistorySectionEmploymentPreventionTypeBridge> IWorkHistorySection.WorkHistorySectionEmploymentPreventionTypeBridges
        {
            get => (from x in WorkHistorySectionEmploymentPreventionTypeBridges where !x.IsDeleted select x).Cast<IWorkHistorySectionEmploymentPreventionTypeBridge>().ToList();

            set => WorkHistorySectionEmploymentPreventionTypeBridges = value.Cast<WorkHistorySectionEmploymentPreventionTypeBridge>().ToList();
        }

        ICollection<IEmploymentInformation> IWorkHistorySection.EmploymentInformations
        {
            get => EmploymentInformations.Cast<IEmploymentInformation>().ToList();
            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        IYesNoUnknownLookup IWorkHistorySection.YesNoUnknownLookup
        {
            get => YesNoUnknownLookup;
            set => YesNoUnknownLookup = (YesNoUnknownLookup) value;
        }

        ICollection<IWorkHistorySectionEmploymentPreventionTypeBridge> IWorkHistorySection.AllWorkHistorySectionEmploymentPreventionTypeBridges => WorkHistorySectionEmploymentPreventionTypeBridges.Cast<IWorkHistorySectionEmploymentPreventionTypeBridge>().ToList();

        #region ICloneable

        public object Clone()
        {
            var wh = new WorkHistorySection();

            wh.Id                                                = Id;
            wh.NonFullTimeDetails                                = NonFullTimeDetails;
            wh.PreventionFactors                                 = PreventionFactors;
            wh.HasVolunteered                                    = HasVolunteered;
            wh.Notes                                             = Notes;
            wh.EmploymentStatusTypeId                            = EmploymentStatusTypeId;
            wh.EmploymentStatusType                              = (EmploymentStatusType) EmploymentStatusType?.Clone();
            wh.WorkHistorySectionEmploymentPreventionTypeBridges = WorkHistorySectionEmploymentPreventionTypeBridges.Select(x => (WorkHistorySectionEmploymentPreventionTypeBridge) x.Clone()).ToList();
            wh.HasCareerAssessment                               = HasCareerAssessment;
            wh.HasCareerAssessmentNotes                          = HasCareerAssessmentNotes;

            return wh;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WorkHistorySection;
            return obj != null && Equals(obj);
        }

        public bool Equals(WorkHistorySection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmploymentStatusTypeId, other.EmploymentStatusTypeId))
                return false;
            if (!AdvEqual(NonFullTimeDetails, other.NonFullTimeDetails))
                return false;
            if (!AdvEqual(HasVolunteered, other.HasVolunteered))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(EmploymentStatusType, other.EmploymentStatusType))
                return false;
            if (AreBothNotNull(WorkHistorySectionEmploymentPreventionTypeBridges, other.WorkHistorySectionEmploymentPreventionTypeBridges) && !WorkHistorySectionEmploymentPreventionTypeBridges.OrderBy(x => x.Id).SequenceEqual(other.WorkHistorySectionEmploymentPreventionTypeBridges.OrderBy(x => x.Id)))
                return false;
            if (!AdvEqual(HasCareerAssessment, other.HasCareerAssessment))
                return false;
            if (!AdvEqual(HasCareerAssessmentNotes, other.HasCareerAssessmentNotes))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
