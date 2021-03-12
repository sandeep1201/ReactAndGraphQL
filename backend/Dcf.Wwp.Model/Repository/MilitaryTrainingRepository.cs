using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using System;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IMilitaryTrainingSectionRepository
    {
        public IMilitaryTrainingSection NewMilitaryTrainingSection(IParticipant parentParticipant, string user)
        {
            var section = new MilitaryTrainingSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user,
                ParticipantId = parentParticipant.Id
            };
            _db.MilitaryTrainingSections.Add(section);

            return section;
        }

    }
}
