using System;
using DCF.Core.Domain.Entities;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// Standard filters of ABP.
    /// </summary>
    public static class DataFilters
    {
        /// <summary>
        /// "SoftDelete".
        /// Soft delete filter.
        /// Prevents getting deleted data from database.
        /// See <see cref="ISoftDelete"/> interface.
        /// </summary>
        public const String SoftDelete = "SoftDelete";

    }
}