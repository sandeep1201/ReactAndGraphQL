using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IActivityRepository
    {
        #region Methods

        IEnumerable<IActivity>           WhereActivites(Expression<Func<IActivity, bool>>                 clause);
        IEnumerable<IActivity>           AllActiviesByEP(int                                              epId);
        IActivity                        NewActivity(int                                                  epId);
        INonSelfDirectedActivity            NewSelfDirectedActivity(IActivity                                activity, string user);
        void                             DeleteActivity(int                                               id);
        string                           GetActivityLocationName(int?                                     id);
        IActivity                        GetActivity(int                                                  id);
        INonSelfDirectedActivity            GetSelfDirectedActivity(int                                      activityId);
        IEnumerable<IActivitySchedule>   WhereActivitySchedules(Expression<Func<IActivitySchedule, bool>> clause);
        IEnumerable<IActivityScheduleFrequencyBridge> WhereActivityScheduleFrequencies(Expression<Func<IActivityScheduleFrequencyBridge, bool>> clause);
        IActivitySchedule                NewActivitySchedule(int                                          activityId);
        IActivitySchedule                GetActivitySchedule(int                                          id);
        IActivityScheduleFrequencyBridge NewActivityScheduleFrequencyBridge(IActivitySchedule schedule);
        IActivityScheduleFrequencyBridge GetActivityScheduleFrequencyBridge(int                           id);
        void                             DeleteActivitySchedules(IEnumerable<int>                         ids);
        void DeleteActivityScheduleFrequencies(IEnumerable<int> ids);
        void                             DeleteSelfDirectedActivity(int                                   id);
        IEnumerable<IFrequencyType>      FrequencyTypes();
        IEnumerable<IFrequency>          WeeklyFrequencies();
        IEnumerable<IFrequency>          MonthlyFrequencies();

        #endregion
    }
}
