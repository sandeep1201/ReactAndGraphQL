using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IEmployabilityPlanViewModel
    {
        #region Properties

        #endregion

        #region Methods

        List<EmployabilityPlanContract> GetEmployabilityPlans(string                      pin);
        List<ActivityContract>          GetActivitesForEp(int                             epId);
        ActivityContract                GetActivity(int                                   activityId);
        List<GoalContract>              GetGoalsForEp(int                                 epId);
        EmployabilityPlanContract GetEpById(int                                     epId);
        bool                            IsValidEp(int                                     epId);
        EmployabilityPlanContract       UpsertEmployabilityPlan(EmployabilityPlanContract employabilityPlanContract, string           pin);
        //ActivityContract                UpsertActivity(IGoogleApi                         googleApi,                 ActivityContract activityContract, string pin, int epId, string user);
        ActivityContract                UpsertActivity(ActivityContract activityContract, string pin, int epId);
        bool                            DeleteActivity(string                             pin,                       int              activityId);
        GoalContract                    UpsertGoal(GoalContract                           goalContract,              string           pin, int epId);
        bool                            DeleteGoal(string                                 pin,                       int              goalId);
        bool DeleteEP(string pin, int epId);

        List<SupportiveServiceContract> GetSupportiveService(string                             pin,                       int    id);
        List<SupportiveServiceContract> UpsertSupportiveService(List<SupportiveServiceContract> supportiveServiceContract, string pin, int epId);
        GoalContract                    GetGoal(int                                             goalId);

        EmployabilityPlanContract SubmitEP(string pin, int epId);

        #endregion
    }
}
