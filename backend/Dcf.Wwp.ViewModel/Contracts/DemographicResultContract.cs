using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class DemographicResultContract
    {
        public DemographicResultContract()
        {
            Name = new PersonNameContract();
        }

        public PersonNameContract Name                           { get;  set; }
        public DateTime           DateOfBirth                    { get;  set; }
        public string             Gender                         { get;  set; }
        public string             Ssn                            { get;  set; }
        public string             SsnVerificationCode            { get;  set; }
        public string             SsnVerificationCodeDescription { get;  set; }
        public int                Score                          { get;  set; }
        public decimal?           MciId                          { get;  set; }
        public bool               IsMciKnownToCww                {  get; set; }
    }
}
