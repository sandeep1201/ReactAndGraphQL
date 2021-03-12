using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
	public partial class Repository : IContactTitleTypeRepository
	{
		public IEnumerable<IContactTitleType> ContactTitleTypes()
		{
			var q = from x in _db.ContactTitleTypes orderby x.SortOrder select x;
			return q;
		}

	    public IContactTitleType ContactTitleById(int? id)
	    {
	        return (from x in _db.ContactTitleTypes where x.Id == id select x).FirstOrDefault();
	    }

    }
}
