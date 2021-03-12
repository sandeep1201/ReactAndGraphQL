using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IRuleReason
    {
        #region Properties

        int      Id           { get; set; }
        string   Category     { get; set; }
        string   SubCategory  { get; set; }
        string   Name         { get; set; }
        string   Code         { get; set; }
        int      SortOrder    { get; set; }
        bool     IsDeleted    { get; set; }
        string   ModifiedBy   { get; set; }
        DateTime ModifiedDate { get; set; }
        byte[]   RowVersion   { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
