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
    public partial class Repository : IJobDutyDetailRepository
    {
        public IJobDutiesDetail JobDutyById(int? id)
        {
            return (from x in _db.JobDutiesDetails where x.Id == id where x.IsDeleted == false select x).SingleOrDefault();
        }

        public IJobDutiesDetail NewJobDuty(string user)
        {
            IJobDutiesDetail jd = new JobDutiesDetail();
            jd.ModifiedDate = DateTime.Now;
            jd.ModifiedBy = user;
            jd.IsDeleted = false;
            _db.JobDutiesDetails.Add((JobDutiesDetail)jd);
            return jd;
        }
    }
}
