using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Timelimits.Core.Tasks;
using MediatR;

namespace DCF.Timelimits.Core.Processors
{
    public interface ITaskMediator : IMediator
    {
        Task<IEnumerable<TResponse>> SendToMany<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken());
    }

    public class BatchTaskRunner : ITaskMediator
    {

        internal IMediator baseMediator;
        private readonly ILog _logger;

        public SortedList<Int32, IBatchTask> Processors { get; } = new SortedList<Int32, IBatchTask>();
        private readonly SingleInstanceFactory _singleInstanceFactory;
        private readonly MultiInstanceFactory _multiInstanceFactory;
        private static readonly ConcurrentDictionary<Type, NotificationHandler> _notificationHandlers = new ConcurrentDictionary<Type, NotificationHandler>();
        private static readonly ConcurrentDictionary<Type, object> _requestHandlers = new ConcurrentDictionary<Type, object>();

        internal static ReadOnlyDictionary<Type, Object> CurrentRequestHandlers => BatchTaskRunner._requestHandlers.ToReadOnlyDictionary();
        internal static IReadOnlyDictionary<Type, NotificationHandler> CurrentNotificationHandlers => BatchTaskRunner._notificationHandlers.ToReadOnlyDictionary();

        public BatchTaskRunner(MultiInstanceFactory multiInstanceFactory, SingleInstanceFactory singleInstanceFactory)
        {
            this._singleInstanceFactory = singleInstanceFactory;
            this._multiInstanceFactory = multiInstanceFactory;
            this._logger = LogProvider.GetLogger(this.GetType());
            this.baseMediator = singleInstanceFactory(typeof(IMediator)) as IMediator;
        }

        public Task<IEnumerable<TResponse>> SendToMany<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            var requestType = request.GetType();
            var handler = (RequestHandler<TResponse>) BatchTaskRunner._requestHandlers.GetOrAdd(requestType, t => (RequestHandler<TResponse>) Activator.CreateInstance(typeof(RequestHandlerImpl<,>).MakeGenericType(requestType, typeof(TResponse))));
            return handler.Handle(request, cancellationToken, this._multiInstanceFactory, this.PublishCore, this._logger);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            return this.baseMediator.Send(request, cancellationToken);
        }

        public Task Send(IRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return this.baseMediator.Send(request, cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken))
            where TNotification : INotification
        {
            var notificationType = notification.GetType();
            var handler = BatchTaskRunner._notificationHandlers.GetOrAdd(notificationType,
                t => (NotificationHandler)Activator.CreateInstance(typeof(NotificationHandlerImpl<>).MakeGenericType(notificationType)));

            return handler.Handle(notification, cancellationToken, this._multiInstanceFactory, this.PublishCore, this._logger);
        }

        /// <summary>
        /// Override in a derived class to control how the tasks are awaited. By default the implementation is <see cref="Task.WhenAll(IEnumerable{Task})" />
        /// </summary>
        /// <param name="allHandlers">Enumerable of tasks representing invoking each notification/request handler</param>
        /// <returns>A task representing invoking all handlers</returns>
        protected virtual async Task PublishCore(IEnumerable<Task> allHandlers)
        {
            await Task.WhenAll(allHandlers).ConfigureAwait(false);
        }

        /// <summary>
        /// Override in a derived class to control how the tasks are awaited. By default the implementation is <see cref="Task.WhenAll(IEnumerable{Task})" />
        /// </summary>
        /// <param name="allHandlers">Enumerable of tasks representing invoking each notification/request handler</param>
        /// <returns>A task representing invoking all handlers</returns>
        protected virtual async Task<IEnumerable<T>> PublishCore<T>(IEnumerable<Task<T>> allHandlers)
        {
            await Task.WhenAll(allHandlers).ConfigureAwait(false);
            return allHandlers.Where(handler => handler.Status == TaskStatus.RanToCompletion).Select(handler => handler.Result).ToList();
        }
    }


    internal abstract class RequestHandler<TResponse>
    {
        public abstract Task<IEnumerable<TResponse>> Handle(IRequest<TResponse> request, CancellationToken cancellationToken, MultiInstanceFactory multiFactory, Func<IEnumerable<Task<TResponse>>, Task<IEnumerable<TResponse>>> publish, ILog log);

    }

    internal class RequestHandlerImpl<TRequest, TResponse> : RequestHandler<TResponse>
        where TRequest : IRequest<TResponse>, IRequest
    {
        public override async Task<IEnumerable<TResponse>> Handle(IRequest<TResponse> request, CancellationToken cancellationToken, MultiInstanceFactory multiInstanceFactory, Func<IEnumerable<Task<TResponse>>, Task<IEnumerable<TResponse>>> publish, ILog log)
        {
            var handlerPriorityStacks = this.GetHandlers((TRequest) request, cancellationToken, multiInstanceFactory);
            var handlerPriorites = handlerPriorityStacks.Select(x => x.Key).ToList();
            handlerPriorites.InsertionSort();
            List<TResponse> results = new List<TResponse>();

            foreach (var priorityStack in handlerPriorites)
            {
                log.Info($"Running async ({handlerPriorityStacks[priorityStack].Count()}) Batch Task(s) with Priority: {priorityStack}");
                try
                {
                    var tasks = handlerPriorityStacks[priorityStack].Select(x => x());
                    var priorityStackResults = await publish(tasks).ConfigureAwait(false);
                    results.AddRange(priorityStackResults);
                }
                catch (Exception e)
                {
                    this.ProcessHandlerError(e, priorityStack);
                }

                log.Info($"Completed async ({handlerPriorityStacks[priorityStack].Count()}) Batch Task(s) with Priority: {priorityStack}");
            }
            return results;
        }

        private void ProcessHandlerError(Exception exception, Int32 priorityStack)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException ?? exception).Throw();
        }

        private static IEnumerable<THandler> GetHandlers<THandler>(MultiInstanceFactory factory)
        {
            return factory(typeof(THandler)).Cast<THandler>();
        }

        private ILookup<Int32, Func<Task<TResponse>>> GetHandlers(TRequest context, CancellationToken cancellationToken, MultiInstanceFactory factory)
        {
            var requestHandlers = RequestHandlerImpl<TRequest, TResponse>.GetHandlers<IRequestHandler<TRequest>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task<TResponse>>(() =>
                {
                    x.Handle(context);
                    return Task.FromResult(default(TResponse));
                }));

            var asyncRequestHandlers = RequestHandlerImpl<TRequest, TResponse>.GetHandlers<IAsyncRequestHandler<TRequest>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task<TResponse>>(() =>
                {
                    x.Handle(context);
                    return Task.FromResult(default(TResponse));
                }));

            var cancellableAsyncRequestHandlers = RequestHandlerImpl<TRequest, TResponse>.GetHandlers<ICancellableAsyncRequestHandler<TRequest>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task<TResponse>>(() =>
                {
                    x.Handle(context, cancellationToken);
                    return Task.FromResult(default(TResponse));
                }));


            var requestResponseHandlers = RequestHandlerImpl<TRequest, TResponse>.GetHandlers<IRequestHandler<TRequest, TResponse>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task<TResponse>>(() => Task.FromResult(x.Handle(context))));

            var asyncRequestResponseHandlers = RequestHandlerImpl<TRequest, TResponse>.GetHandlers<IAsyncRequestHandler<TRequest, TResponse>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task<TResponse>>(() => x.Handle(context)));

            var cancellableAsyncRequestResponseHandlers = RequestHandlerImpl<TRequest, TResponse>.GetHandlers<ICancellableAsyncRequestHandler<TRequest, TResponse>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task<TResponse>>(() => x.Handle(context, cancellationToken)));

            var allHandlers = requestHandlers
                .ConcatLookups(asyncRequestHandlers)
                .ConcatLookups(cancellableAsyncRequestHandlers)
                .ConcatLookups(requestResponseHandlers)
                .ConcatLookups(asyncRequestResponseHandlers)
                .ConcatLookups(cancellableAsyncRequestResponseHandlers);

            return allHandlers;
        }

        private Int32 GetPriority(Object arg)
        {
            var batchTaskAttribute = arg.GetType().GetCustomAttribute<BatchTaskProcessAttribute>(true);
            return (Int32) (batchTaskAttribute?.Priority ?? 0);

        }

    }

    internal abstract class NotificationHandler
    {
        public abstract Task Handle(INotification notification, CancellationToken cancellationToken, MultiInstanceFactory multiInstanceFactory, Func<IEnumerable<Task>, Task> publish, ILog log);
    }

    internal class NotificationHandlerImpl<TNotification> : NotificationHandler
        where TNotification : INotification
    {
        public override async Task Handle(INotification notification, CancellationToken cancellationToken, MultiInstanceFactory multiInstanceFactory, Func<IEnumerable<Task>, Task> publish, ILog log)
        {
            var handlerPriorityStacks = this.GetHandlers((TNotification) notification, cancellationToken, multiInstanceFactory);
            var handlerPriorites = handlerPriorityStacks.Select(x => x.Key).ToList();
            handlerPriorites.InsertionSort();
            foreach (var priorityStack in handlerPriorites)
            {
                log.Info($"Running async ({handlerPriorityStacks[priorityStack].Count()}) Batch Task(s) with Priority: {priorityStack}");
                try
                {
                    var tasks = handlerPriorityStacks[priorityStack].Select(x => x());
                    await publish(tasks).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    this.ProcessHandlerError(e, priorityStack);
                }

                log.Info($"Completed async ({handlerPriorityStacks[priorityStack].Count()}) Batch Task(s) with Priority: {priorityStack}");
            }
        }

        private void ProcessHandlerError(Exception exception, Int32 priorityStack)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException ?? exception).Throw();
        }

        private static IEnumerable<THandler> GetHandlers<THandler>(MultiInstanceFactory factory)
        {
            return factory(typeof(THandler)).Cast<THandler>();
        }

        private ILookup<Int32, Func<Task>> GetHandlers(TNotification notification, CancellationToken cancellationToken, MultiInstanceFactory factory)
        {
            var notificationHandlers = NotificationHandlerImpl<TNotification>.GetHandlers<INotificationHandler<TNotification>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task>(() =>
                    {
                        x.Handle(notification);
                        return Task.CompletedTask;
                    }
                ));

            var asyncNotificationHandlers = NotificationHandlerImpl<TNotification>.GetHandlers<IAsyncNotificationHandler<TNotification>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task>(() => x.Handle(notification)));

            ILookup<Int32, Func<Task>> cancellableAsyncNotificationHandlers = NotificationHandlerImpl<TNotification>.GetHandlers<ICancellableAsyncNotificationHandler<TNotification>>(factory)
                .ToLookup(this.GetPriority, x => new Func<Task>(() => x.Handle(notification, cancellationToken)));

            var allHandlers = notificationHandlers
                .ConcatLookups(asyncNotificationHandlers)
                .ConcatLookups(cancellableAsyncNotificationHandlers);

            return allHandlers;
        }

        private Int32 GetPriority(Object arg)
        {
            var batchTaskAttribute = arg.GetType().GetCustomAttribute<BatchTaskProcessAttribute>(true);
            return (Int32) (batchTaskAttribute?.Priority ?? 0);

        }
    }
}