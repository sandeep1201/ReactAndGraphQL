using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
	public partial class Repository : IPolarLookupRepository
	{
		public IEnumerable<IPolarLookup> PolarLookups()
		{
			return (from x in _db.PolarLookups select x).ToList();
		}
	}
}
