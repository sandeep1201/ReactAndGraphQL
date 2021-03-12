using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IEnumerable<IPopulationType> PopulationTypes()
        {
            var q = _db.PopulationTypes;
            var r = q.ToList();

            return (r);
        }

        public IEnumerable<IPopulationTypeDto> PopulationTypesFor(string programCode, string agencyId = null)
        {
            // correctthe kabob-casing (ie, 'employ-milwuakee' becomes 
            // 'employ milwaukee' which is how it it in SqlSver)
            //            if (!string.IsNullOrEmpty(agencyName) && agencyName.Contains("-"))
            //            {
            //                agencyName = agencyName.Replace('-', ' ');
            //            }

            int? orgId = null;
            if (agencyId != null)
                orgId = int.Parse(agencyId);

            var r = _db.EnrolledProgramOrganizationPopulationTypeBridges
                   .Where(epab => epab.EnrolledProgram.ProgramCode == programCode && epab.Organization.Id == orgId && !epab.IsDeleted)
                   .OrderBy(epab => epab.PopulationType.SortOrder)
                   .Select(epab => new PopulationTypeDto()
                   {
                       Id = epab.PopulationType.Id,   //PopulationTypeId
                       Name = epab.PopulationType.Name, // PopulationType
                       DisablesOthers = epab.DisabledPopulationTypes.Count(dpt => dpt.EnrolledProgramOrganizationPopulationTypeBridgeId == epab.Id && !dpt.IsDeleted) > 0,
                       DisabledIds = epab.DisabledPopulationTypes
                                                            .Where(dpt => dpt.EnrolledProgramOrganizationPopulationTypeBridgeId == epab.Id && !dpt.IsDeleted)
                                                            .Select(dpt => dpt.PopulationTypeId)
                   }).ToList();

            return (r);
        }
    }
}
