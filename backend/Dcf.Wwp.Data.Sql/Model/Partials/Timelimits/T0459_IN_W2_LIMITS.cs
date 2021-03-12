using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class T0459_IN_W2_LIMITS : IT0459_IN_W2_LIMITS
    {
        #region ICloneable

        public object Clone()
        {
            return this.Copy(null);
        }

        #endregion ICloneable

        public IT0459_IN_W2_LIMITS Copy(IT0459_IN_W2_LIMITS destination)
        {
            if (destination == null)
            {
                destination = new T0459_IN_W2_LIMITS();
            }

            destination.PIN_NUM            = this.PIN_NUM;
            destination.BENEFIT_MM         = this.BENEFIT_MM;
            destination.HISTORY_SEQ_NUM    = this.HISTORY_SEQ_NUM;
            destination.CLOCK_TYPE_CD      = this.CLOCK_TYPE_CD;
            destination.CRE_TRAN_CD        = this.CRE_TRAN_CD;
            destination.FED_CLOCK_IND      = this.FED_CLOCK_IND;
            destination.FED_CMP_MTH_NUM    = this.FED_CMP_MTH_NUM;
            destination.FED_MAX_MTH_NUM    = this.FED_MAX_MTH_NUM;
            destination.HISTORY_CD         = this.HISTORY_CD;
            destination.OT_CMP_MTH_NUM     = this.OT_CMP_MTH_NUM;
            destination.OVERRIDE_REASON_CD = this.OVERRIDE_REASON_CD;
            destination.TOT_CMP_MTH_NUM    = this.TOT_CMP_MTH_NUM;
            destination.TOT_MAX_MTH_NUM    = this.TOT_MAX_MTH_NUM;
            destination.UPDATED_DT         = this.UPDATED_DT;
            destination.USER_ID            = this.USER_ID;
            destination.WW_CMP_MTH_NUM     = this.WW_CMP_MTH_NUM;
            destination.WW_MAX_MTH_NUM     = this.WW_MAX_MTH_NUM;
            destination.COMMENT_TXT        = this.COMMENT_TXT;
            destination.Id                 = this.Id;
            return destination;
        }

        public Boolean AreSemanticallyEqual(IT0459_IN_W2_LIMITS other)
        {
            if (other == null || this.GetType() != other.GetType())
            {
                return false;
            }

            return this.PIN_NUM    == other.PIN_NUM    &&
                   this.BENEFIT_MM == other.BENEFIT_MM &&
                   //this.CRE_TRAN_CD == other.CRE_TRAN_CD &&
                   this.FED_CLOCK_IND      == other.FED_CLOCK_IND      &&
                   this.PIN_NUM            == other.PIN_NUM            &&
                   this.BENEFIT_MM         == other.BENEFIT_MM         &&
                   this.HISTORY_SEQ_NUM    == other.HISTORY_SEQ_NUM    &&
                   this.FED_CLOCK_IND      == other.FED_CLOCK_IND      &&
                   this.FED_CMP_MTH_NUM    == other.FED_CMP_MTH_NUM    &&
                   this.FED_MAX_MTH_NUM    == other.FED_MAX_MTH_NUM    &&
                   this.HISTORY_CD         == other.HISTORY_CD         &&
                   this.OT_CMP_MTH_NUM     == other.OT_CMP_MTH_NUM     &&
                   this.OVERRIDE_REASON_CD == other.OVERRIDE_REASON_CD &&
                   this.TOT_CMP_MTH_NUM    == other.TOT_CMP_MTH_NUM    &&
                   this.TOT_MAX_MTH_NUM    == other.TOT_MAX_MTH_NUM    &&
                   //this.UPDATED_DT == other.UPDATED_DT &&
                   //this.USER_ID == other.USER_ID &&
                   this.WW_CMP_MTH_NUM == other.WW_CMP_MTH_NUM &&
                   this.WW_MAX_MTH_NUM == other.WW_MAX_MTH_NUM;
            //this.COMMENT_TXT == other.COMMENT_TXT;
        }

        //public override Int32 GetHashCode()
        //{
        //    return this.PIN_NUM.GetHashCode() ^
        //           this.BENEFIT_MM.GetHashCode() ^
        //           this.CRE_TRAN_CD.GetHashCode() ^
        //           this.FED_CLOCK_IND.GetHashCode() ^
        //           this.PIN_NUM.GetHashCode() ^
        //           this.BENEFIT_MM.GetHashCode() ^
        //           this.HISTORY_SEQ_NUM.GetHashCode() ^
        //           this.CRE_TRAN_CD.GetHashCode() ^
        //           this.FED_CLOCK_IND.GetHashCode() ^
        //           this.FED_CMP_MTH_NUM.GetHashCode() ^
        //           this.FED_MAX_MTH_NUM.GetHashCode() ^
        //           this.HISTORY_CD.GetHashCode() ^
        //           this.OT_CMP_MTH_NUM.GetHashCode() ^
        //           this.OVERRIDE_REASON_CD.GetHashCode() ^
        //           this.TOT_CMP_MTH_NUM.GetHashCode() ^
        //           this.TOT_MAX_MTH_NUM.GetHashCode() ^
        //           this.UPDATED_DT.GetHashCode() ^
        //           this.USER_ID.GetHashCode() ^
        //           this.WW_CMP_MTH_NUM.GetHashCode() ^
        //           this.WW_MAX_MTH_NUM.GetHashCode() ^
        //           this.COMMENT_TXT.GetHashCode();
        //}

        public Boolean IsCompositeSameAs(IT0459_IN_W2_LIMITS input)
        {
            throw new NotImplementedException();
        }
    }
}
