using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Authorization
{
    /// <summary>
    /// This context is used on <see cref="AuthorizationProvider.SetPermissions"/> method.
    /// </summary>
    public interface IPermissionDefinitionContext
    {
        /// <summary>
        /// Creates a new permission under this group.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <param name="displayName">Display name of the permission</param>
        /// <param name="description">A brief description for this permission</param>
        /// <param name="multiTenancySides">Which side can use this permission</param>
        /// <param name="featureDependency">Depended feature(s) of this permission</param>
        /// <returns>New created permission</returns>
        IPermission CreatePermission(String name );

        /// <summary>
        /// Gets a permission with given name or null if can not find.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <returns>Permission object or null</returns>
        IPermission GetPermissionOrNull(String name);
    }
}
