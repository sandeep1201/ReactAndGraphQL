using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository
    {
       public IPendingCharge NewPendingCharge(ILegalIssuesSection parentSection, string user)
        {
            IPendingCharge p = new PendingCharge();
            p.LegalIssuesSection = parentSection;
           p.IsDeleted = false;
            _db.PendingCharges.Add((PendingCharge)p);

            return p;
        }

        public void DeletePendingCharge(IPendingCharge PendingCharge)
        {
            _db.PendingCharges.Remove(PendingCharge as PendingCharge);
        }
    }
}
