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
using EnumsNET;
using ExtensionDecision = DCF.Timelimits.Rules.Domain.ExtensionDecision;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IT0460_IN_W2_EXTRepository
    {
        public IT0460_IN_W2_EXT NewT0460InW2Ext(bool isTracked)
        {
            if (!isTracked) return new T0460_IN_W2_EXT();

            var t0460 = this._db.T0460_IN_W2_EXT.Create();
            t0460.HISTORY_CD = 0;
            this._db.T0460_IN_W2_EXT.Add(t0460);
            return t0460;
            //var dbset = this._db.Entry(obj);
            //dbset.State = EntityState.Added;
            //return dbset.Entity;
        }

        public IT0460_IN_W2_EXT GetW2ExtensionByClockType(Decimal pinNum, ClockTypes timelimitType, Int32 sequenceNumber)
        {
            var clockTypeCd = timelimitType.HasAnyFlags(ClockTypes.State) ? "60MO" : timelimitType.ToString();
            return this._db.T0460_IN_W2_EXT.FirstOrDefault(x =>
                x.PIN_NUM == pinNum && x.HISTORY_CD == 0 && x.EXT_SEQ_NUM == sequenceNumber && x.CLOCK_TYPE_CD.Trim() == clockTypeCd
                
                && x.HISTORY_SEQ_NUM == this._db.T0460_IN_W2_EXT.Where(y => y.PIN_NUM == pinNum && y.HISTORY_CD == 0 && y.EXT_SEQ_NUM == sequenceNumber && y.CLOCK_TYPE_CD.Trim() == clockTypeCd).Max(y => y.HISTORY_SEQ_NUM)
            );
        }
        public IEnumerable<IT0460_IN_W2_EXT> GetW2Extensions(Decimal pinNum)
        {
            return this._db.T0460_IN_W2_EXT.Where(x =>
                x.PIN_NUM == pinNum && x.HISTORY_CD == 0
            ).ToList();
        }
    }
}
