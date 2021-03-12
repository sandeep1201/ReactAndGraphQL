using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IGoalTypeRepository
    {
        IEnumerable<IGoalType> GoalTypes();
    }
}
