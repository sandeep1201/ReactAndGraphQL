using System;
using System.Collections.Generic;
using System.Transactions;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// Used to get/set default options for a unit of work.
    /// </summary>
    public interface IUnitOfWorkDefaultOptions
    {
        /// <summary>
        /// Scope option.
        /// </summary>
        TransactionScopeOption Scope { get; set; }

        /// <summary>
        /// Should unit of works be transactional.
        /// Default: true.
        /// </summary>
        Boolean IsTransactional { get; set; }

        /// <summary>
        /// Gets/sets a timeout value for unit of works.
        /// </summary>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets/sets isolation level of transaction.
        /// This is used if <see cref="IsTransactional"/> is true.
        /// </summary>
        IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Gets list of all data filter configurations.
        /// </summary>
        IReadOnlyList<IDataFilterConfiguration> Filters { get; }

        /// <summary>
        /// Registers a data filter to unit of work system.
        /// </summary>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="isEnabledByDefault">Is filter enabled by default.</param>
        void RegisterFilter(String filterName, Boolean isEnabledByDefault);

        /// <summary>
        /// Overrides a data filter definition.
        /// </summary>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="isEnabledByDefault">Is filter enabled by default.</param>
        void OverrideFilter(String filterName, Boolean isEnabledByDefault);
    }
}