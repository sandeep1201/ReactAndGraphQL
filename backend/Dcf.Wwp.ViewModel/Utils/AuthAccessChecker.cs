using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Utils
{
    public class AuthAccessChecker : IAuthAccessChecker
    {
        #region Properties

        private readonly IAuthUser _authUser;

        #endregion

        #region Methods

        #endregion

        public AuthAccessChecker (IAuthUser authUser)
        {
            _authUser = authUser;
        }

        public bool HasAccess(string supervisorId, List<MostRecentProgram> programs)
        {
            if (programs == null)
            {
                return (false);
            }

            if (!string.IsNullOrEmpty(supervisorId) && _authUser.WIUID == supervisorId)
            {
                return (true);
            }

            var hasAccess = programs.Any(i => i.WIUID == _authUser.WIUID);

            return (hasAccess);
        }
    }
}
