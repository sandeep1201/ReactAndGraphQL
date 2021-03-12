using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ITransportationSectionMethodBridgeRepository
    {
        public ITransportationSectionMethodBridge NewTransportationSectionMethodBridge(ITransportationSection parent, string user)
        {
            ITransportationSectionMethodBridge mb = new TransportationSectionMethodBridge();

            mb.TransportationSectionId = parent.Id;
            mb.ModifiedBy = user;
            mb.ModifiedDate = DateTime.Now;
            mb.IsDeleted = false;

            _db.TransportationSectionMethodBridges.Add((TransportationSectionMethodBridge)mb);

            return mb;
        }
    }
}