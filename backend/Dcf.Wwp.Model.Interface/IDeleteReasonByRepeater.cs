using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IDeleteReasonByRepeater
    {
        int Id { get; set; }
        string Repeater { get; set; }
        int DeleteReasonId { get; set; }
        int SortOrder { get; set; }
        bool IsDeleted { get; set; }
        Nullable<System.DateTime> ModifiedDate { get; set; }
        string ModifiedBy { get; set; }
        byte[] RowVersion { get; set; }
        IDeleteReason DeleteReason { get; set; }
    }
}

