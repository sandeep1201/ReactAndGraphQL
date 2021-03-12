using System;
using System.Collections.Generic;

namespace DCF.Core.Domain.Uow
{
    public interface IDataFilterConfiguration
    {
        String FilterName { get; }
        IDictionary<String, Object> FilterParameters { get; }
        Boolean IsEnabled { get; }
    }
}