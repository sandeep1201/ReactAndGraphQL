using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;

namespace DCF.Timelimits.Processors
{
    public class BatchCleanupProcessor : BatchTaskProcessBase<BatchCleanupContext, BatchCleanupResult>
    {
        private readonly IDb2TimelimitService _db2TimelimitService;
        private readonly ApplicationContext _appContext;

        public BatchCleanupProcessor(IDb2TimelimitService db2TimelimitService, ApplicationContext appContext)
        {
            this._db2TimelimitService = db2TimelimitService;
            this._appContext = appContext;
        }

        public override async Task<BatchCleanupResult> Handle(BatchCleanupContext context, CancellationToken token)
        {

            var result = new BatchCleanupResult();

            this._db2TimelimitService.IsSimulated = this._appContext.IsSimulation;
            Db2TimelimitService.TransCode = "WWPBATCH";
            context.Timeline.TimelineDate = this._appContext.Date.AddMonths(1).StartOf(DateTimeUnit.Month);
            //TODO: make Async
            var newTicks = this._db2TimelimitService.UpdateTicks0459(context.TicksToUpdate, context.Timeline, this._appContext.Date, false, "Extension Cleanup by WWP BATCH ");

            await this._db2TimelimitService.SaveAsync(token).ConfigureAwait(false);

            result.NewTicks = newTicks;
            result.OldTicks = context.TicksToUpdate;
            return result;
        }
    }
}