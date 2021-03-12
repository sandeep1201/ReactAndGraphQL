using System;
using Dcf.Wwp.Api.Library.Extensions;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class FeatureToggleContract
    {
        public int      Id             { get; set; }
        public string   ParameterName  { get; set; }
        public string   ParameterValue { get; set; }
        public string ModifiedDate   { get; set; }

        public static FeatureToggleContract CreateContract(int id, string parameterName, string parameterValue, string modifiedDate)
        {
            return new FeatureToggleContract { Id = id, ParameterName = parameterName.SafeTrim(), ParameterValue = parameterValue.SafeTrim(), ModifiedDate = modifiedDate };
        }
    }
}
