using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWpGeoAreaRepository
    {
        public GeoArea WpGeoAreaByOfficeNumber(short officeNumber, string programCode = "WW")
        {
            var geoArea = new GeoArea();

            if (officeNumber == 9999) // magic strings in here are Phase I HACK ;)
            {
                geoArea.AgencyCode   = "WI";
                geoArea.AgencyName   = "State";
                geoArea.OfficeNumber = 9999;
            }
            else
            {
                var currentDate = _authUser.CDODate ?? DateTime.Today;
                geoArea = _db.Offices.Where(i => (i.OfficeNumber == officeNumber && i.ContractArea.EnrolledProgram.ProgramCode == programCode)
                                            && ((i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate))
                             .Select(i => new GeoArea
                                          {
                                              Name         = i.ContractArea.ContractAreaName,
                                              AgencyCode   = i.ContractArea.Organization.EntsecAgencyCode,
                                              AgencyName   = i.ContractArea.Organization.AgencyName,
                                              OfficeNumber = i.OfficeNumber,
                                              CountyNumber = i.CountyAndTribe.CountyNumber
                                          }).FirstOrDefault();
            }

            return (geoArea);
        }

        public GeoArea WpGeoAreaByPin(decimal pin)
        {
            var geoArea = new GeoArea();

            var recentStatus      = new ObjectParameter("RecentStatus",      typeof(string));
            var referralDate      = new ObjectParameter("ReferralDate",      typeof(DateTime));
            var enrollmentDate    = new ObjectParameter("EnrollmentDate",    typeof(DateTime));
            var disEnrollmemtDate = new ObjectParameter("DisEnrollmemtDate", typeof(DateTime));
            var enrolledProgramId = new ObjectParameter("EnrolledProgramId", typeof(int));

            var mostRecentPep = (IEnumerable<IUSP_ProgramStatus_Recent_Result>) _db.USP_ProgramStatus_Recent(
                                                                                                             pin,
                                                                                                             Database,
                                                                                                             true,
                                                                                                             null,
                                                                                                             recentStatus,
                                                                                                             referralDate,
                                                                                                             enrollmentDate,
                                                                                                             disEnrollmemtDate,
                                                                                                             enrolledProgramId
                                                                                                            ).ToList();

            var officeId = mostRecentPep.FirstOrDefault(x => x.ProgramName.Trim() == "WW")?.OfficeId;

            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var officeNumber = _db.Offices.Where(i => (i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate)
                                  .FirstOrDefault(x => x.Id == officeId)?.OfficeNumber;


            if (officeNumber == 9999) // magic strings in here are Phase I HACK ;)
            {
                geoArea.AgencyCode   = "WI";
                geoArea.AgencyName   = "State";
                geoArea.OfficeNumber = 9999;
            }
            else
            {
                geoArea = _db.Offices.Where(i => (i.OfficeNumber == officeNumber && i.ContractArea.EnrolledProgram.ProgramCode.Trim() == "WW")
                                                 && (i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate)
                             .Select(i => new GeoArea
                                          {
                                              Name         = i.ContractArea.ContractAreaName,
                                              AgencyCode   = i.ContractArea.Organization.EntsecAgencyCode,
                                              AgencyName   = i.ContractArea.Organization.AgencyName,
                                              OfficeNumber = i.OfficeNumber,
                                              CountyNumber = i.CountyAndTribe.CountyNumber
                                          }).FirstOrDefault();
            }

            return (geoArea);
        }
    }
}
