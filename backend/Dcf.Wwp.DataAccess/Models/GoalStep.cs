using System;
using Dcf.Wwp.DataAccess.Base;
using Newtonsoft.Json;

namespace Dcf.Wwp.DataAccess.Models
{
    public class GoalStep : BaseEntity
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

        [JsonIgnore]
        public virtual Goal Goal { get; set; }

        #endregion
    }
}
