using System;
using Dcf.Wwp.Model;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class T0754_LTR_RQST : IT0754_LTR_RQST
    {
        #region Properties

        public string CS_RFA_PRV_PIN_NUM { get; }
        public string CS_RFA_PRV_PIN_IND { get; } = "C";
        public string DEPT_ID            { get; } = "DFS";
        public string PROGRAM_CD         { get; } = "WW";
        public string SUBPROGRAM_CD      { get; } = " ";
        public string AG_SEQ_NUM         { get; } = "0";
        public string RQST_TMS           { get; } = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.ffffff");
        public string CNTY_NUM           { get; }
        public string CRE_IND            { get; set; } = "S";
        public string DOC_CD             { get; set; } = "WEXT";
        public string LTR_MO             { get; }
        public string OFC_NUM            { get; }      = "0";
        public string PROC_DT            { get; }      = DateTime.Now.ToString("yyyy-MM-dd");
        public string PRVD_LOC_NUM       { get; set; } = "0";
        public string SEC_RCPT_ID        { get; }
        public string SPRS_USER_ID       { get; } = "      ";
        public string USER_ID            { get; } = "      ";
        public string LTR_TXT            { get; set; }

        #endregion

        #region Methods

        public T0754_LTR_RQST(DateTime effectiveMonth, GeoArea wpGeoArea, IParticipantInfo participant, ISP_GetCARESCaseNumber_Result caseNumberResult)
        {
            CS_RFA_PRV_PIN_NUM = caseNumberResult.CaseNumber.ToString().PadLeft(10, '0');
            LTR_MO             = effectiveMonth.ToStringMonthYearComposite();
            CNTY_NUM           = (wpGeoArea?.CountyNumber.GetValueOrDefault().ToString() ?? "0");
            SEC_RCPT_ID        = participant.PinNumber.ToString().PadLeft(10, '0');
        }

        #endregion
    }
}
