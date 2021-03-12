using System;
using System.Threading.Tasks;

namespace DCF.Core.Runtime.Pipeline
{
    public interface INode<in T> : IDisposable where T : class
    {
        String Id { get; set; }
        String PipeId { get; set; }
        IExecutionOptions LocalOptions { get; }
        PipelineNodeStatus Status { get; set; }
        void Reset();
        Task<Boolean> ShouldExecuteAsync(IExecutionContext<T> sourceContext);
        Task<IPipelineNodeResult> ExecuteAsync(IExecutionContext<T> sourceContext);
        IExecutionOptions GetMergedExecutionOptions(IExecutionOptions globalOptions);

        /// <summary>
        /// Called before the node is executed. Override to add functionality.
        /// </summary>
        /// <param name="context">Effective context for execution.</param>
        Task OnBeforeExecuteAsync(IExecutionContext<T> context);

        /// <summary>
        /// Called after the node is executed. Override to add functionality.
        /// </summary>
        /// <param name="context">Effective context for execution.</param>
        Task OnAfterExecuteAsync(IExecutionContext<T> context);
    }
}