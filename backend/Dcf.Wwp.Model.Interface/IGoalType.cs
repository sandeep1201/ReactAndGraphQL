using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IGoalType : ICommonModelFinal, ICloneable
    {
        Int32  Id                { get; set; }
        String Name              { get; set; }
        Int32  EnrolledProgramId { get; set; }
        Int32 SortOrder         { get; set; }

        IEnrolledProgram   EnrolledProgram { get; set; }
        ICollection<IGoal> Goals           { get; set; }
    }
}
