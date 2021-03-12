using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IT0459_IN_W2_LIMITSRepository
    {
        public IT0459_IN_W2_LIMITS NewT0459_IN_W2_LIMITS(bool isTracked)
        {
            if (!isTracked) return new T0459_IN_W2_LIMITS();

            var t0459 = this._db.T0459_IN_W2_LIMITS.Create();
            t0459.HISTORY_CD = 0;
            this._db.T0459_IN_W2_LIMITS.Add(t0459);
            return t0459;
            //var dbset = this._db.Entry(t0459);
            //dbset.State = EntityState.Added;
            //return dbset.Entity;
        }


        public List<IT0459_IN_W2_LIMITS> GetLatestW2LimitsMonthsForEachClockType(Decimal pinNum)
        {
            return new List<IT0459_IN_W2_LIMITS>(this._db.T0459_IN_W2_LIMITS.Where(x => x.PIN_NUM == pinNum && x.HISTORY_CD == 0 && x.OVERRIDE_REASON_CD.StartsWith("S")==false ).GroupBy(x => x.CLOCK_TYPE_CD.Trim() )
                .Select(y => y.FirstOrDefault(z => z.BENEFIT_MM == y.Max(d => d.BENEFIT_MM))));
        }

        public IT0459_IN_W2_LIMITS GetLatestW2LimitsByClockType(Decimal pinNum, ClockTypes clockType)
        {
            var clockTypeCode = clockType.ToString();
            //var subQuery = this._db.T0459_IN_W2_LIMITS.Where(y => y.PIN_NUM == pinNum && y.HISTORY_CD == 0 && y.CLOCK_TYPE_CD == clockTypeCode);
            //return this._db.T0459_IN_W2_LIMITS.FirstOrDefault(x => x.PIN_NUM == pinNum && x.HISTORY_CD == 0 && x.CLOCK_TYPE_CD == clockTypeCode && x.BENEFIT_MM == subQuery.Max(y => y.BENEFIT_MM));
            return this.GetLatestW2LimitsMonthsForEachClockType(pinNum).FirstOrDefault(x => x.CLOCK_TYPE_CD.Trim() == clockTypeCode);

        }

        public IT0459_IN_W2_LIMITS GetW2LimitByMonth(Decimal effectiveMonth, Decimal pinNum)
        {
                return this._db.T0459_IN_W2_LIMITS.FirstOrDefault(x => x.PIN_NUM == pinNum && x.HISTORY_CD == 0 && x.BENEFIT_MM == effectiveMonth );
        }

        public List<IT0459_IN_W2_LIMITS> GetW2LimitsByPin(Decimal pinNum)
        {
                return new List<IT0459_IN_W2_LIMITS>(this._db.T0459_IN_W2_LIMITS.Where(x => x.PIN_NUM == pinNum && x.HISTORY_CD == 0));
        }

        public List<IT0459_IN_W2_LIMITS> GetSubsequentW2Limits(Decimal pinNum, DateTime timelineMonthDate)
        {
            var dDate = Decimal.Parse(timelineMonthDate.ToStringMonthYearComposite());
            return new List<IT0459_IN_W2_LIMITS>(this._db.T0459_IN_W2_LIMITS.Where(x => x.PIN_NUM == pinNum && x.HISTORY_CD == 0 && x.BENEFIT_MM > dDate && !x.OVERRIDE_REASON_CD.StartsWith("S") == false).ToList());
        }

        public void DB2_T0459_Update(IT0459_IN_W2_LIMITS db2Record)
        {

            this._db.DB2_T0459_Update(
                db2Record.PIN_NUM,
                db2Record.BENEFIT_MM,
                db2Record.HISTORY_SEQ_NUM,
                db2Record.CLOCK_TYPE_CD,
                db2Record.CRE_TRAN_CD,
                db2Record.FED_CLOCK_IND,
                db2Record.FED_CMP_MTH_NUM,
                db2Record.FED_MAX_MTH_NUM,
                db2Record.HISTORY_CD,
                db2Record.OT_CMP_MTH_NUM,
                db2Record.OVERRIDE_REASON_CD,
                db2Record.TOT_CMP_MTH_NUM,
                db2Record.TOT_MAX_MTH_NUM,
                db2Record.UPDATED_DT,
                db2Record.USER_ID,
                db2Record.WW_CMP_MTH_NUM,
                db2Record.WW_MAX_MTH_NUM,
                db2Record.COMMENT_TXT);
        }

    }
}
