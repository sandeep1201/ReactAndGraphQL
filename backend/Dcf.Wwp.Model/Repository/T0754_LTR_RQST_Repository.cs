using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IT0754_LTR_RQST_Repository
    {
        public void SpDB2_T0754_Insert(IT0754_LTR_RQST letterRequest)
        {
            this._db.DB2_T0754_Insert(letterRequest.CS_RFA_PRV_PIN_NUM, letterRequest.CS_RFA_PRV_PIN_IND, letterRequest.DEPT_ID, letterRequest.PROGRAM_CD
                , letterRequest.SUBPROGRAM_CD, letterRequest.AG_SEQ_NUM, letterRequest.RQST_TMS, letterRequest.CNTY_NUM, letterRequest.CRE_IND,
                letterRequest.DOC_CD, letterRequest.LTR_MO, letterRequest.OFC_NUM, letterRequest.PROC_DT, letterRequest.PRVD_LOC_NUM, letterRequest.SEC_RCPT_ID,
                letterRequest.SPRS_USER_ID, letterRequest.USER_ID, letterRequest.LTR_TXT);
        }
    }  
}
