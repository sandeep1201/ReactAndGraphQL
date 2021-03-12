using System;

namespace DCF.Core.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if <see cref="ModifiedDate"/> of this entity must be stored.
    /// <see cref="ModifiedDate"/> is automatically set when updating <see cref="Entity"/>.
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? ModifiedDate { get; set; }
    }
}