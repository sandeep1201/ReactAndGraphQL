using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IRequestForAssistanceStatus : ICommonDelModel
    {
        string                             Name                  { get; set; }
        int                                SortOrder             { get; set; }

        ICollection<IRequestForAssistance> RequestsForAssistance { get; set; }
    }
}
