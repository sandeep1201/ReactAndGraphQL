using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Core.Runtime.Session;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// Base for all Unit Of Work classes.
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public String Id { get; private set; }

        public IUnitOfWork Outer { get; set; }

        /// <inheritdoc/>
        public event EventHandler Completed;

        /// <inheritdoc/>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <inheritdoc/>
        public event EventHandler Disposed;

        /// <inheritdoc/>
        public IUnitOfWorkOptions Options { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyList<IDataFilterConfiguration> Filters
        {
            get { return this._filters.AsReadOnly(); }
        }
        private readonly List<IDataFilterConfiguration> _filters;

        /// <summary>
        /// Gets default UOW options.
        /// </summary>
        protected IUnitOfWorkDefaultOptions DefaultOptions { get; }

        /// <summary>
        /// Gets the connection string resolver.
        /// </summary>
        protected IConnectionStringResolver ConnectionStringResolver { get; }

        /// <summary>
        /// Gets a value indicates that this unit of work is disposed or not.
        /// </summary>
        public Boolean IsDisposed { get; private set; }

        /// <summary>
        /// Reference to current ABP session.
        /// </summary>
        public IApplicationSession AppSession { protected get; set; }

        protected IUnitOfWorkFilterExecuter FilterExecuter { get; }

        /// <summary>
        /// Is <see cref="Begin"/> method called before?
        /// </summary>
        private Boolean _isBeginCalledBefore;

        /// <summary>
        /// Is <see cref="Complete"/> method called before?
        /// </summary>
        private Boolean _isCompleteCalledBefore;

        /// <summary>
        /// Is this unit of work successfully completed.
        /// </summary>
        private Boolean _succeed;

        /// <summary>
        /// A reference to the exception if this unit of work failed.
        /// </summary>
        private Exception _exception;


        /// <summary>
        /// Constructor.
        /// </summary>
        protected UnitOfWorkBase(
            IConnectionStringResolver connectionStringResolver, 
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter)
        {
            this.FilterExecuter = filterExecuter;
            this.DefaultOptions = defaultOptions;
            this.ConnectionStringResolver = connectionStringResolver;

            this.Id = Guid.NewGuid().ToString("N");
            this._filters = defaultOptions.Filters.ToList();

            //this.AppSession = NullAppSession.Instance;
        }

        /// <inheritdoc/>
        public void Begin(IUnitOfWorkOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.PreventMultipleBegin();
            this.Options = options; //TODO: Do not set options like that, instead make a copy?

            this.SetFilters(options.FilterOverrides);

            this.BeginUow();
        }

        /// <inheritdoc/>
        public abstract void SaveChanges();

        /// <inheritdoc/>
        public abstract Task SaveChangesAsync();

        /// <inheritdoc/>
        public IDisposable DisableFilter(params String[] filterNames)
        {
            //TODO: Check if filters exists?

            var disabledFilters = new List<String>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = this.GetFilterIndex(filterName);
                if (this._filters[filterIndex].IsEnabled)
                {
                    disabledFilters.Add(filterName);
                    this._filters[filterIndex] = new DataFilterConfiguration(this._filters[filterIndex], false);
                }
            }

            disabledFilters.ForEach(this.ApplyDisableFilter);

            return new DisposeAction(() => this.EnableFilter(disabledFilters.ToArray()));
        }

        /// <inheritdoc/>
        public IDisposable EnableFilter(params String[] filterNames)
        {
            //TODO: Check if filters exists?

            var enabledFilters = new List<String>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = this.GetFilterIndex(filterName);
                if (!this._filters[filterIndex].IsEnabled)
                {
                    enabledFilters.Add(filterName);
                    this._filters[filterIndex] = new DataFilterConfiguration(this._filters[filterIndex], true);
                }
            }

            enabledFilters.ForEach(this.ApplyEnableFilter);

            return new DisposeAction(() => this.DisableFilter(enabledFilters.ToArray()));
        }

        /// <inheritdoc/>
        public Boolean IsFilterEnabled(String filterName)
        {
            return this.GetFilter(filterName).IsEnabled;
        }

        /// <inheritdoc/>
        public IDisposable SetFilterParameter(String filterName, String parameterName, Object value)
        {
            var filterIndex = this.GetFilterIndex(filterName);

            var newfilter = new DataFilterConfiguration(this._filters[filterIndex]);

            //Store old value
            Object oldValue = null;
            var hasOldValue = newfilter.FilterParameters.ContainsKey(parameterName);
            if (hasOldValue)
            {
                oldValue = newfilter.FilterParameters[parameterName];
            }

            newfilter.FilterParameters[parameterName] = value;

            this._filters[filterIndex] = newfilter;

            this.ApplyFilterParameterValue(filterName, parameterName, value);

            return new DisposeAction(() =>
            {
                //Restore old value
                if (hasOldValue)
                {
                    this.SetFilterParameter(filterName, parameterName, oldValue);
                }
            });
        }

        /// <inheritdoc/>
        public void Complete()
        {
            this.PreventMultipleComplete();
            try
            {
                this.CompleteUow();
                this._succeed = true;
                this.OnCompleted();
            }
            catch (Exception ex)
            {
                this._exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task CompleteAsync()
        {
            this.PreventMultipleComplete();
            try
            {
                await this.CompleteUowAsync();
                this._succeed = true;
                this.OnCompleted();
            }
            catch (Exception ex)
            {
                this._exception = ex;
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.IsDisposed = true;

            if (!this._succeed)
            {
                this.OnFailed(this._exception);
            }

            this.DisposeUow();
            this.OnDisposed();
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected virtual void BeginUow()
        {
            
        }

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();

        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();

        protected virtual void ApplyDisableFilter(String filterName)
        {
            this.FilterExecuter.ApplyDisableFilter(this, filterName);
        }

        protected virtual void ApplyEnableFilter(String filterName)
        {
            this.FilterExecuter.ApplyEnableFilter(this, filterName);
        }

        protected virtual void ApplyFilterParameterValue(String filterName, String parameterName, Object value)
        {
            this.FilterExecuter.ApplyFilterParameterValue(this, filterName, parameterName, value);
        }

        protected virtual String ResolveConnectionString(ConnectionStringResolveArgs args)
        {
            return this.ConnectionStringResolver.GetNameOrConnectionString(args);
        }

        /// <summary>
        /// Called to trigger <see cref="Completed"/> event.
        /// </summary>
        protected virtual void OnCompleted()
        {
            this.Completed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(Exception exception)
        {
            this.Failed?.Invoke(this, new UnitOfWorkFailedEventArgs(exception));
        }

        /// <summary>
        /// Called to trigger <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            this.Disposed?.Invoke(this,EventArgs.Empty);
        }

        private void PreventMultipleBegin()
        {
            if (this._isBeginCalledBefore)
            {
                throw new DCFApplicationException("This unit of work has started before. Can not call Start method more than once.");
            }

            this._isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (this._isCompleteCalledBefore)
            {
                throw new DCFApplicationException("Complete is called before!");
            }

            this._isCompleteCalledBefore = true;
        }

        private void SetFilters(List<IDataFilterConfiguration> filterOverrides)
        {
            for (var i = 0; i < this._filters.Count; i++)
            {
                var filterOverride = filterOverrides.FirstOrDefault(f => f.FilterName == this._filters[i].FilterName);
                if (filterOverride != null)
                {
                    this._filters[i] = filterOverride;
                }
            }
        }

        private void ChangeFilterIsEnabledIfNotOverrided(List<DataFilterConfiguration> filterOverrides, String filterName, Boolean isEnabled)
        {
            if (filterOverrides.Any(f => f.FilterName == filterName))
            {
                return;
            }

            var index = this._filters.FindIndex(f => f.FilterName == filterName);
            if (index < 0)
            {
                return;
            }

            if (this._filters[index].IsEnabled == isEnabled)
            {
                return;
            }

            this._filters[index] = new DataFilterConfiguration(filterName, isEnabled);
        }

        private IDataFilterConfiguration GetFilter(String filterName)
        {
            var filter = this._filters.FirstOrDefault(f => f.FilterName == filterName);
            if (filter == null)
            {
                throw new DCFApplicationException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filter;
        }

        private Int32 GetFilterIndex(String filterName)
        {
            var filterIndex = this._filters.FindIndex(f => f.FilterName == filterName);
            if (filterIndex < 0)
            {
                throw new DCFApplicationException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filterIndex;
        }
    }
}