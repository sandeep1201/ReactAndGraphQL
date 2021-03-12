using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IRole : ICommonDelModel
    {
        string Name { get; set; }
        String Code { get; set; }
        Nullable<int> InheritedRoleId { get; set; }
        ICollection<IRoleAuthorization> RoleAuthorizations { get; set; }
    }
}