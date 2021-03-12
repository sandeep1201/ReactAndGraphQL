using System;
using System.Collections.Generic;

namespace DCF.Core.Plugins
{
    public interface IPlugInSourceList : IList<IPlugInSource>
    {
        List<Type> GetAllModules();
    }
}