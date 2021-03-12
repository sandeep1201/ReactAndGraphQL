using System.Linq;
using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    // Type created for JSON at <<root>>
    [DataContract]
    public class GooglePlaceDetails
    {
        #region Properties

        [DataMember]
        public object[] html_attributions { get; set; }

        [DataMember]
        public Result result { get; set; }

        [DataMember]
        public string status { get; set; }

        #region Methods

        #endregion

        public string GetAddressTypeDescription(string type)
        {
            var r = string.Empty;

            if (result?.address_components != null && result.address_components.Length > 0)
            {
                r = result.address_components
                          .Where(i => i.types.Contains(type))
                          .Select(i => i.long_name)
                          .FirstOrDefault();
            }

            return (r);
        }

        public string GetAddressTypeShortDescription(string type)
        {
            var r = string.Empty;

            if (result?.address_components != null && result.address_components.Length > 0)
            {
                r = result.address_components
                          .Where(i => i.types.Contains(type))
                          .Select(i => i.short_name)
                          .FirstOrDefault();
            }

            return (r);
        }

        #endregion
    }
}
