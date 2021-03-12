using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class RuleReason : BaseEntity
    {
        #region Properties

        public string   Category     { get; set; }
        public string   SubCategory  { get; set; }
        public string   Name         { get; set; }
        public string   Code         { get; set; }
        public int      SortOrder    { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Nav Props

        #endregion
    }
}
