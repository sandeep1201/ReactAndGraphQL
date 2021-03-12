using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JobType: BaseEntity
    {
        public string Name                        { get; set; }
        public bool?  IsRequired                  { get; set; }
        public int?   SortOrder                   { get; set; }
        public bool   IsDeleted                   { get; set; }
        public bool?  IsUsedForEmploymentOfRecord { get; set; }

        public virtual ICollection<EmploymentInformation> EmploymentInformations { get; set; } = new List<EmploymentInformation>();
    }
}
