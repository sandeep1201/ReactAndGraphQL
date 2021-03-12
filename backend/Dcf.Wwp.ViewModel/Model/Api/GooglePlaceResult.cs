namespace Dcf.Wwp.Api.Library.Model.Api
{
    [System.Runtime.Serialization.DataContractAttribute()]
    public class GooglePlaceResult
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object[] html_attributions;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public Result[] results;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string status;
    }
}
