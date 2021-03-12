using System;
using System.Collections;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IActivity : ICommonModelFinal, ICloneable
    {
        int                                           Id                               { get; set; }
        int                                           ActivityTypeId                   { get; set; }
        string                                        Description                      { get; set; }
        int?                                          ActivityLocationId               { get; set; }
        string                                        Details                          { get; set; }
        DateTime?                                     StartDate                        { get; set; }
        IActivityLocation                             ActivityLocation                 { get; set; }
        IActivityType                                 ActivityType                     { get; set; }
        ICollection<IActivitySchedule>                ActivitySchedules                { get; set; }
        ICollection<INonSelfDirectedActivity>         SelfDirectedActivities           { get; set; }
        ICollection<IActivityContactBridge>           ActivityContactBridges           { get; set; }
        ICollection<IActivityContactBridge>           NonActivityContactBridges        { get; }
        ICollection<IEmployabilityPlanActivityBridge> EmployabilityPlanActivityBridges { get; set; }
    }
}
