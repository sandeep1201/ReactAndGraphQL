using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface IActionNeeded : ICommon2Model, ICloneable
    {
        int ParticipantId { get; set; }
        int ActionNeededPageId { get; set; }
        bool IsNoActionNeeded { get; set; }

        IActionNeededPage ActionNeededPage { get; set; }
        //IParticipant Participant { get; set; }
        ICollection<IActionNeededTask> ActionNeededTasks { get; set; }
    }
}
