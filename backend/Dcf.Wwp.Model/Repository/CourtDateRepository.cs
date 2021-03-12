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
        public ICourtDate NewCourtDate(ILegalIssuesSection parentSection, string user)
        {
            ICourtDate cd = new CourtDate();
            cd.LegalIssuesSection = parentSection;
            cd.IsDeleted = false;
            _db.CourtDates.Add((CourtDate)cd);
            return cd;
        }

        public void DeleteCourtDate(ICourtDate courtDate)
        {
            _db.CourtDates.Remove(courtDate as CourtDate);
        }
    }
}
