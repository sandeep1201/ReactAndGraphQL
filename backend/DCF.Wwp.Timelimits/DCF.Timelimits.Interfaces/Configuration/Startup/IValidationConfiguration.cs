using System;
using System.Collections.Generic;

namespace DCF.Core.Configuration.Startup
{
    public interface IValidationConfiguration
    {
        List<Type> IgnoredTypes { get; }
    }
}