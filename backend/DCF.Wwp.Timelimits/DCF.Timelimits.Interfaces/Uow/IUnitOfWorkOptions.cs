using System;
using System.Collections.Generic;
using System.Transactions;

namespace DCF.Core.Domain.Uow
{
    public interface IUnitOfWorkOptions
    {
        TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }
        List<IDataFilterConfiguration> FilterOverrides { get; }
        IsolationLevel? IsolationLevel { get; set; }
        Boolean? IsTransactional { get; set; }
        TransactionScopeOption? Scope { get; set; }
        TimeSpan? Timeout { get; set; }

        void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions);
    }
}