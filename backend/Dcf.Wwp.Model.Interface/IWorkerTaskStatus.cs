using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWorkerTaskStatus : ICommonModelFinal
    {
        string    Code            { get; set; }
        string    Name            { get; set; }
        int       SortOrder       { get; set; }
        bool      IsSystemUseOnly { get; set; }
        DateTime  EffectiveDate   { get; set; }
        DateTime? EndDate         { get; set; }
    }
}
