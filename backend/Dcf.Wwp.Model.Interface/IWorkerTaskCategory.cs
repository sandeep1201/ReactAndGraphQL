using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWorkerTaskCategory : ICommonModelFinal
    {
        string    Code            { get; set; }
        string    Name            { get; set; }
        int       SortOrder       { get; set; }
        bool      IsSystemUseOnly { get; set; }
        DateTime  EffectiveDate   { get; set; }
        DateTime? EndDate         { get; set; }
        string    Description     { get; set; }
        bool      IsWWLF          { get; set; }
        bool      IsCF            { get; set; }
        bool      IsTJTMJ         { get; set; }
        bool      IsFCDP          { get; set; }
        bool      IsEA            { get; set; }
    }
}
