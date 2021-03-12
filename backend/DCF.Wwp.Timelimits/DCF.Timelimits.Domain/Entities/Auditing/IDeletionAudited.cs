using System;

namespace DCF.Core.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// </summary>
    public interface IDeletionAudited 
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        Int64? DeleterUserId { get; set; }
    }
}