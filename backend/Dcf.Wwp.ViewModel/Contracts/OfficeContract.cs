using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class OfficeContract : BaseContract
    {
        public int? OfficeNumber { get; set; }

        public AgencyContract Agency { get; set; }

        public static OfficeContract Create(IOffice office, IList<ICountyAndTribe> counties)
        {
            var contract = new OfficeContract
            {
                Id           = office.Id,
                OfficeNumber = office.OfficeNumber
            };

            if (office.ContractArea?.Organization != null)
            {
                var agencyContract = new AgencyContract
                {
                    Id           = (int)office.ContractArea?.Organization?.Id,
                    //AgencyNumber = office.Agency.AgencyNumber,    //TODO: this doesn't exist in the new schema
                    AgencyCode   = office.ContractArea?.Organization?.EntsecAgencyCode,
                    AgencyName   = office.ContractArea?.Organization?.AgencyName,
                    AgencyCounty = counties.SingleOrDefault(x => x.CountyNumber == office.CountyAndTribe?.CountyNumber)?
                        .CountyName?.Trim().ToTitleCase()
                };

                contract.Agency = agencyContract;
            }

            return contract;
        }
    }

    public class AgencyContract : BaseContract
    {
        public short? AgencyNumber { get; set; }
        public string AgencyName   { get; set; }
        public string AgencyCode   { get; set; }
        public string AgencyCounty { get; set; }

        public static AgencyContract Create(IOrganization organization, IList<IOrganization> organizations)
        {
            var contract = new AgencyContract
                           {
                               Id         = organization.Id,
                               AgencyName = organization.AgencyName,
                               AgencyCode =  organization.EntsecAgencyCode
                           };

            return contract;
        }
    }
}

