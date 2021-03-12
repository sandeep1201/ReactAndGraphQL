using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class InvolvedWorkProgram
    {
        #region Properties

        public int?      WorkProgramSectionId { get; set; }
        public int?      WorkProgramStatusId  { get; set; }
        public int?      WorkProgramId        { get; set; }
        public int?      CityId               { get; set; }
        public DateTime? StartMonth           { get; set; }
        public DateTime? EndMonth             { get; set; }
        public int?      ContactId            { get; set; }
        public string    ContactInfo          { get; set; }
        public string    Details              { get; set; }
        public int?      SortOrder            { get; set; }
        public bool      IsDeleted            { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime? ModifiedDate         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual WorkProgramSection WorkProgramSection { get; set; }
        public virtual WorkProgramStatus  WorkProgramStatus  { get; set; }
        public virtual WorkProgram        WorkProgram        { get; set; }
        public virtual City               City               { get; set; }
        public virtual Contact            Contact            { get; set; }

        #endregion
    }
}
