using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExamVersionType
    {
         Int32 Id { get; set; }
         Int32? VersionNo { get; set; }
         Int32? SortOrder { get; set; }
         String ModifiedBy { get; set; }
         DateTime? ModifiedDate { get; set; }
         Byte[] RowVersion { get; set; }
         //ICollection<IExamResult> ExamResults { get; set; }
    }
}
