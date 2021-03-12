using System;

//using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PostSecondaryCollege
    {
        #region Properties

        public int       PostSecondaryEducationSectionId { get; set; }
        public int?      SchoolCollegeEstablishmentId    { get; set; }
        public bool?     HasGraduated                    { get; set; }
        public int?      LastYearAttended                { get; set; }
        public bool?     CurrentlyAttending              { get; set; }
        public int?      Semesters                       { get; set; }
        public decimal?  Credits                         { get; set; }
        public string    Details                         { get; set; }
        public int?      OriginId                        { get; set; }
        public int?      SortOrder                       { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual PostSecondaryEducationSection PostSecondaryEducationSection { get; set; }
        public virtual SchoolCollegeEstablishment    SchoolCollegeEstablishment    { get; set; }

        #endregion

        // #region Convenience backwards compatibility properties
        //
        // [NotMapped]
        // public bool? IsCurrentlyAttending
        // {
        //     get => CurrentlyAttending;
        //     set => CurrentlyAttending = value;
        // }
        //
        // #endregion
    }
}
