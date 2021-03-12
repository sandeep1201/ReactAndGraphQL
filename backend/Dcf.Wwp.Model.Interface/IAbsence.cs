using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
   public interface IAbsence:ICommonModel,ICloneable
    {
         Int32? EmploymentInformationId { get; set; }
         DateTime? BeginDate { get; set; }
         DateTime? EndDate { get; set; }
         Int32? AbsenceReasonId { get; set; }
         String Details { get; set; }
         Int32? SortOrder { get; set; }
        Boolean IsDeleted { get; set; }
         IAbsenceReason AbsenceReason { get; set; }
         IEmploymentInformation EmploymentInformation { get; set; }
    }
}
