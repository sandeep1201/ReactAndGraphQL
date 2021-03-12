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
        public IBarrierAssessmentSection NewBarrierAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new BarrierAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.BarrierAssessmentSection = section;
            _db.BarrierAssessmentSections.Add(section);

            return section;
        }
    }
}
