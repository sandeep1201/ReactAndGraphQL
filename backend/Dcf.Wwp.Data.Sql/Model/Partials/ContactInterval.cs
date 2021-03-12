using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContactInterval : BaseCommonModel, IContactInterval
    {
        ICollection<INonCustodialCaretaker> IContactInterval.NonCustodialCaretakers
        {
            get { return NonCustodialCaretakers.Cast<INonCustodialCaretaker>().ToList(); }
            set { NonCustodialCaretakers = value.Cast<NonCustodialCaretaker>().ToList(); }
        }

        ICollection<INonCustodialChild> IContactInterval.NonCustodialChilds
        {
            get { return NonCustodialChilds.Cast<INonCustodialChild>().ToList(); }
            set { NonCustodialChilds = value.Cast<NonCustodialChild>().ToList(); }
        }
    }
}
