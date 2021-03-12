using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "result")]
    public class Result
    {
        [DataMember]
        public string adr_address { get; set; }

        [DataMember]
        public string formatted_address { get; set; }

        [DataMember]
        public string formatted_phone_number { get; set; }

        [DataMember]
        public string icon { get; set; }

        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string international_phone_number { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string place_id { get; set; }

        [DataMember]
        public double rating { get; set; }

        [DataMember]
        public string reference { get; set; }

        [DataMember]
        public string scope { get; set; }

        [DataMember]
        public string[] types { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public int user_ratings_total { get; set; }

        [DataMember]
        public int utc_offset { get; set; }

        [DataMember]
        public string vicinity { get; set; }

        [DataMember]
        public string website { get; set; }

        [DataMember]
        public Geometry geometry { get; set; }

        [DataMember]
        public Photos[] photos { get; set; }

        [DataMember]
        public Reviews[] reviews { get; set; }

        [DataMember]
        public AddressComponents[] address_components { get; set; }
    }
}
