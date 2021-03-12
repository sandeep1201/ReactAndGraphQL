namespace Dcf.Wwp.BritsBatch.Models
{
    /// <remarks>
    /// These are intentionally numbered to match
    /// the enterprise-wide numerice ones.
    /// </remarks>
    public enum AppEnvironment
    {
        Dev  = 1,
        Sys  = 2,
        Acc  = 3,
        Trn  = 4,
        Uat  = 6,   // this one is not defined in W2 or TL so it throws off the numbers
        Prod = 5,
    }
}
