using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository
    {
      public int EmploymentProgramTypeByName(string name)
      {
          return (from x in _db.EmploymentProgramTypes where x.Name.ToLower() == name.ToLower() select x.Id).FirstOrDefault();
      }
    }
}
