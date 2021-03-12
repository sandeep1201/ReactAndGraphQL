using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IOfficeRepository
    {
        public List<IOffice> GetOffices()
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Offices.Where(x => ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate)
                                     && ((x.ContractArea.InActivatedDate == null || x.ContractArea.InActivatedDate >= currentDate) && x.ContractArea.ActivatedDate <= currentDate)
                                     && ((x.ContractArea.Organization.InActivatedDate == null || x.ContractArea.Organization.InActivatedDate >= currentDate) && x.ContractArea.Organization.ActivatedDate <= currentDate))
                      .Include(x => x.ContractArea.Organization).ToList<IOffice>();
        }

        public IEnumerable<IOffice> MilwaukeeOffices()
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Offices.Where(x => (x.CountyAndTribe.CountyNumber == County.Milwaukee && x.ContractArea.Organization != null)
                                          && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate));
        }

        public IEnumerable<IOffice> MilwaukeeOfficesByProgramCode(string programCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Offices.Where(x => (x.ContractArea.EnrolledProgram.ProgramCode == programCode
                                          && x.CountyAndTribe.CountyNumber == County.Milwaukee && x.ContractArea.Organization != null)
                                          && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate));
        }


        public IEnumerable<IOffice> OfficesByOrganizationCode(string entSecCode)
        {
            //var q = _db.Organizations.Where(i => i.EntsecAgencyCode == entSecCode).SelectMany(i => i.ContractAreas.SelectMany(j => j.Offices));
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var q = _db.Offices.Where(x => (x.ContractArea.Organization.EntsecAgencyCode == entSecCode)
                                           && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate));
            return (q);

            // original query:
            //_db.Offices.Where(x => x.ContractArea.Organization.EntsecAgencyCode == entSecCode);

            // the first here produces a better SQL with no LEFT OUTER JOINs 
            //return _db.Organizations.Where(i => i.EntsecAgencyCode == entSecCode).SelectMany(i => i.ContractAreas).SelectMany(i => i.Offices);    // <-- No LEFT OUTER JOINS
            //return _db.ContractAreas.Where(i => i.Organization.EntsecAgencyCode == entSecCode).SelectMany(i => i.Offices);                        // <-- One OUTER LEFT JOIN
            //return _db.Offices.Where(x => x.ContractArea.Organization.EntsecAgencyCode == entSecCode);  //                                        // <-- two LEFT OUTER JOINS
        }

        public IOffice GetOfficeByNumberAndProgram(string officeNumber, int programId)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var r = _db.Offices.FirstOrDefault(i => (i.OfficeNumber.ToString() == officeNumber && i.ContractArea.EnrolledProgram.Id == programId)
                                                    && ((i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate));

            return (r);

            //return _db.Offices.AsNoTracking().SingleOrDefault(x => x.OfficeNumber.ToString() == officeNumber);
        }

        public IOffice GetOfficeByNumberAndProgramCode(int officeNumber, string programCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var r = _db.Offices.FirstOrDefault(i => (i.OfficeNumber == officeNumber && i.ContractArea.EnrolledProgram.ProgramCode == programCode)
                                                    && ((i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate));

            return (r);
        }

        public IEnumerable<IOffice> GetOfficesByCountyAndProgramCode(int countyandTribeId, string programCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var r = _db.Offices.Where(i => (i.CountyandTribeId == countyandTribeId && i.ContractArea.EnrolledProgram.ProgramCode == programCode)
                                           && ((i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate));

            return (r);
        }

        public IEnumerable<IOffice> GetOfficesByContractAreaAndProgramCode(int contractAreaId, string programCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var r = _db.Offices.Where(i => (i.ContractAreaId == contractAreaId && i.ContractArea.EnrolledProgram.ProgramCode == programCode)
                                           && ((i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate));

            return (r);
        }


        public IOffice GetOfficeById(int id)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Offices.SingleOrDefault(x => (x.Id == id)
                                                    && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate));
        }

        public IOffice GetOfficeByNumber(int officeNumber)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Offices.SingleOrDefault(x => (x.OfficeNumber == officeNumber)
                                                    && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate));
        }

    }
}
