using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
	public partial class Repository
	{
		public IHousingHistory NewHousingHistory(IHousingSection parentAssessment, string user)
		{
			IHousingHistory hh = new HousingHistory();
			hh.HousingSection = parentAssessment;
			hh.ModifiedBy = user;
			hh.ModifiedDate = DateTime.Now;
			hh.IsDeleted = false;

			_db.HousingHistories.Add((HousingHistory)hh);

			return hh;
		}
	}
}
