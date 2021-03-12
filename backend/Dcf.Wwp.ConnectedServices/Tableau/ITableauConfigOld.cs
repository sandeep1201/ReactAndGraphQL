using System.Net;

namespace Dcf.Wwp.ConnectedServices.Tableau
{
    public interface ITableauConfigOld
    {
        #region Properties

        string Env       { get; set; }
        string Endpoint  { get; set; }
        string Uid       { get; set; }
        string SiteId    { get; set; }
        string IpAddress { get; set; }

        //        IPAddress IPAddress { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
