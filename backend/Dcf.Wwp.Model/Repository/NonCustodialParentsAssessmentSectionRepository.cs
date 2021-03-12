using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialParentsAssessmentSectionRepository
    {
        public INonCustodialParentsAssessmentSection NewNonCustodialParentsAssessmentSection(IInformalAssessment parentAssessment,
            string user)
        {
            var section = new NonCustodialParentsAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.NonCustodialParentsAssessmentSection = section;
            _db.NonCustodialParentsAssessmentSections.Add(section);

            return section;
        }
    }
}