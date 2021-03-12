using System;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Data.Sql.Model;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IPostSecondaryEducationRepository
    {
        public IPostSecondaryEducationSection NewPostSecondaryEducationSection(IParticipant parentParticipant, string user)
        {
            var section = new PostSecondaryEducationSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user,
                ParticipantId = parentParticipant.Id
            };
            _db.PostSecondaryEducationSections.Add(section);

            return section;
        }
    }
}
