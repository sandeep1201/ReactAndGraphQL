using System;

namespace DCF.Common
{
    public class DataFilters
    {
        /// <summary>
        /// "SoftDelete".
        /// Soft delete filter.
        /// Prevents getting deleted data from database.
        /// See <see cref="DCF.Core.Domain.ISoftDelete"/> interface.
        /// </summary>
        public const String SoftDelete = "SoftDelete";
    }
}