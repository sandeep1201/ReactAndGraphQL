using System.Linq;

namespace Dcf.Wwp.Api.Library.Model.Api
{
    // Type created for JSON at <<root>>
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class GooglePlaceDetails
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object[] html_attributions;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Result result;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string status;
    }

    public partial class GooglePlaceDetails
    {
        public string GetAddressTypeDescription(string type)
        {
            if (result?.address_components != null && result.address_components.Length > 0)
            {
                return (from x in result.address_components where x.types.Contains(type) select x.long_name).FirstOrDefault();
            }

            return string.Empty;
        }

        public string GetAddressTypeShortDescription(string type)
        {
            if (result?.address_components != null && result.address_components.Length > 0)
            {
                return (from x in result.address_components where x.types.Contains(type) select x.short_name).FirstOrDefault();
            }

            return string.Empty;
        }
    }
}
