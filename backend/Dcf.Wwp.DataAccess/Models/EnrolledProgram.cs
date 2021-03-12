using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EnrolledProgram : BaseEntity
    {
        #region Properties

        public string    ProgramCode     { get; set; }
        public string    SubProgramCode  { get; set; }
        public string    Name            { get; set; }
        public string    ShortName       { get; set; }
        public string    ProgramType     { get; set; }
        public string    DescriptionText { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<EnrolledProgramValidity>             EnrolledProgramValidities            { get; set; } = new List<EnrolledProgramValidity>();
        public virtual ICollection<ContractArea>                        ContractAreas                        { get; set; } = new List<ContractArea>();
        public virtual ICollection<EnrolledProgramEPActivityTypeBridge> EnrolledProgramEPActivityTypeBridges { get; set; } = new List<EnrolledProgramEPActivityTypeBridge>();
        public virtual ICollection<GoalType>                            GoalTypes                            { get; set; } = new List<GoalType>();
        public virtual ICollection<EnrolledProgramPinCommentTypeBridge> EnrolledProgramPinCommentTypeBridges { get; set; } = new List<EnrolledProgramPinCommentTypeBridge>();

        #endregion
    }
}
