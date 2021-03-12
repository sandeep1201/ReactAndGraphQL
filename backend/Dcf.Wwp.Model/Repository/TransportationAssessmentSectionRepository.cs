using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    partial class Repository : ITransportationAssessmentSectionRepository
    {
        public ITransportationAssessmentSection NewTransportationAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new TransportationAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.TransportationAssessmentSection = section;
            _db.TransportationAssessmentSections.Add(section);

            return section;
        }

    }
}