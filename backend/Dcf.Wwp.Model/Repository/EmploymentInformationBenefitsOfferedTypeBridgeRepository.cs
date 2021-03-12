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
		public IEmploymentInformationBenefitsOfferedTypeBridge NewJobBenefitsOfferedActionBridge(IEmploymentInformation parentObject, string user)
		{
			IEmploymentInformationBenefitsOfferedTypeBridge jboab = new EmploymentInformationBenefitsOfferedTypeBridge();
			jboab.EmploymentInformation = parentObject;
            jboab.ModifiedBy = user;
		    jboab.ModifiedDate = DateTime.Now;
			jboab.IsDeleted = false;
			_db.EmploymentInformationBenefitsOfferedTypeBridges.Add((EmploymentInformationBenefitsOfferedTypeBridge)jboab);
			return jboab;
		}
	}
}
