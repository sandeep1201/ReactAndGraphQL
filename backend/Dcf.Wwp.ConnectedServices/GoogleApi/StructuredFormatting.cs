using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "structured_formatting")]
    public  class StructuredFormatting
    {
        [DataMember]
        public string main_text { get; set; }

        [DataMember]
        public string secondary_text { get; set; }
    }
}