using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public  partial class Repository:IFamilyMemberRepository
    {
        public IFamilyMember NewFamilyMember(IFamilyBarriersSection familyBarriersSection)
        {
            var fm = new FamilyMember();
            fm.FamilyBarriersSection = familyBarriersSection as FamilyBarriersSection;                      
            _db.FamilyMembers.Add(fm);

            return fm;
        }
    }
}
