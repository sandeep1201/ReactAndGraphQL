using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IContactRepository
    {
        IContact ContactById(int? id);
        IEnumerable<IContact> AllContactsByParticipant(int participantId);
        IContact NewContact(IParticipant participant, string user);
        bool DeleteContact(int id, string user);
    }
}
