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
    public partial class Repository:IAbsenceRepository
    {
        public IAbsence NewAbsence(IEmploymentInformation parentObject, string user)
        {
            IAbsence ab = new Absence();
            ab.EmploymentInformation = parentObject;
            ab.ModifiedBy = user;
            ab.ModifiedDate = DateTime.Now;
            ab.IsDeleted = false;
            _db.Absences.Add((Absence)ab);
            return ab;
        }
    }
}
