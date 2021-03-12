using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IT0460_IN_W2_EXT
    {
        Decimal PIN_NUM { get; set; }
        String CLOCK_TYPE_CD { get; set; }
        Int16 EXT_SEQ_NUM { get; set; }
        Int16 HISTORY_SEQ_NUM { get; set; }
        String AGY_DCSN_CD { get; set; }
        System.DateTime AGY_DCSN_DT { get; set; }
        Decimal BENEFIT_MM { get; set; }
        String DELETE_REASON_CD { get; set; }
        Decimal EXT_BEG_MM { get; set; }
        Decimal EXT_END_MM { get; set; }
        System.DateTime EXT_REQ_PRC_DT { get; set; }
        Int16 HISTORY_CD { get; set; }
        String STA_DCSN_CD { get; set; }
        System.DateTime UPDATED_DT { get; set; }
        String USER_ID { get; set; }
        Int32 Id { get; set; }
    }
}
