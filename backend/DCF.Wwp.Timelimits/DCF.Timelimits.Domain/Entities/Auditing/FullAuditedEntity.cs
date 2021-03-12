using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCF.Core.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntity : FullAuditedEntity<Int32>
    {

    }

    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual Boolean IsDeleted { get; set; }

    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey, TUser>, IFullAudited<TUser>
        where TUser : IEntity<Int64>
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual Boolean IsDeleted { get; set; }

        
    }
}