using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IAgencyRepository
    {
        public List<IAgency> GetAgencies()
        {
            return _db.Agencies.AsNoTracking().ToList<IAgency>();
        }

        public IAgency GetAgencyByOfficeNumber(string officeNumber)
        {
            var agency = new Agency();
            if (officeNumber == null) return null;
            var officeId = _db.Offices?.Where(x => x.OfficeNumber.ToString() == officeNumber).Select(y => y.Id)
                .FirstOrDefault();

            if (officeId == null) return null;

            var agencyId = _db.WPOrganizations?.Where(x => x.officeId == officeId).Select(y => y.AgencyId)
                .FirstOrDefault();

            return _db.Agencies?.SingleOrDefault(x => x.Id == agencyId);
        }


        public IEnumerable<IAgency> GetContractorsForProgram(string programName)
        {
            var r = _db.Contractors
                       .AsNoTracking()
                       .Where(i => i.EnrolledProgram.ProgramCode == programName && !i.Agency.IsDeleted)
                       .Select(i => i.Agency);

            return (r);
        }
    }
}
