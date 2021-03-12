using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IActivityType : ICommonModelFinal, ICloneable
    {
        string                                            Code                                 { get; set; }
        string                                            Name                                 { get; set; }
        int                                               SortOrder                            { get; set; }
        DateTime?                                         EffectiveDate                        { get; set; }
        DateTime?                                         EndDate                              { get; set; }
        ICollection<IActivity>                            Activities                           { get; set; }
        ICollection<IEnrolledProgramEPActivityTypeBridge> EnrolledProgramEPActivityTypeBridges { get; set; }
    }
}
