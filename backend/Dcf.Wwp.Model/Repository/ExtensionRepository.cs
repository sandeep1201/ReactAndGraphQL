using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IExtensionRepository
    {
        public IEnumerable<ITimeLimitExtension> GetExtensionSequenceExtensionsByExtById(Int32 id)
        {
            var extensions = new List<TimeLimitExtension>();
            var ext        = this._db.TimeLimitExtensions.FirstOrDefault(x => x.Id == id);
            if (ext != null)
            {
                extensions =
                    this._db.TimeLimitExtensions.Where(
                                                       x =>
                                                           x.ParticipantId   == ext.ParticipantId && x.ExtensionSequence == ext.ExtensionSequence &&
                                                           x.TimeLimitTypeId == ext.TimeLimitTypeId).ToList();
            }

            return extensions;
        }

        public IEnumerable<ITimeLimitExtension> GetExtensionsByPin(String pin)
        {
            Decimal pinDec;
            var     extensions = new List<TimeLimitExtension>();
            if (Decimal.TryParse(pin, out pinDec))
            {
                extensions = this._db.TimeLimitExtensions.Where(x => x.Participant.PinNumber == pinDec).ToList();
            }

            return extensions;
        }

        public ITimeLimitExtension GetExtensionsById(Int32 id)
        {
            return this._db.TimeLimitExtensions.FirstOrDefault(x => x.Id == id);
        }

        public ITimeLimitExtension GetCurrentExtensionByType(Int32 timelimitTypeId, String pin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IApprovalReason> GetExtensionApprovalReasons()
        {
            return this._db.ApprovalReasons.ToList();
        }

        public IEnumerable<IDenialReason> GetExtensionDenialReasons()
        {
            return this._db.DenialReasons.ToList();
        }

        public ITimeLimitExtension NewTimeLimitExtension()
        {
            var timelimitExtension = this._db.TimeLimitExtensions.Create();
            timelimitExtension.IsDeleted   = false;
            timelimitExtension.CreatedDate = DateTime.Now;
            this._db.TimeLimitExtensions.Add(timelimitExtension);
            return timelimitExtension;
            //var dbset = this._db.Entry(timelimit);
            //dbset.State = EntityState.Added;
            //return dbset.Entity;
        }

        public Int32? GetCurrentExtensionSequenceNumber(Int32 participantId, Int32 timelimitTypeId)
        {
            return
                this._db.TimeLimitExtensions.Where(
                                                   x =>
                                                       x.ParticipantId     == participantId && x.TimeLimitTypeId == timelimitTypeId &&
                                                       x.ExtensionSequence > 0).Select(x => x.ExtensionSequence).Max(x => x);
        }

        // note: DateRange Filtering is done in memory cause I couldn't figure out how to normalize and comparte the dateranges in SQL/LINQ. 
        public ITimeLimitExtension GetExensionByDateRange(Int32 participantId, Int32 timelimitTypeId, DateTime startDate, DateTime endDate)
        {
            var modelDateRange = new DateTimeRange(startDate, endDate);
            var extensions     = this._db.TimeLimitExtensions.Where(x => !x.IsDeleted && x.ParticipantId == participantId && x.TimeLimitTypeId == timelimitTypeId).ToList();
            return extensions.FirstOrDefault(x =>
                                             {
                                                 var dateRange = new DateTimeRange(x.BeginMonth.GetValueOrDefault(), x.EndMonth.GetValueOrDefault());
                                                 return dateRange.Overlaps(modelDateRange);
                                             });
        }

        public string GetTimeLimitType(int? timelimitTypeId)
        {
            var type = _db.TimeLimitTypes.Where(i => i.Id == timelimitTypeId).Select(j => j.Name).FirstOrDefault()?.Trim();
            return type;
        }

        public string GetExtensionDecision(int? extensionDecisionId)
        {
            var type = _db.ExtensionDecisions.Where(i => i.Id == extensionDecisionId).Select(j => j.Name).FirstOrDefault()?.Trim();
            return type;
        }

        public IQueryable<ITimeLimit> GetTimeLimit(int participantId)
        {
            var tl = _db.TimeLimits.Where(i => i.ParticipantID == participantId);
            return tl;
        }
    }
}
