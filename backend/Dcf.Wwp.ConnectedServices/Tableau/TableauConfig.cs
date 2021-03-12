namespace Dcf.Wwp.ConnectedServices.Tableau
{
    public class TableauConfig : ITableauConfig
    {
        #region Properties

        public string Env       { get; set; }
        public string Endpoint  { get; set; }
      //public Uri    Endpoint  { get; set; }
        public string UserName  { get; set; }
        public string SiteId    { get; set; }
        public string IpAddress { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
