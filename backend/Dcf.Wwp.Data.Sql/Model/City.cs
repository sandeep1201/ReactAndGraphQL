using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class City
    {
        #region Properties

        public string    Name            { get; set; }
        public string    GooglePlaceId   { get; set; }
        public int?      CountryId       { get; set; }
        public int?      StateId         { get; set; }
        public decimal?  LatitudeNumber  { get; set; }
        public decimal?  LongitudeNumber { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual Country Country { get; set; }

        [JsonIgnore]
        public virtual State State { get; set; }

        public virtual ICollection<SchoolCollegeEstablishment>  SchoolCollegeEstablishments  { get; set; }
        public virtual ICollection<EmploymentInformation>       EmploymentInformations       { get; set; }
        public virtual ICollection<InvolvedWorkProgram>         InvolvedWorkPrograms         { get; set; }
        public virtual ICollection<EmployerOfRecordInformation> EmployerOfRecordInformations { get; set; }
        public virtual ICollection<ParticipantContactInfo>      ParticipantContactInfoes     { get; set; }
        public virtual ICollection<AlternateMailingAddress>     AlternateMailingAddresses    { get; set; }
        public virtual ICollection<NonSelfDirectedActivity>     NonSelfDirectedActivities    { get; set; }

        #endregion
    }
}
