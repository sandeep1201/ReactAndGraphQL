using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class AgencyViewModel : BaseViewModel
    {
        private readonly IAuthUser _authUser;

        public AgencyViewModel(IRepository repo, IAuthUser authUser)
            : base(repo, authUser)
        {
            _authUser = authUser;
        }

        /// <summary>
        ///  Returns all workers in agency grouped by program. ProgramCode is optional. If programCode is supplied, 
        ///  only those that program's workers are returned.
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="programCode"></param>
        /// <returns></returns>
        public List<ProgramWorkersContract> GetWorkersByAgencyCode(string orgCode, string programCode)
        {
            if (string.IsNullOrEmpty(orgCode)) return null;

            var org = Repo.GetOrganizationByCode(orgCode);
            if (org == null) return null;

            var list          = new List<ProgramWorkersContract>();
            var associatedOrg = Repo.GetAssociatedOrganization(org);
            var workers       = Repo.GetWorkersByOrganization(orgCode).ToList();

            IEnumerable<IEnrolledProgram> enrolledPrograms;
            var                           contractAreas = org.ContractAreas;
            // If we have the programCode only get workers for that program.
            if (!string.IsNullOrEmpty(programCode))
                enrolledPrograms = contractAreas.GroupBy(x => x.EnrolledProgram.Id).Select(g => g.First().EnrolledProgram).Where(z => z.ProgramCode.Trim().ToLower() == programCode.ToLower());
            else
            {
                // Loop through all the distinct programs unless programCode is specified.
                if (associatedOrg != null)
                {
                    var associatedContractAreas = Repo.GetContractArea(associatedOrg.ContractAreaId);
                    if (associatedContractAreas.Any())
                    {
                        workers = Repo.GetWorkersByOrganization(associatedContractAreas.FirstOrDefault()?.Organization.EntsecAgencyCode).ToList();
                        associatedContractAreas.ForEach(i => contractAreas.Add(i));
                    }
                }

                enrolledPrograms = contractAreas.GroupBy(x => x.EnrolledProgram.Id).Select(g => g.First().EnrolledProgram);
            }

            if (!workers.Any()) return list;

            foreach (var ep in enrolledPrograms)
            {
                var pwc = new ProgramWorkersContract();
                pwc.ProgramName = ep.Name;
                pwc.ProgramCd   = ep.ProgramCode;

                // HACK: Major hack for now
                // TODO: We need to ask the security service what roles have access to the given program.
                // select * from sec.Role where Code LIKE '%CFMGR%'
                var roleCode = string.Empty;

                // select distinct ProgramCode from wwp.EnrolledProgram
                switch (ep.ProgramCode.Trim())
                {
                    case "WW":
                    case "LF":
                        roleCode = "FEP";
                        break;

                    case "TMJ":
                        roleCode = "TMJ";
                        break;

                    case "TJ":
                        roleCode = "TJ";
                        break;

                    case "CF":
                        roleCode = "CFMGR";
                        break;
                    case "FCD":
                        roleCode = "FCMGR";
                        break;
                }

                if (!string.IsNullOrEmpty(roleCode))
                    pwc.AgencyWorkers = workers.Where(x => !string.IsNullOrEmpty(x.Roles) && x.Roles.Contains(roleCode)).Select(w => new WorkerContract()
                                                                                                                                     {
                                                                                                                                         Id            = w.Id,
                                                                                                                                         WamsId        = w.WAMSId,
                                                                                                                                         WorkerId      = w.MFUserId,
                                                                                                                                         Wiuid         = w.WIUID,
                                                                                                                                         FirstName     = w.FirstName,
                                                                                                                                         MiddleInitial = w.MiddleInitial,
                                                                                                                                         LastName      = w.LastName,
                                                                                                                                         Organization  = org.AgencyName,
                                                                                                                                         IsActive      = w.WorkerActiveStatusCode == "ACTIVE"
                                                                                                                                     }).ToList();

                list.Add(pwc);
            }

            return list;
        }

        public List<WorkerContract> GetWorkersByAuthorization(string agencyCode, string authcode)
        {
            if (String.IsNullOrEmpty(agencyCode) || String.IsNullOrEmpty(authcode)) return null;
            var workers = Repo.GetWorkersByAuthToken(agencyCode, authcode);

            return workers?.Select(item => new WorkerContract()
                                           {
                                               Id        = item.Id,
                                               WamsId    = item.WAMSId,
                                               WorkerId  = item.MFUserId,
                                               Wiuid     = item.WIUID,
                                               FirstName = item.FirstName,
                                               LastName  = item.LastName,
                                               IsActive  = item.WorkerActiveStatusCode == "ACTIVE"
                                           }).ToList();
        }

        public List<OfficeContract> GetTransferDestinations(string officeNumber)
        {
            List<OfficeContract> list = null;

            // Look up the office we are using as thte basis for where participants
            // can be transferred to.
            var office = Repo.GetOfficeByNumberAndProgram(officeNumber, 0); // Depricalled

            if (office != null)
            {
                var counties = Repo.GetCountyAndTribes().ToList();
                list = new List<OfficeContract>();

                // Check if we are dealing with Milwuakee county or not.
                //if (office.CountyNumber == County.Milwaukee) //TODO: Check with chris is this intended or should it be office->contract
                if (office.CountyAndTribe.CountyNumber == County.Milwaukee)
                {
                    var mkeOffices = Repo.MilwaukeeOffices();
                    // If so, get all the other MKE offices.
                    var currentDate = _authUser.CDODate ?? DateTime.Today;
                    list.AddRange(mkeOffices.Where(x => (x.OfficeNumber != office.OfficeNumber)
                                                        && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate))
                                            .OrderBy(x => x.OfficeNumber).Select(otherOffice => OfficeContract.Create(otherOffice, counties)));
                }
                else
                    if (office.ContractArea?.Organization != null)
                    {
                        // If not, get all the other offices that have the same Agency CODE (FSC for example has
                        // two actual agency records, so we use the code to "join" them).
                        var currentDate   = _authUser.CDODate ?? DateTime.Today;
                        var agencyOffices = Repo.OfficesByOrganizationCode(office.ContractArea?.Organization?.EntsecAgencyCode);
                        list.AddRange(agencyOffices.Where(x => (x.OfficeNumber != office.OfficeNumber)
                                                               && ((x.InactivatedDate == null || x.InactivatedDate >= currentDate) && x.ActiviatedDate <= currentDate))
                                                   .OrderBy(x => x.OfficeNumber).Select(otherOffice => OfficeContract.Create(otherOffice, counties)));
                    }
                    else
                    {
                        // Hmmm... this should not happen.
                        // TODO: Log a warning.
                    }
            }

            return list;
        }

        public List<OfficeContract> GetOfficesWithAccess(string user)
        {
            var worker = Repo.WorkerByWamsId(user);

            if (worker == null)
                return new List<OfficeContract>();

            // Look up the office we are using as thte basis for where participants
            // can be transferred to.
            var offices  = Repo.OfficesByOrganizationCode(worker.Organization?.EntsecAgencyCode);
            var counties = Repo.GetCountyAndTribes().ToList();

            return offices?.Select(item => OfficeContract.Create(item, counties)).ToList();
        }


        public List<AgencyContract> GetAgencies()
        {
            var orgs = Repo.GetOrganizations().OrderBy(x => x.AgencyName).ToList();

            return orgs.Select(item => AgencyContract.Create(item, orgs)).ToList();
        }
    }
}
