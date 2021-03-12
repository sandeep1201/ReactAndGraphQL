using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IAlternateMailingAddressRepository
    {
        public IAlternateMailingAddress NewAlternateMailingAddress(IParticipantContactInfo parent)
        {
            var x = new AlternateMailingAddress() as IAlternateMailingAddress;
            parent.AlternateMailingAddress = x;
            _db.AlternateMailingAddresses.Add((AlternateMailingAddress) x);
            return x;
        }
    }
}
