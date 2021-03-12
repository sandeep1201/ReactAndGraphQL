using System;

namespace DCF.Core.Reflection
{
    public interface ITypeFinder
    {
        Type[] Find(Func<Type, Boolean> predicate);

        Type[] FindAll();
    }
}