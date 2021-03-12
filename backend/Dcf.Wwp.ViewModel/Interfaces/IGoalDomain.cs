using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IGoalDomain
    {
        #region Properties

        #endregion

        #region Methods

        GoalContract       GetGoal(int             id);
        List<GoalContract> GetGoalsForEP(int       epId);
        List<GoalContract> GetGoalsForPin(string   pin);
        bool               DeleteGoal(int    goalId, int epId, bool canCommit = true);
        GoalContract       UpsertGoal(GoalContract goalContract, string pin,    int epId);

        // void InsertGoal(GoalContract goalContract);
        // void UpdateGoal(GoalContract goalContract);

        #endregion
    }
}
