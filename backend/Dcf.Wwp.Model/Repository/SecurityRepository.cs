using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ISecurityRepository
    {
        public List<string> AuthorizationsForRoles(IEnumerable<string> roleCodes)
        {
            var authList = new List<string>();

            if (roleCodes != null)
            {
                // Get the active roles.
                // NOTE: using explicit loading of navigation properties to avoid lazy loading in the loop
                var roles =
                    _db.Roles.Where(x => roleCodes.Contains(x.Code) && !x.IsDeleted)
                       .Select(x => x)
                       .Include(x => x.RoleAuthorizations)
                       .Include(x => x.RoleAuthorizations.Select(y => y.Authorization));

                // TODO: Handle inheriting roles.

                foreach (var role in roles)
                {
                    authList.AddRange(from x in role.RoleAuthorizations where !x.IsDeleted select x.Authorization.Name);
                }
            }

            return authList.Distinct().ToList();
        }

        public IEnumerable<IRole> AuthorizationRoles()
        {
            return _db.Roles.Where(x => !x.IsDeleted).ToList();
        }

        public IEnumerable<IRole> AuthorizationRoles(string[] roleCodes)
        {
            var t = _db.Roles.Where(x => roleCodes.Contains(x.Code)).ToList();
            return t;
        }

        public IEnumerable<string> GetWorkerUsernames()
        {
            return _db.Workers.Select(x => x.WAMSId).ToList();
        }
    }
}
