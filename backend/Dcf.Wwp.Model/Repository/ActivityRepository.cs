using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository :  IActivityRepository
    {
        #region Methods

        public IEnumerable<IActivity> WhereActivites(Expression<Func<IActivity, bool>> clause)
        {
            var q = _db.Activities.Where(clause);

            return (q);
        }

        public IEnumerable<IActivity> AllActiviesByEP(int epId)
        {
            //return _db.EmployabilityPlans.SingleOrDefault(x => x.Id == epId)?.Activities.AsEnumerable<IActivity>(); original code - 04/16/2019

            var r = _db.EmployabilityPlanActivityBridges
                       .Where(i => i.EmployabilityPlanId == epId)
                       .Select(i => i.Activity)
                       .ToList();
            return (r);
            
        }

        public IActivity NewActivity(int epId)
        {
            var activity = new Activity();
            var epab = new EmployabilityPlanActivityBridge
                       {
                           EmployabilityPlanId = epId
                       };

            activity.EmployabilityPlanActivityBridges = new List<EmployabilityPlanActivityBridge> { epab };

            _db.Activities.Add(activity);
            return activity;
        }

        public INonSelfDirectedActivity NewSelfDirectedActivity(IActivity activity, string user)
        {
            INonSelfDirectedActivity sda = new NonSelfDirectedActivity();
            sda.Activity = activity;
            sda.ModifiedBy   = user;
            sda.ModifiedDate = DateTime.Now;
            sda.IsDeleted    = false;
            _db.NonSelfDirectedActivities.Add((NonSelfDirectedActivity)sda);
            return sda;
        }

        public void DeleteActivity(int id)
        {
            var item = _db.Activities.SingleOrDefault(x => x.Id == id);
            var s = _db.NonSelfDirectedActivities.Where(x => x.ActivityId == id);
            var c = _db.ActivityContactBridges.Where(x => x.ActivityId == id);
            var sc = _db.ActivitySchedules.Where(x => x.ActivityId == id);
            var activityScheduleIds = sc.Select(i => (int?) i.Id).ToList();
            var f = _db.ActivityScheduleFrequencyBridges.Where(x => activityScheduleIds.Contains(x.ActivityScheduleId));

            if (s.Any()) _db.NonSelfDirectedActivities.RemoveRange(s);
            if (c.Any()) _db.ActivityContactBridges.RemoveRange(c);
            if (sc.Any()) _db.ActivitySchedules.RemoveRange(sc);
            if (f.Any()) _db.ActivityScheduleFrequencyBridges.RemoveRange(f);
            if (item != null) _db.Activities.Remove(item);
        }


        public string GetActivityLocationName(int? id)
        {
            var item = _db.ActivityLocations.Single(x => x.Id == id);
            return item.Name;
        }

        public IActivity GetActivity (int id)
        {
            var activity = _db.Activities.SingleOrDefault(i => i.Id == id);
            return activity;
        }

        public INonSelfDirectedActivity GetSelfDirectedActivity (int activityId)
        {
            var nonSelfDirectedActivity = _db.NonSelfDirectedActivities.SingleOrDefault(i => i.ActivityId == activityId);
            return nonSelfDirectedActivity;
        }

        public IEnumerable<IActivitySchedule> WhereActivitySchedules(Expression<Func<IActivitySchedule, bool>> clause)
        {
            try
            {
                return _db.ActivitySchedules.Where(clause);
            }
            catch (NullReferenceException)
            {
                return new List<IActivitySchedule>();
            }
        }

        public IEnumerable<IActivityScheduleFrequencyBridge> WhereActivityScheduleFrequencies(Expression<Func<IActivityScheduleFrequencyBridge, bool>> clause)
        {
            try
            {
                return _db.ActivityScheduleFrequencyBridges.Where(clause);
            }
            catch (NullReferenceException)
            {
                return new List<IActivityScheduleFrequencyBridge>();
            }
        }

        public IActivitySchedule NewActivitySchedule(int activityId)
        {
            var schedule = new ActivitySchedule();
            schedule.ActivityId = activityId;
            _db.ActivitySchedules.Add(schedule);
            return schedule;
        }

        public IActivitySchedule GetActivitySchedule(int id)
        {
            var r = _db.ActivitySchedules?.FirstOrDefault(x => x.Id == id);

            return (r);
        }

        public IActivityScheduleFrequencyBridge NewActivityScheduleFrequencyBridge(IActivitySchedule schedule)
        {
            IActivityScheduleFrequencyBridge frequency = new ActivityScheduleFrequencyBridge();
            frequency.ActivitySchedule = schedule;
            _db.ActivityScheduleFrequencyBridges.Add((ActivityScheduleFrequencyBridge)frequency);
            return frequency;
        }

        public IActivityScheduleFrequencyBridge GetActivityScheduleFrequencyBridge(int id)
        {
            var r = _db.ActivityScheduleFrequencyBridges?.FirstOrDefault(x => x.Id == id);

            return (r);
        }

        public void DeleteActivitySchedules(IEnumerable<int> ids)
        {
            var deleteActivitySchedules = _db.ActivitySchedules.Where(i => ids.Contains(i.Id));

            var activityScheduleIds               = ids.Select(i => (int?) i).ToList();
            var deleteActivityScheduleFrequencies = _db.ActivityScheduleFrequencyBridges.Where(i => activityScheduleIds.Contains(i.ActivityScheduleId));

            _db.ActivityScheduleFrequencyBridges.RemoveRange(deleteActivityScheduleFrequencies);
            _db.ActivitySchedules.RemoveRange(deleteActivitySchedules);
        }

        public void DeleteActivityScheduleFrequencies(IEnumerable<int> ids)
        {
            var deleteActivityScheduleFrequencies = _db.ActivityScheduleFrequencyBridges.Where(i => ids.Contains(i.Id));

            _db.ActivityScheduleFrequencyBridges.RemoveRange(deleteActivityScheduleFrequencies);
        }

        public void DeleteSelfDirectedActivity(int id)
        {
            var item = _db.NonSelfDirectedActivities.Single(x => x.Id == id);
            _db.NonSelfDirectedActivities.Remove(item);

        }

        public IEnumerable<IFrequencyType> FrequencyTypes()
        {
            var frequencyTypes = (from ft in _db.FrequencyTypes orderby ft.SortOrder select ft);
            return frequencyTypes;
        }

        public IEnumerable<IFrequency> WeeklyFrequencies()
        {
            var weeklyFrequencies = _db.Frequencies.Where(i => i.Code == "WK").OrderBy(i => i.SortOrder).ToList();
            return weeklyFrequencies;
        }

        public IEnumerable<IFrequency> MonthlyFrequencies()
        {
            var monthlyFrequencies = _db.Frequencies.Where(i => i.Code == "MR").OrderBy(i => i.SortOrder).ToList();
            return monthlyFrequencies;
        }
        #endregion
    }
}
