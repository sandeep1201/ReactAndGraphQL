using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAssistanceGroup
    {
        List<IAssistanceGroupMember> Parents { get; set; }
        List<IAssistanceGroupMember> Children { get; set; }
    }
    public interface IAssistanceGroupMember
    {
        Decimal? OTHER_PARTICIPANT { get; set; }
        Decimal? PARTICIPANT { get; set; }
        String FIRST_NAME { get; set; }
        String LAST_NAME { get; set; }
        String MIDDLE_INITIAL_NAME { get; set; }
        DateTime? BIRTH_DATE { get; set; }
        DateTime? DEATH_DATE { get; set; }
        String GENDER { get; set; }
        String RELATIONSHIP { get; set; }
        String AGE { get; set; }
        String ELIGIBILITY_PART_STATUS_CODE { get; set; }
        String ISINPLACEMENTPLACED { get; set; }

        Boolean IsChild();
    }
}
