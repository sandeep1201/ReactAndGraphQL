using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class InvolvedWorkProgram : BaseCommonModel, IInvolvedWorkProgram, IEquatable<InvolvedWorkProgram>
    {
        IWorkProgram IInvolvedWorkProgram.WorkProgram
        {
            get { return WorkProgram; }

            set { WorkProgram = (WorkProgram) value; }
        }

        IWorkProgramSection IInvolvedWorkProgram.WorkProgramSection
        {
            get { return WorkProgramSection; }

            set { WorkProgramSection = (WorkProgramSection) value; }
        }

        IWorkProgramStatus IInvolvedWorkProgram.WorkProgramStatus
        {
            get { return WorkProgramStatus; }

            set { WorkProgramStatus = (WorkProgramStatus) value; }
        }

        ICity IInvolvedWorkProgram.City
        {
            get { return City; }

            set { City = (City) value; }
        }

        IContact IInvolvedWorkProgram.Contact
        {
            get { return Contact; }

            set { Contact = (Contact) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var iwp = new InvolvedWorkProgram();

            iwp.Id                  = this.Id;
            iwp.ContactId           = this.ContactId;
            iwp.WorkProgramStatusId = this.WorkProgramStatusId;
            iwp.WorkProgramId       = this.WorkProgramId;
            iwp.StartMonth          = this.StartMonth;
            iwp.EndMonth            = this.EndMonth;
            iwp.ContactInfo         = this.ContactInfo;
            iwp.Details             = this.Details;
            iwp.WorkProgram         = (WorkProgram) this.WorkProgram?.Clone();
            iwp.WorkProgramStatus   = (WorkProgramStatus) this.WorkProgramStatus?.Clone();
            iwp.City                = (City) this.City?.Clone();
            iwp.CityId              = this.CityId;
            iwp.IsDeleted           = this.IsDeleted;
            // NOTE: We don't clone references to "parent" objects such as WorkProgramSection
            //iwp.WorkProgramSection = (WorkProgramSection)this.WorkProgramSection.Clone();

            return iwp;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as InvolvedWorkProgram;
            return obj != null && Equals(obj);
        }

        public bool Equals(InvolvedWorkProgram other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ContactId, other.ContactId))
                return false;
            if (!AdvEqual(WorkProgramStatusId, other.WorkProgramStatusId))
                return false;
            if (!AdvEqual(WorkProgramId, other.WorkProgramId))
                return false;
            if (!AdvEqual(StartMonth, other.StartMonth))
                return false;
            if (!AdvEqual(EndMonth, other.EndMonth))
                return false;
            if (!AdvEqual(ContactInfo, other.ContactInfo))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(WorkProgram, other.WorkProgram))
                return false;
            if (!AdvEqual(WorkProgramStatus, other.WorkProgramStatus))
                return false;
            if (!AdvEqual(CityId, other.CityId))
                return false;
            if (!AdvEqual(City, other.City))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }
    }

    #endregion IEquatable<T>
}
