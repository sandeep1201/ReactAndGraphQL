using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IT0754_LTR_RQST
    {
        /// <summary>
        /// Case Number
        /// </summary>
        String CS_RFA_PRV_PIN_NUM {get;}
        String CS_RFA_PRV_PIN_IND {get;}
        String DEPT_ID {get;}
        String PROGRAM_CD {get;}
        String SUBPROGRAM_CD {get;}
        String AG_SEQ_NUM {get;}
        String RQST_TMS {get;} 

        /// <summary>
        /// WPGeoArea County Number
        /// </summary>
        String CNTY_NUM {get;}
        String CRE_IND {get;set;}
        String DOC_CD {get;set;}

        /// <summary>
        /// Effective Month (yyyyMM) format
        /// </summary>
        String LTR_MO {get;}
        String OFC_NUM {get;}
        String PROC_DT {get;}
        String PRVD_LOC_NUM {get;set;}
        
        /// <summary>
        /// Pin Number
        /// </summary>
        String SEC_RCPT_ID {get;}
        String SPRS_USER_ID {get;}
        String USER_ID {get;}
        String LTR_TXT {get;set;}

    }
}