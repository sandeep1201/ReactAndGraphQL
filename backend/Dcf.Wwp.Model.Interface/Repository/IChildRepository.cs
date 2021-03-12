using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IChildRepository
    {
        IEnumerable<IChild> AllChildrenForParticipant(int participantId);

        IChild NewChild();
    }
}
