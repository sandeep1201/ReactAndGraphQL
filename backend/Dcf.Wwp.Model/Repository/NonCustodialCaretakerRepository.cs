using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialCaretakerRepository
    {
        public INonCustodialCaretaker NewNonCustodialCaretaker(INonCustodialParentsSection section, string user)
        {
            var ncc = new NonCustodialCaretaker();
            ncc.NonCustodialParentsSection = section as NonCustodialParentsSection;
            ncc.ModifiedBy = user;
            ncc.ModifiedDate = DateTime.Now;
            _db.NonCustodialCaretakers.Add(ncc);

            return ncc;
        }
    }
}