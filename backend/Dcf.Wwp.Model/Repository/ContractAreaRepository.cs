using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once RedundantExtendsListEntry
    public partial class Repository : IContractAreaRepository
    {
        public List<IContractArea> GetContractAreasByProgramCode(string programCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.ContractAreas.AsNoTracking().Where(x => (x.EnrolledProgram.ProgramCode.ToLower().Trim() == programCode.ToLower().Trim())
                                                               && ((x.InActivatedDate == null || x.InActivatedDate >= currentDate) && x.ActivatedDate <= currentDate))
                      .ToList<IContractArea>();
        }

        public List<IContractArea> GetContractAreasByProgramCodeAndOrganizationId(string programCode, int orgId)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.ContractAreas.AsNoTracking()
                      .Where(x => (x.EnrolledProgram.ProgramCode.ToLower().Trim() == programCode.ToLower().Trim() && x.OrganizationId == orgId)
                                  && ((x.InActivatedDate == null || x.InActivatedDate >= currentDate)             && x.ActivatedDate  <= currentDate))
                      .ToList<IContractArea>();
        }

        public List<IContractArea> GetContractArea(int id)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.ContractAreas.Where(x => x.Id == id && (x.InActivatedDate == null || x.InActivatedDate >= currentDate) && x.ActivatedDate <= currentDate).ToList<IContractArea>();
        }
    }
}
