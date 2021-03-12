using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DCF.Common.Exceptions;

namespace DCF.Core.Domain.Uow
{
    internal class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
    {
        public TransactionScopeOption Scope { get; set; }

        /// <inheritdoc/>
        public Boolean IsTransactional { get; set; }

        /// <inheritdoc/>
        public TimeSpan? Timeout { get; set; }

        /// <inheritdoc/>
        public IsolationLevel? IsolationLevel { get; set; }

        public IReadOnlyList<IDataFilterConfiguration> Filters
        {
            get { return this._filters; }
        }
        private readonly List<IDataFilterConfiguration> _filters;

        public void RegisterFilter(String filterName, Boolean isEnabledByDefault)
        {
            if (this._filters.Any(f => f.FilterName == filterName))
            {
                throw new DCFApplicationException("There is already a filter with name: " + filterName);
            }

            this._filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }

        public void OverrideFilter(String filterName, Boolean isEnabledByDefault)
        {
            this._filters.RemoveAll(f => f.FilterName == filterName);
            this._filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }

        public UnitOfWorkDefaultOptions()
        {
            this._filters = new List<IDataFilterConfiguration>();
            this.IsTransactional = true;
            this.Scope = TransactionScopeOption.Required;
        }
    }
}