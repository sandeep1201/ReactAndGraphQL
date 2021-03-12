namespace Dcf.Wwp.Api.Library.Contracts
{
    public class JRHistoryInfoContract
    {
        public JRHistoryInfoContract()
        {
        }

        public int    Id                    { get; set; }
        public string LastJobDetails        { get; set; }
        public string AccomplishmentDetails { get; set; }
        public string StrengthDetails       { get; set; }
        public string AreasNeedImprove      { get; set; }
    }
}
