using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ITimelimitRepository
    {
        public IEnumerable<ITimeLimit> TimeLimitsByPin(String pin)
        {
            Decimal pinDec;
            var     timelimitMonths = new List<TimeLimit>();
            if (Decimal.TryParse(pin, out pinDec))
            {
                timelimitMonths = this._db.TimeLimits.Where(x => x.Participant.PinNumber == pinDec && !x.IsDeleted).ToList();
            }

            return timelimitMonths;
        }

        public ITimeLimit TimeLimitById(Int32 id)
        {
            return this._db.TimeLimits.FirstOrDefault(x => x.Id == id  );
        }

        public IEnumerable<ITimeLimit> TimeLimitsByIds(IEnumerable<Int32> ids)
        {
            return this._db.TimeLimits.Where(x => ids.Contains(x.Id));
        }

        public ITimeLimit TimeLimitByDate(String pin, DateTime date, Boolean includeDeleted)
        {
            ITimeLimit timelimit = null;
            Decimal    pinDec;
            if (Decimal.TryParse(pin, out pinDec))
            {
                var query =  this._db.TimeLimits.Where(
                                                       x => x.Participant.PinNumber == pinDec && x.EffectiveMonth.HasValue && DbFunctions.DiffMonths(x.EffectiveMonth.Value, date) == 0);
                if (!includeDeleted)
                {
                    query = query.Where(x => !x.IsDeleted);
                }

                timelimit = query.FirstOrDefault();
            }

            return timelimit;
        }

        public IEnumerable<ITimeLimit> TimeLimitsByDates(String pin, List<DateTime> dates)
        {
            IEnumerable<TimeLimit> timelimit = null;
            Decimal                pinDec;
            if (Decimal.TryParse(pin, out pinDec))
            {
                var filteredDates = dates.Select(x => new DateTime?(new DateTime(x.Year, x.Month, 1))).ToList();
                timelimit = this._db.TimeLimits.Where(
                                                      x => !x.IsDeleted && x.Participant.PinNumber == pinDec && x.EffectiveMonth.HasValue &&
                                                           filteredDates.Contains(DbFunctions.CreateDateTime(SqlFunctions.DatePart("year", x.EffectiveMonth), SqlFunctions.DatePart("month", x.EffectiveMonth), null, null, null, null) )).ToList();
            }

            return timelimit;
        }

        public IEnumerable<ITimeLimit> TimeLimitsHistory(Int32 id)
        {
            //return this._db.SPReadCDCandHistoryData<TimeLimit>(nameof(TimeLimit), id.ToString());
            return this._db.SPReadCDCHistory<TimeLimit>(nameof(TimeLimit), id.ToString());
        }


        public ITimeLimit NewTimeLimit()
        {
            var timelimit = new TimeLimit
                            {
                                IsDeleted   = false,
                                CreatedDate = DateTime.Now
                            };
            //var timelimit = this._db.TimeLimits.Create();
            //timelimit.IsDeleted   = false;
            //timelimit.CreatedDate = DateTime.Now;
            this._db.TimeLimits.Add(timelimit);
            return timelimit;
            //var dbset = this._db.Entry(timelimit);
            //dbset.State = EntityState.Added;
            //return dbset.Entity;
        }

        public IEnumerable<ITimeLimitState> TimeLimitStates(Boolean excludeWisconsin = true)
        {
            IQueryable<TimeLimitState> query = this._db.TimeLimitStates;
            if (excludeWisconsin)
            {
                query = query.Where(x => x.Code != "WI");
            }

            return query.ToList();
        }

        public IEnumerable<IChangeReason> ChangeReasons()
        {
            return this._db.ChangeReasons.ToList();
        }

        public ITimeLimitSummary NewTimeLimitSummary()
        {
            var entity = this._db.TimeLimitSummaries.Create();
            this._db.Entry(entity).State = EntityState.Added;
            return entity;
        }
    }
}
