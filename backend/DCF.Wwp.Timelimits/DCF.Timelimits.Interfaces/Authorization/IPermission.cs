using System;
using System.Collections.Generic;

namespace DCF.Core.Authorization
{
    public interface IPermission
    {
        IReadOnlyList<IPermission> Children { get; }
        String Description { get; set; }
        String DisplayName { get; set; }
        String Name { get; }
        IPermission Parent { get; }

        IPermission CreateChildPermission(String name, String displayName = null, String description = null);
    }
}