using System;
using Dcf.Wwp.Model.Interface.Cww;

namespace Dcf.Wwp.Model.Cww
{
    public class SocialSecurityStatus : ISocialSecurityStatus
    {
        public string    Participant  { get; set; }
        public string    FirstName    { get; set; }
        public string    Middle       { get; set; }
        public string    LastName     { get; set; }
        public DateTime? Dob          { get; set; }
        public string    Relationship { get; set; }
        public string    Age          { get; set; }
        public string    FedSsi       { get; set; }
        public string    StateSsi     { get; set; }
        public string    Ssa          { get; set; }
    }
}
