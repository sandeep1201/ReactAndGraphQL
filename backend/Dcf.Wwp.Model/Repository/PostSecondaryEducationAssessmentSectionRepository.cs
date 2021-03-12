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
        public IPostSecondaryEducationAssessmentSection NewPostSecondaryEducationAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new PostSecondaryEducationAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.PostSecondaryEducationAssessmentSection = section;
            _db.PostSecondaryEducationAssessmentSections.Add(section);
            return section;
        }
    }
}
