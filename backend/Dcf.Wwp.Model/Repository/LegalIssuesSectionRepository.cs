using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository : ILegalIssuesSectionRepository
    {
        public ILegalIssuesSection NewLegalIssuesSection(IParticipant parentParticipant, string user)
        {
            var section = new LegalIssuesSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user,
                ParticipantId = parentParticipant.Id
            };
            _db.LegalIssuesSections.Add(section);

            return section;
        }

    }
}
