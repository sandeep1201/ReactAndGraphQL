using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IDeleteReason
    {
        Int32 Id { get; set; }
        String Name { get; set; }
        DateTime? CreatedDate { get; set; }
        String ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Byte[] RowVersion { get; set; }
        ICollection<ITimeLimitExtension> TimeLimitExtensions { get; set; }
        ICollection<IDeleteReasonByRepeater> DeleteReasonByRepeaters { get; set; }
    }
}