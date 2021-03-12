using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployerOfRecordInformation : BaseCommonModel, IEmployerOfRecordInformation, IEquatable<EmployerOfRecordInformation>
    {
        #region Properties

        #endregion

        #region Navigation Properties

        ICity IEmployerOfRecordInformation.City
        {
            get => City;
            set => City = (City) value;
        }

        IContact IEmployerOfRecordInformation.Contact
        {
            get => Contact;
            set => Contact = (Contact) value;
        }

        IEmploymentInformation IEmployerOfRecordInformation.EmploymentInformation
        {
            get => EmploymentInformation;
            set => EmploymentInformation = (EmploymentInformation) value;
        }

        IJobSector IEmployerOfRecordInformation.JobSector
        {
            get => JobSector;
            set => JobSector = (JobSector) value;
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            var em = new EmployerOfRecordInformation();

            em.EmploymentInformationId = this.EmploymentInformationId;
            em.CompanyName             = this.CompanyName;
            em.Fein                    = this.Fein;
            em.StreetAddress           = this.StreetAddress;
            em.ZipAddress              = this.ZipAddress;
            em.CityId                  = this.CityId;
            em.JobSectorId             = this.JobSectorId;
            em.ContactId               = this.ContactId;
            em.IsDeleted               = this.IsDeleted;
            em.ModifiedBy              = this.ModifiedBy;
            em.ModifiedDate            = this.ModifiedDate;

            em.Contact   = (Contact) this.Contact?.Clone();
            em.City      = (City) this.City?.Clone();
            em.JobSector = (JobSector) this.JobSector?.Clone();
            //em.EmploymentInformation = (EmploymentInformation) this.EmploymentInformation.Clone();

            return em;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmployerOfRecordInformation;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmployerOfRecordInformation other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal

            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmploymentInformationId, other.EmploymentInformationId))
                return false;
            if (!AdvEqual(CompanyName, other.CompanyName))
                return false;
            if (!AdvEqual(Fein, other.Fein))
                return false;
            if (!AdvEqual(StreetAddress, other.StreetAddress))
                return false;
            if (!AdvEqual(ZipAddress, other.ZipAddress))
                return false;
            if (!AdvEqual(CityId, other.CityId))
                return false;
            if (!AdvEqual(JobSectorId, other.JobSectorId))
                return false;
            if (!AdvEqual(ContactId, other.ContactId))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(ModifiedBy, other.ModifiedBy))
                return false;
            if (!AdvEqual(ModifiedDate, other.ModifiedDate))
                return false;
            if (!AdvEqual(Contact, other.Contact))
                return false;
            if (!AdvEqual(City, other.City))
                return false;
            if (!AdvEqual(JobSector, other.JobSector))
                return false;
            if (!AdvEqual(EmploymentInformation, other.EmploymentInformation))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
