using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository: IAkaRepository
    {
        public IAKA NewAKA(IParticipant parent)
        {
            var aka = new AKA() as IAKA;
            aka.Participant = parent;
            aka.IsDeleted = false;
            _db.AKAs.Add((AKA)aka);
            return aka;
        }
    }
}
