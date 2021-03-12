using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IActivityDomain
    {
        #region Properties

        #endregion

        #region Methods

        EmployabilityPlan         GetRecentSubmittedEP(string                  pin,        int  epId, int enrolledProgramId, bool considerEnded = false);
        ActivityContract          GetActivity(int                              activityId, int  employabilityPlanId);
        List<ActivityContract>    GetActivitiesForPin(string                   pin,        bool fromEvents = false);
        List<ActivityContract>    GetActivitiesForPep(string                   pin,        int  pepId);
        List<ActivityContract>    GetActivitiesForEp(int                       epId);
        List<EventsContract>      GetEvents(string                             pin,               int      epId, int programId, DateTime epBeginDate);
        bool                      DeleteActivity(string                        pin,               int      id,   int epId,      bool     fromEndEp);
        void                      DeleteActivityAndSchedules(int               epId,              Activity activity);
        PreSavingActivityContract PreSaveActivity(string                       pin,               int      epId,        string                                activityTypeId);
        ActivityContract          UpsertActivity(ActivityContract              activityContract,  string   pin,         int                                   epId, int  subepId);
        List<ActivityContract>    UpsertElapsedActivity(List<ActivityContract> activityContracts, string   pin,         int                                   epId, bool isDisenrollment, bool fromEpOverView);
        void                      EndActivityTransactionalSave(string          xml,               int?     epId = null, string                                programCd = null);
        void                      EndStatus(int                                epId,              decimal  pin,         IReadOnlyCollection<ActivityContract> contracts, DateTime updateTime);

        #endregion
    }
}
