using System;
using DCF.Timelimits.Core.Tasks;
using MediatR;

namespace DCF.Timelimits.Core.Processors
{
    public interface IBatchTaskProcessor<in T, TK> : ICancellableAsyncRequestHandler<T,TK>, IBatchTaskProcessor where T : IBatchTask<TK>, IRequest<TK>
    {
    }

    public interface IBatchTaskProcessor
    {
        String Name { get; set; }
        String Description { get; set; }
        Boolean IsActive { get; set; }
        Int32 Priority { get; set; }
    }
}