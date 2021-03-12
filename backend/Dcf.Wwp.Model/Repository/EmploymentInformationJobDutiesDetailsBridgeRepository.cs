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
       public IEmploymentInformationJobDutiesDetailsBridge NewEmploymentInformationJobDutiesDetailsBridge(IEmploymentInformation parentObject, string user)
       {
            IEmploymentInformationJobDutiesDetailsBridge ejdb = new EmploymentInformationJobDutiesDetailsBridge();
            ejdb.EmploymentInformation = parentObject;
            ejdb.ModifiedDate = DateTime.Now;
            ejdb.ModifiedBy = user;
            ejdb.IsDeleted = false;
            _db.EmploymentInformationJobDutiesDetailsBridges.Add((EmploymentInformationJobDutiesDetailsBridge)ejdb);
            return ejdb;
        }
   }
}
