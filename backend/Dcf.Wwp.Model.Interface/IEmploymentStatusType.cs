using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmploymentStatusType
    {
         Int32 Id { get; set; }
         Int32? SortOrder { get; set; }
         String Name { get; set; }
         String ModifiedBy { get; set; }
         DateTime? ModifiedDate { get; set; }
         Byte[] RowVersion { get; set; }
        ICollection<IWorkHistorySection> WorkHistorySections { get; set; }
    }
}