using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ILanguageSectionRepository
    {
        public ILanguageSection NewLanguageSection(IParticipant parentParticipant, string user)
        {
            var section = new LanguageSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            section.ParticipantId = parentParticipant.Id;
            _db.LanguageSections.Add(section);

            return section;
        }
    }
}