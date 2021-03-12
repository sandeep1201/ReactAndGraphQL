using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core
{
    //TODO: Move Common interfaces in Common DLL and out of core.interfaces
    /// <summary>
    /// Used to represent a named type selector.
    /// </summary>
    public class NamedTypeSelector : INamedTypeSelector
    {
        /// <summary>
        /// Name of the selector.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Predicate.
        /// </summary>
        public Func<Type, Boolean> Predicate { get; set; }

        /// <summary>
        /// Creates new <see cref="NamedTypeSelector"/> object.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="predicate">Predicate</param>
        public NamedTypeSelector(String name, Func<Type, Boolean> predicate)
        {
            Name = name;
            Predicate = predicate;
        }
    }
}
