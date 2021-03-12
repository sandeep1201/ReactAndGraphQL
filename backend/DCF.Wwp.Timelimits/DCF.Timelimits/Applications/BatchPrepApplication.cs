using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;

namespace DCF.Timelimits
{
    public class BatchPrepApplication : BatchApplication
    {
        public BatchPrepApplication(ApplicationContext context) : base(context)
        {
        }

        public override void CreateProcessingQueue()
        {
            // Do nothing, we don't need a queue for batch prep
        }

        internal override Task StartProducerQueue()
        {
            // Do nothing, we don't need a queue for batch prep
            return Task.CompletedTask;
        }

        internal override void WriteJobOutput()
        {
            var file_path = Path.Combine(this.Context.OutputPath, $"batch_cleanup_{this.Context.JobQueuePartion}_{this.Context.Date:MMMM_yyyy)}_{Guid.NewGuid().ToString("N").Substring(0, 10)}.xlsx");
            File.WriteAllText(file_path,"Batch Prep Completed succesfully");
        }

        public override async Task StartAsync()
        {
            if (this.Context.IsSimulation)
            {
                await Task.Delay(10000);
            }
            else
            {
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                {
                    await timelimitService.RefreshParticipantsAsync(this.Context.Date, this._token);
                }
            }
        }
    }
}