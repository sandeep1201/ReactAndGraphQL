using Dcf.Wwp.Data.Sql.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GoalContract
    {
        public string                 BeginDate           { get; set; }
        public string                 Details             { get; set; }
        public int                    Id                  { get; set; }
        public int?                   GoalTypeId          { get; set; }
        public string                 GoalTypeName        { get; set; }
        public string                 Name                { get; set; }
        public string                 EndDate             { get; set; }
        public int?                   EndReasonId         { get; set; }
        public string                 EndReasonName       { get; set; }
        public string                 EndReasonDetails    { get; set; }
        public bool?                  IsGoalEnded         { get; set; }
        public string                 ModifiedBy          { get; set; }
        public string                 Program             { get; set; }
        public int                    EmployabilityPlanId { get; set; }
        public DateTime               ModifiedDate        { get; set; }
        public List<GoalStepContract> GoalSteps           { get; set; }
    }
}
