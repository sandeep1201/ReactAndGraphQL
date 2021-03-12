using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using System;
using System.Linq;
using Dcf.Wwp.Model.Cww;
using Dcf.Wwp.Model.Interface.Cww;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IWorkProgramSection NewWorkProgramSection(IParticipant parentParticipant, string user)
        {
            var section = new WorkProgramSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user,
                ParticipantId = parentParticipant.Id
            };

            _db.WorkProgramSections.Add(section);

            return section;
        }

        public IFsetStatus CwwFsetStatus(string pin)
        {
            var fset = new FsetStatus();
            if (string.IsNullOrWhiteSpace(pin))
                return null;

            var fsetStatus = _db.SP_FSETStatus(pin, Database).FirstOrDefault();
            fset.CurrentStatusCode = fsetStatus?.CURRENT_STATUS_CD;
            fset.DisenrollmentDate = fsetStatus?.DISENROLLMENT_DATE;
            fset.DisenrollmentReasonCode = fsetStatus?.DISENROLLMENT_REASON_CD;
            fset.EnrollmentDate = fsetStatus?.ENROLLMENT_DATE;

            return fset;
        }
    }
}
