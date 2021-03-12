using System;

namespace DCF.Core.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if <see cref="CreatedDate"/> of this entity must be stored.
    /// <see cref="CreatedDate"/> is automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreatedDate { get; set; }
    }
}