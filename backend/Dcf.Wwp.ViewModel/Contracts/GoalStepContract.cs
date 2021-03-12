using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GoalStepContract
    {
        public int    Id                  { get; set; }
        public string Details             { get; set; }
        public bool?  IsGoalStepCompleted { get; set; }
    }
}
