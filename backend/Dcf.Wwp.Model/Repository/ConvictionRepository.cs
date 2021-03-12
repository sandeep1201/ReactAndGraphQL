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
        public IConviction NewConviction(ILegalIssuesSection parentSection, string user)
        {
            IConviction c = new Conviction();
            c.LegalIssuesSection = parentSection;
            _db.Convictions.Add((Conviction)c);
            return c;
        }

       public void DeleteConviction(IConviction Conviction)
       {
            _db.Convictions.Remove(Conviction as Conviction);
        }
    }
}
