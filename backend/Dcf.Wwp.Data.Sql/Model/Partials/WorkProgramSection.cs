using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkProgramSection : BaseCommonModel, IWorkProgramSection, IComplexModel, IEquatable<WorkProgramSection>
    {
        ICollection<IInvolvedWorkProgram> IWorkProgramSection.InvolvedWorkPrograms
        {
            get => (from x in InvolvedWorkPrograms where x.IsDeleted == false select x).Cast<IInvolvedWorkProgram>().ToList();

            set => InvolvedWorkPrograms = value.Cast<InvolvedWorkProgram>().ToList();
        }

        ICollection<IInvolvedWorkProgram> IWorkProgramSection.AllInvolvedWorkPrograms
        {
            get => InvolvedWorkPrograms.Cast<IInvolvedWorkProgram>().ToList();

            set => InvolvedWorkPrograms = value.Cast<InvolvedWorkProgram>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var wps = new WorkProgramSection();

            wps.Id                   = Id;
            wps.IsInOtherPrograms    = IsInOtherPrograms;
            wps.Notes                = Notes;
            wps.InvolvedWorkPrograms = InvolvedWorkPrograms.Select(x => (InvolvedWorkProgram) x.Clone()).ToList();

            return wps;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WorkProgramSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(WorkProgramSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(IsInOtherPrograms, other.IsInOtherPrograms))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (AreBothNotNull(InvolvedWorkPrograms, other.InvolvedWorkPrograms) && !InvolvedWorkPrograms.OrderBy(x => x.Id).SequenceEqual(other.InvolvedWorkPrograms.OrderBy(x => x.Id))) // this little magic checks if the InvolvedWorkProgram lists are the same
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

            Debug.Assert(cloned is IWorkProgramSection, "cloned is not IWorkProgramSection");

            var clone = (IWorkProgramSection) cloned;

            if (AreBothNotNull(InvolvedWorkPrograms, clone.InvolvedWorkPrograms))
            {
                var first  = InvolvedWorkPrograms.OrderBy(x => x.Id).ToList();
                var second = clone.InvolvedWorkPrograms.OrderBy(x => x.Id).ToList();

                var i = 0;
                foreach (var fms1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (fms1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (i < second.Count)
                        {
                            var fms2 = second[i];

                            if (!fms1.Equals(fms2))
                            {
                                fms1.ModifiedBy   = user;
                                fms1.ModifiedDate = modDate;
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            fms1.ModifiedBy   = user;
                            fms1.ModifiedDate = modDate;
                        }
                    }

                    i++;
                }
            }
        }

        #endregion IComplexModel
    }
}
