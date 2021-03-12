using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once RedundantExtendsListEntry
    public partial class Repository : IOrganizationRepository
    {
        public IEnumerable<IOrganization> GetOrganizations()
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Organizations.Where(x => (x.InActivatedDate == null || x.InActivatedDate >= currentDate) && x.ActivatedDate <= currentDate)
                      .ToList<IOrganization>();
        }

        public IOrganization GetOrganizationByOfficeNumber(short? officeNumber)
        {
            if (officeNumber == null) return null;

            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Offices
                      .Where(i => (i.OfficeNumber == officeNumber)
                                  && ((i.InactivatedDate                           == null || i.InactivatedDate                           >= currentDate) && i.ActiviatedDate                          <= currentDate)
                                  && ((i.ContractArea.InActivatedDate              == null || i.ContractArea.InActivatedDate              >= currentDate) && i.ContractArea.ActivatedDate              <= currentDate)
                                  && ((i.ContractArea.Organization.InActivatedDate == null || i.ContractArea.Organization.InActivatedDate >= currentDate) && i.ContractArea.Organization.ActivatedDate <= currentDate))
                      .Select(i => i.ContractArea.Organization)
                      .FirstOrDefault(i => (i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate);
        }

        public IOrganization GetOrganizationByCode(string orgCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Organizations.SingleOrDefault(x => (x.EntsecAgencyCode == orgCode) && ((x.InActivatedDate == null || x.InActivatedDate >= currentDate) && x.ActivatedDate <= currentDate));
        }

        public IAssociatedOrganization GetAssociatedOrganization(IOrganization org)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var associatedOrganization = _db.AssociatedOrganizations.FirstOrDefault(i => i.OrganizationId == org.Id
                                                                                         && (i.InactivatedDate == null || i.InactivatedDate >= currentDate)
                                                                                         && i.ActivatedDate <= currentDate);
            return associatedOrganization;
        }

        public IEnumerable<IOrganization> GetOrganizationsForProgram(string programCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.ContractAreas
                      .AsNoTracking()
                      .Where(i => i.EnrolledProgram.ProgramCode.Equals(programCode) && !i.IsDeleted && !i.EnrolledProgram.IsDeleted && !i.Organization.IsDeleted
                                  && ((i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate))
                      .Select(i => i.Organization)
                      .Where(i => (i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate)
                      .Distinct();
        }

        public IEnumerable<IOrganization> GetOrganizationsByProgramId(string programId)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var progId      = int.Parse(programId);
            return _db.ContractAreas
                      .AsNoTracking()
                      .Where(i => i.EnrolledProgram.Id == progId && !i.IsDeleted && !i.EnrolledProgram.IsDeleted && !i.Organization.IsDeleted
                                  && ((i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate))
                      .Select(i => i.Organization)
                      .Where(i => (i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate)
                      .Distinct();
        }
    }
}
