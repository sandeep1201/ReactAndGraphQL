using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ISecurityRepository
    {
        List<string>        AuthorizationsForRoles(IEnumerable<string> roles);
        IEnumerable<IRole>  AuthorizationRoles();
        IEnumerable<IRole>  AuthorizationRoles(string[] roleCodes);
        IEnumerable<string> GetWorkerUsernames();
    }
}
