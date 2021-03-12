using System;

namespace DCF.Core
{
    public interface INamedTypeSelector
    {
        String Name { get; set; }
        Func<Type, Boolean> Predicate { get; set; }
    }
}