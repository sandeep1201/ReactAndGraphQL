using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITimeLimitType : ICommonDelModel
    {
        String Name { get; set; }
        DateTime? CreatedDate { get; set; }
        ICollection<ITimeLimit> TimeLimits { get; set; }
        ICollection<ITimeLimitExtension> TimeLimitExtensions { get; set; }
    }
}