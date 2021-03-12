using System;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Interface;
using Constants = Dcf.Wwp.Model.Interface.Constants;
using DCF.Common.Exceptions;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IInformalAssessmentRepository
    {
        public IInformalAssessment NewInformalAssessment(int participantId, bool isSubsequent, string user)
        {
            var ia = new InformalAssessment
            {
                ParticipantId = participantId,
                AssessmentTypeId = isSubsequent ? Constants.AssessmentType.SubsequentId : Constants.AssessmentType.InitialId,
                CreatedDate = _authUser.CDODate ?? DateTime.Now,
                ModifiedBy = user,
                ModifiedDate = DateTime.Now
            };

            _db.InformalAssessments.Add(ia);

            return ia;
        }

        public IInformalAssessment InformalAssessmentById(int id)
        {
            return (from x in _db.InformalAssessments where x.Id == id select x).SingleOrDefault();
        }

        public void SP_DB2_InformalAssessment_Update(decimal? pinNumber, string mFWorkerId)
        {
            
            _db.SP_DB2_InformalAssessment_Update(pinNumber, _authUser.CDODate ?? DateTime.Today, mFWorkerId, Database);
        }

        public IInformalAssessment GetMostRecentAssessment(IParticipant part)
        {
            var mostRecentAssessment = part.InProgressInformalAssessment 
                                        ?? _db.InformalAssessments.OrderByDescending(i => i.EndDate).FirstOrDefault(i => i.ParticipantId == part.Id && i.EndDate != null && !i.IsDeleted);
            return mostRecentAssessment;
        }
    }
}
