using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PostSecondaryDegree
    {
        #region Properties

        public int       PostSecondaryEducationSectionId { get; set; }
        public string    Name                            { get; set; }
        public string    College                         { get; set; }
        public int?      DegreeTypeId                    { get; set; }
        public int?      YearAttained                    { get; set; }
        public int?      OriginId                        { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual PostSecondaryEducationSection PostSecondaryEducationSection { get; set; }
        public virtual DegreeType                    DegreeType                    { get; set; }

        #endregion
    }
}
