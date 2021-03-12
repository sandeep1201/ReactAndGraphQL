using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPopulationTypeDto
    {
        int              Id             { get; set; }
        string           Name           { get; set; }
        bool             DisablesOthers { get; set; }
        IEnumerable<int> DisabledIds    { get; set; }
    }
}
