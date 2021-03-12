using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "opening_hours")]
    public class OpeningHours
    {
        public bool     open_now     { get; set; }
        public object[] weekday_text { get; set; }
    }
}
