
namespace Dcf.Wwp.Api.Library.Contracts
{
    public  class NonSelfDirectedActivity
    {
        public LocationContract BusinessLocation      { get; set; }
        public string           BusinessName          { get; set; }
        public decimal?         BusinessPhoneNumber   { get; set; }
        public string           BusinessStreetAddress { get; set; }
        public string           BusinessZipAddress    { get; set; }
    }
}
