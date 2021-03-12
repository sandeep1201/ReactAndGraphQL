using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "reviews")]
    public  class Reviews
    {
        [DataMember]
        public Aspects[] aspects { get; set; }

        [DataMember]
        public string author_name { get; set; }

        [DataMember]
        public string author_url { get; set; }

        [DataMember]
        public string language { get; set; }

        [DataMember]
        public string profile_photo_url { get; set; }

        [DataMember]
        public int rating { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public int time { get; set; }
    }
}
