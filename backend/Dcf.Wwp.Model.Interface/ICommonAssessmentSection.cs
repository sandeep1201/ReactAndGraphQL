using System;


namespace Dcf.Wwp.Model.Interface
{
    public interface ICommonAssessmentSection : ICommonDelModel
    {
        Nullable<bool> ReviewCompleted { get; set; }
    }
}