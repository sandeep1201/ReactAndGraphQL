using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "results")]
    public class Results
    {
        public string       icon          { get; set; }
        public string       id            { get; set; }
        public string       name          { get; set; }
        public string       place_id      { get; set; }
        public double       rating        { get; set; }
        public string       reference     { get; set; }
        public string       scope         { get; set; }
        public string[]     types         { get; set; }
        public string       vicinity      { get; set; }
        public Geometry     geometry      { set; get; }
        public OpeningHours opening_hours { get; set; }
        public Photos[]     photos        { get; set; }
    }
}
