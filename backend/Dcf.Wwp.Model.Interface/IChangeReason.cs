using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IChangeReason : ICommonDelCreatedModel
    {
        String Name { get; set; }
        Boolean? IsRequired { get; set; }
        string Code { get; set; }
        ICollection<ITimeLimit> TimeLimits { get; set; }
    }
}