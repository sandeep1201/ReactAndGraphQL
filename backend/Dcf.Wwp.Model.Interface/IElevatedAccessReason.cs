using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IElevatedAccessReason
    {
        #region Properties

        int       Id           { get; set; }
        string    Reason       { get; set; }
        int?      SortOrder    { get; set; }
        string    ModifiedBy   { get; set; }
        bool      IsDeleted    { get; set; }
        DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Props

        ICollection<IElevatedAccess> ElevatedAccesses { get; set; }
        
        #endregion
    }
}
