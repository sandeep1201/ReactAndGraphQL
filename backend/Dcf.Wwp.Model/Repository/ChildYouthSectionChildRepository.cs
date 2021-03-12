using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IChildYouthSectionChildRepository
    {
        public IChildYouthSectionChild NewChildYouthSectionChild(IChildYouthSection childYouthSection)
        {
            var cysc = new ChildYouthSectionChild();
            cysc.ChildYouthSection = childYouthSection as ChildYouthSection;
            _db.ChildYouthSectionChilds.Add(cysc);

            return cysc;
        }
    }
}