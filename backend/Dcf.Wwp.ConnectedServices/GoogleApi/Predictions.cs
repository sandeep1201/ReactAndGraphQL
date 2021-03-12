using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "predictions")]
    public  class Predictions
    {
        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string place_id { get; set; }

        [DataMember]
        public Terms[] terms { get; set; }

        [DataMember]
        public StructuredFormatting structured_formatting { get; set; }
    }
}