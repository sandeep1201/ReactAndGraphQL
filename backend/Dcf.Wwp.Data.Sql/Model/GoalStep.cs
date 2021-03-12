using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class GoalStep
    {
        #region Properties

        public int      GoalId              { get; set; }
        public string   Details             { get; set; }
        public bool?    IsGoalStepCompleted { get; set; }
        public bool     IsDeleted           { get; set; }
        public string   ModifiedBy          { get; set; }
        public DateTime ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Goal Goal { get; set; }

        #endregion
    }
}
