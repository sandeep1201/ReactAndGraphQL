using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ILanguageAssessmentSectionRepository
    {
        public ILanguageAssessmentSection NewLanguageAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new LanguageAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.LanguageAssessmentSection = section;
            _db.LanguageAssessmentSections.Add(section);

            return section;
        }
    }
}