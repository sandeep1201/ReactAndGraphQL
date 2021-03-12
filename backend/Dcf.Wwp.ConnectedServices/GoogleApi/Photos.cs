using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "photos")]
    public  class Photos
    {
        [DataMember]
        public int height                   { get; set; }

        [DataMember]
        public string[] html_attributions   { get; set; }

        [DataMember]
        public string photo_reference       { get; set; }

        [DataMember]
        public int width                    { get; set; }
    }
}
