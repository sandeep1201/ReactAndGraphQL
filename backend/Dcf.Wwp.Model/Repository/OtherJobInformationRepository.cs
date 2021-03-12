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
    public partial class Repository : IOtherJobInformationRepository
    {
        public IOtherJobInformation OtherJobInformationById(int? id)
        {
            return (from x in _db.OtherJobInformations where x.Id == id where x.IsDeleted == false select x).SingleOrDefault();
        }

        public IOtherJobInformation NewOtherJobInformation(string user)
        {
            IOtherJobInformation ojb = new OtherJobInformation();
            ojb.ModifiedDate = DateTime.Now;
            ojb.ModifiedBy = user;
            ojb.IsDeleted = false;
            _db.OtherJobInformations.Add((OtherJobInformation)ojb);
            return ojb;
        }
    }
}
