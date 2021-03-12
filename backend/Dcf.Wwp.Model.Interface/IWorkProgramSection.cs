using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface IWorkProgramSection : ICommonDelModel, ICloneable
    {
        bool? IsInOtherPrograms { get; set; }
        string Notes { get; set; }

        ICollection<IInvolvedWorkProgram> InvolvedWorkPrograms { get; set; }
        ICollection<IInvolvedWorkProgram> AllInvolvedWorkPrograms { get; set; }
    }
}