using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Api.Library.Utils
{
    public static class CountyAgencyOfficeHelper
    {
        public static string GetCountyName(int? countyNumber, IEnumerable<ICountyAndTribe> countyList)
        {
            if (countyNumber == null || countyNumber == 0) return null;
            var countyName = countyList?.Where(x => x.CountyNumber == countyNumber).Select(y => y.CountyName)
                .FirstOrDefault();

            return countyName.ToTitleCase();
        }

        public static string GetAgencyName(int? agencyId, IEnumerable<IOrganization> agencyList)
        {
            if (agencyId == null || agencyId == 0) return null;

            var agencyName = agencyList?.Where(x => x.Id == agencyId).Select(c => c.AgencyName)
                .FirstOrDefault();

            return agencyName;
        }

        public static IOrganization GetAgencyByOfficeNumber(short? officeNumber, IEnumerable<IOffice> officeList)
        {
            if (officeNumber == null || officeNumber == 0) return null;

            return officeList.FirstOrDefault(x => x.OfficeNumber == officeNumber)?.ContractArea?.Organization;
        }

        public static string GetAgencyCode(int? agencyId, IEnumerable<IOrganization> agencyList)
        {
            if (agencyId == null || agencyId == 0) return null;

            return agencyList?.Where(x => x.Id == agencyId).Select(c => c.EntsecAgencyCode)
                .FirstOrDefault();
        }

        public static string GetAgencyCodeByOfficeNum(short? officeNumber, IEnumerable<IOffice> officeList)
        {
            return GetAgencyByOfficeNumber(officeNumber, officeList)?.EntsecAgencyCode;
        }

        public static string GetAgencyNameByOfficeNum(short? officeNumber, IRepository repo)
        {
            if (officeNumber == null || officeNumber == 0) return null;

            var agency = repo.GetOrganizationByOfficeNumber(officeNumber);

            return agency?.AgencyName;
        }

        public static string GetCountyNameFromNumber(short? countyNumber, IEnumerable<ICountyAndTribe> countyList)
        {
            if (countyNumber == null || countyList == null) return null;

            var countyName = countyList.Where(x => x.CountyNumber == countyNumber)
                                       .Select(y => y.CountyName)
                                       .FirstOrDefault();

            return countyName?.Trim().ToTitleCase();
        }
    }
}