namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IEntSecConfig
    {
        #region Properties

        string Env              { get; set; }
        string Endpoint     { get; set; }
        string ApplicationKey   { get; set; }
        string Username         { get; set; }
        string Password         { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
