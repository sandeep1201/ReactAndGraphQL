using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.Auth
{
    public static class AuthUserExtensions
    {
        public static List<string> EnrolledProgramCodes(this IAuthUser authUser, IRepository repo)  //TODO: refactor *this*
        {
            // Get the worker record.
            var org = repo.GetOrganizationByCode(authUser.AgencyCode);

            if (org == null || authUser.Authorizations == null)
            {
                return new List<string>();
            }

            // First let's get a list of the Programs the Organization is setup for.
            var epcList = org.ContractAreas.Select(x => x.EnrolledProgram.ProgramCode).ToList();

            if (epcList.Count == 0)
            {
                return new List<string>();
            }

            // We need the enrolled programs from the authUser. An example of the convention for these authorizations
            // is canAccessProgram-TJ.  Note the enrolled program code returned above is 3 char padded for DB2
            // compatibility, so we need to pad it so they match.
            var authProgCodes = authUser.Authorizations.Where(x => x.StartsWith("canAccessProgram_")).Select(x => x.Split('_')[1].PadRight(3));

            return epcList.Intersect(authProgCodes).ToList();
        }

        public static int? OrganizationId(this IAuthUser authUser, IRepository repo)
        {
            var worker = repo.WorkerByWamsId(authUser.Username);

            return worker?.OrganizationId;
        }

        /// <summary>
        ///     Checks the authenticated user to see if they should have access to the PIN given the
        ///     business rules.
        /// </summary>
        /// <param name="authUser"></param>
        /// <param name="recentPrograms"></param>
        /// <param name="supId"></param>
        /// <returns></returns>
        public static bool HasConfidentialAccess(this IAuthUser authUser, IEnumerable<EnrolledProgramContract> recentPrograms, string supId = null)
        {
            if (recentPrograms == null)
            {
                return (false);
            }

            if (!string.IsNullOrEmpty(supId) && authUser.MainFrameId == supId)
            {
                return (true);
            }

            var b = recentPrograms.Any(x => x.AssignedWorker?.Wiuid == authUser.WIUID);

            return (b);
        }
    }
}
