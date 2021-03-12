using System.Collections.Generic;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IChildRepository
    {
        public IEnumerable<IChild> AllChildrenForParticipant(int participantId)
        {
            // TODO: Fill this in when we implement using the ParticipantChildRelationshipBridge
            throw new System.NotImplementedException();
        }

        public IChild NewChild()
        {
            var c = new Child();
            _db.Children.Add(c);

            return c;
        }
    }
}