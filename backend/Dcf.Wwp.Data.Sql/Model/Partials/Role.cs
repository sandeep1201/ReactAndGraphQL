using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Role : BaseCommonModel, IRole
    {
        ICollection<IRoleAuthorization> IRole.RoleAuthorizations
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }
    }
}
