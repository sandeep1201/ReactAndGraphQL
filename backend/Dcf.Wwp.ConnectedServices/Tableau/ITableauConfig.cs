
namespace Dcf.Wwp.ConnectedServices.Tableau
{
    public interface ITableauConfig
    {
        #region Properties

        string Env       { get; set; }
        string Endpoint  { get; set; }
        string UserName  { get; set; }
        string SiteId    { get; set; }
        string IpAddress { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
