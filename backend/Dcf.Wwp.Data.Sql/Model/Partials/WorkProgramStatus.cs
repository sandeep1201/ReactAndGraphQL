using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class WorkProgramStatus : BaseCommonModel, IWorkProgramStatus, IEquatable<WorkProgramStatus>
    {
        ICollection<IInvolvedWorkProgram> IWorkProgramStatus.InvolvedWorkPrograms
        {
            get => InvolvedWorkPrograms.Cast<IInvolvedWorkProgram>().ToList();
            set => InvolvedWorkPrograms = value.Cast<InvolvedWorkProgram>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var wp = new WorkProgramStatus();

            wp.Id        = Id;
            wp.SortOrder = SortOrder;
            wp.Name      = Name;

            return wp;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WorkProgramStatus;
            return obj != null && Equals(obj);
        }

        public bool Equals(WorkProgramStatus other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)               &&
                   SortOrder.Equals(other.SortOrder) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
