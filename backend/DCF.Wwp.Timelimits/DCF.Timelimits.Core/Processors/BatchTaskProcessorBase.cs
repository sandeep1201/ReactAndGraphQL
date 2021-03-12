using System;
using System.Threading;
using System.Threading.Tasks;
using DCF.Common.Logging;
using DCF.Timelimits.Core.Tasks;

namespace DCF.Timelimits.Core.Processors
{
    public abstract class BatchTaskProcessBase<T,TK> : IBatchTaskProcessor<T,TK> where T : IBatchTask<TK>
    {
        protected ILog _logger;

        protected BatchTaskProcessBase()
        {
            this._logger = LogProvider.GetLogger(this.GetType());
        }

        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 Priority { get; set; }


        public abstract Task<TK> Handle(T context, CancellationToken token);
    }
}
