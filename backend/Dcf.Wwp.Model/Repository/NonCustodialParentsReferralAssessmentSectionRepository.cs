using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialParentsReferralAssessmentSectionRepository
    {
        public INonCustodialParentsReferralAssessmentSection NewNonCustodialParentsReferralAssessmentSection(
            IInformalAssessment parentAssessment, string user)
        {
            var section = new NonCustodialParentsReferralAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.NonCustodialParentsReferralAssessmentSection = section;
            _db.NonCustodialParentsReferralAssessmentSections.Add(section);

            return section;
        }
    }
}