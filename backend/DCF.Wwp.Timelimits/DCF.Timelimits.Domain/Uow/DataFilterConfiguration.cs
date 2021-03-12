using System;
using System.Collections.Generic;

namespace DCF.Core.Domain.Uow
{
    public class DataFilterConfiguration : IDataFilterConfiguration
    {
        public String FilterName { get; }

        public Boolean IsEnabled { get; }

        public IDictionary<String, Object> FilterParameters { get; }

        public DataFilterConfiguration(String filterName, Boolean isEnabled)
        {
            this.FilterName = filterName;
            this.IsEnabled = isEnabled;
            this.FilterParameters = new Dictionary<String, Object>();
        }

        internal DataFilterConfiguration(IDataFilterConfiguration filterToClone, Boolean? isEnabled = null)
            : this(filterToClone.FilterName, isEnabled ?? filterToClone.IsEnabled)
        {
            foreach (var filterParameter in filterToClone.FilterParameters)
            {
                this.FilterParameters[filterParameter.Key] = filterParameter.Value;
            }
        }
    }
}