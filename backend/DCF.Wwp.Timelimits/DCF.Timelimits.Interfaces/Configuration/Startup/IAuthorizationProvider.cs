using DCF.Core.Authorization;

namespace DCF.Core.Configuration.Startup
{
    public interface IAuthorizationProvider
    {
        void SetPermissions(IPermissionDefinitionContext context);
    }
}