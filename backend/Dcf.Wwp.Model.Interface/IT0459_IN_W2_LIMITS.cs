using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IT0459_IN_W2_LIMITS : ICloneable
    {
        decimal PIN_NUM { get; set; }
        decimal BENEFIT_MM { get; set; }
        short HISTORY_SEQ_NUM { get; set; }
        string CLOCK_TYPE_CD { get; set; }
        string CRE_TRAN_CD { get; set; }
        string FED_CLOCK_IND { get; set; }
        short FED_CMP_MTH_NUM { get; set; }
        short FED_MAX_MTH_NUM { get; set; }
        short HISTORY_CD { get; set; }
        short OT_CMP_MTH_NUM { get; set; }
        string OVERRIDE_REASON_CD { get; set; }
        short TOT_CMP_MTH_NUM { get; set; }
        short TOT_MAX_MTH_NUM { get; set; }
        System.DateTime UPDATED_DT { get; set; }
        string USER_ID { get; set; }
        short WW_CMP_MTH_NUM { get; set; }
        short WW_MAX_MTH_NUM { get; set; }
        string COMMENT_TXT { get; set; }
        int Id { get; set; }

        Boolean AreSemanticallyEqual(IT0459_IN_W2_LIMITS other);

        IT0459_IN_W2_LIMITS Copy(IT0459_IN_W2_LIMITS clockTick);
    }
}
