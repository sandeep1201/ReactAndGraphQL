using System;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IParticipantEnrolledProgramRepository
    {
        public IParticipantEnrolledProgram NewPep(IRequestForAssistance rfa, string user)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var office = _db.Offices.Where(i => (i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate)
                            .FirstOrDefault(i => i.Id == rfa.OfficeId);

            var newPep = new ParticipantEnrolledProgram
                         {
                             ParticipantId               = rfa.ParticipantId,
                             EnrolledProgramId           = rfa.EnrolledProgramId,
                             EnrolledProgramStatusCodeId = 1, // Referred
                             ReferralDate                = DateTime.Now,
                             RequestForAssistanceId      = rfa.Id,
                             OfficeId                    = rfa.OfficeId,
                             ModifiedDate                = DateTime.Now,
                             ModifiedBy                  = user,
                             Office                      = office,
                         };

            _db.ParticipantEnrolledPrograms.Add(newPep);

            return (newPep);
        }

        /// <summary>
        /// For when we only have the pep id.
        /// 
        /// FYI: You may already have the pep off the participant.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IParticipantEnrolledProgram GetPepById(int id)
        {
            return _db.ParticipantEnrolledPrograms.FirstOrDefault(x => x.Id == id);
        }

        public ISP_MostRecentFEPFromDB2_Result GetMostRecentFepDetails(string pin)
        {
            // GetMostRecentFepDetails returns null if first is not used.
            var r = _db.SP_MostRecentFEPFromDB2(pin, Database).FirstOrDefault() as ISP_MostRecentFEPFromDB2_Result;

            return (r) ;
        }
    }
}
