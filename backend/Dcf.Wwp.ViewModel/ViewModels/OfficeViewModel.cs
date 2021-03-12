using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class OfficeViewModel : BaseViewModel
    {
        private readonly IAuthUser _authUser;

        public OfficeViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
            _authUser = authUser;
        }

        /// <summary>
        /// Returns an office that offers Param-program in that county. 
        /// </summary>
        /// <param name="programCode"></param>
        /// <param name="countyOrTribeId"></param>
        /// <returns></returns>
        public WwpOfficeContract GetOfficeByCountyAndProgramCode(int countyOrTribeId, string programCode)
        {
            var county = Repo.GetCountyOrTribeById(countyOrTribeId);

            if (county == null)
                return null;

            var officesInCounty = Repo.GetOfficesByCountyAndProgramCode(county.Id, programCode).ToList();

            var off = officesInCounty.SingleOrDefault(i => i.ContractArea.Organization.EntsecAgencyCode == AuthUser.AgencyCode);

            if (off == null)
                return null;

            var office = new WwpOfficeContract
                         {
                             Id           = off.Id,
                             OfficeNumber = off.OfficeNumber,
                             OfficeName   = off.OfficeName,
                             CountyNumber = off.CountyAndTribe?.CountyNumber,
                             CountyName   = off.CountyAndTribe?.CountyName.Trim().ToTitleCase()
                         };

            return office;
        }


        /// <summary>
        /// Returns a list of all the offices a worker has access to for a given program.
        /// </summary>
        /// <param name="programCode"></param>
        /// <param name="wiuid"></param>
        /// <returns></returns>
        public IEnumerable<WwpOfficeContract> GetOfficesForProgramAndWorker(string programCode, string wiuid)
        {
            var worker = Repo.WorkerByWIUID(wiuid);
            if (worker?.Organization == null) return null;

            // Get the contract areas for the workers organization that match the desired program code.  It could be
            // one or multiple contract areas.
            //var contractAreas = worker.Organization.ContractAreas.Where(x => x.EnrolledProgram.ProgramCode.TrimAndLower() == programCode.TrimAndLower());
            var organization  = Repo.GetOrganizationByCode(AuthUser.AgencyCode);
            var contractAreas = Repo.GetContractAreasByProgramCodeAndOrganizationId(programCode, organization.Id);
            var list          = new List<WwpOfficeContract>();

            contractAreas.ForEach(contractArea =>
                                  {
                                      var currentDate = _authUser.CDODate ?? DateTime.Today;
                                      list.AddRange(contractArea.Offices.Where(i => (i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate)
                                                                .Select(off => new WwpOfficeContract
                                                                               {
                                                                                   Id               = off.Id,
                                                                                   OfficeNumber     = off.OfficeNumber,
                                                                                   OfficeName       = off.OfficeName,
                                                                                   CountyNumber     = off.CountyAndTribe?.CountyNumber,
                                                                                   CountyName       = off.CountyAndTribe?.CountyName.Trim().ToTitleCase(),
                                                                                   OrganizationCode = contractArea.Organization?.EntsecAgencyCode,
                                                                                   OrganizationName = contractArea.Organization?.AgencyName,
                                                                                   ProgramShortName = contractArea.EnrolledProgram.ShortName
                                                                               }));
                                  });

            return list.OrderBy(i => i.OfficeNumber);
        }


        public IEnumerable<WwpOfficeContract> GetTransferOfficesByProgramWorkerSourceOffice(string programCode, string workerMfId, int sourceOfficeNumber)
        {
            var worker = Repo.WorkerByMainframeId(workerMfId);
            if (worker?.Organization == null) return null;

            var office = Repo.GetOfficeByNumberAndProgramCode(sourceOfficeNumber, programCode);

            // Get the contract areas for the workers organization that match the desired program code BUT if the sourceOffice is milwaukee, only get W-2 contract areas.  
            // It could be one or multiple contract areas.
            // Tj and W-2 have a special case for when their source office's are in Milwaukee.
            var limitToMilwaukee = programCode == EnrolledProgram.W2ProgramCode && office != null && office.IsLocatedInMilwaukee;
            var contractAreas = limitToMilwaukee
                                    ? Repo.GetContractAreasByProgramCode(programCode).ToList()
                                    : worker.Organization.ContractAreas.Where(i => i.EnrolledProgram.ProgramCode.Trim() == programCode).ToList();

            var list = new List<WwpOfficeContract>();

            contractAreas.ForEach(contractArea =>
                                  {
                                      IEnumerable<IOffice> offices;
                                      // All TJ or W2 offices.
                                      var currentDate = _authUser.CDODate ?? DateTime.Today;

                                      if (limitToMilwaukee)
                                          offices = contractArea.Offices.Where(i => i.IsLocatedInMilwaukee &&
                                                                                    ((i.InactivatedDate == null || i.InactivatedDate >= currentDate)
                                                                                     && i.ActiviatedDate <= currentDate)).ToList();
                                      else
                                          offices = contractArea.Offices.Where(i => (i.InactivatedDate == null || i.InactivatedDate >= currentDate)
                                                                                    && i.ActiviatedDate <= currentDate).ToList().ToList();

                                      CreateOfficeContract(list, offices, contractArea);
                                  });

            return list.OrderBy(i => i.CountyName);
        }

        private void CreateOfficeContract(List<WwpOfficeContract> list, IEnumerable<IOffice> offices, IContractArea contractArea)
        {
            list.AddRange(offices.Select(i => new WwpOfficeContract
                                              {
                                                  Id               = i.Id,
                                                  OfficeNumber     = i.OfficeNumber,
                                                  OfficeName       = i.OfficeName,
                                                  CountyNumber     = i.CountyAndTribe?.CountyNumber,
                                                  CountyName       = i.CountyAndTribe?.CountyName.Trim().ToTitleCase(),
                                                  OrganizationCode = contractArea.Organization?.EntsecAgencyCode,
                                                  OrganizationName = contractArea.Organization?.AgencyName,
                                                  ProgramShortName = contractArea.EnrolledProgram.ShortName
                                              }).ToList());
        }
    }
}
