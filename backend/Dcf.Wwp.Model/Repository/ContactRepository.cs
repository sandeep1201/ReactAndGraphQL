using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Cww;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IContactRepository
    {
        public IContact ContactById(int? contactId)
        {
            return (from x in _db.Contacts where x.Id == contactId where x.IsDeleted == false select x).SingleOrDefault();
        }

        public IEnumerable<IContact> AllContactsByParticipant(int participantId)
        {
            return from x in _db.Contacts where x.ParticipantId == participantId select x;
        }

        //public ICollection<IContact> ContactsByPin(decimal pinNumber)
        //{
        //	var cc = (from x in _db.Contacts where x.Participant.PinNumber == pinNumber select x).ToList();
        //	return cc;
        //}

        public IContact NewContact(IParticipant participant, string user)
        {
            IContact c = new Contact();
            c.ParticipantId = participant?.Id;
            c.ModifiedDate = DateTime.Now;
            c.ModifiedBy = user;
            c.IsDeleted = false;
            _db.Contacts.Add((Contact)c);

            return c;
        }

        public bool DeleteContact(int id, string user)
        {
            var dc             = new DeleteContact();
            var deletedContact = _db.SP_DeleteContact(id, user).FirstOrDefault();
            dc.Return_Value = deletedContact?.Return_Value;
            return (dc.Return_Value == 0);
        }
    }
}
