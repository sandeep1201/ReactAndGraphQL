using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RoleAuthorization : BaseEntity, IRoleAuthorization
    {
        IAuthorization IRoleAuthorization.Authorization
        {
            get { return Authorization; }
            set { Authorization = (Authorization) value; }
        }
    }
}
