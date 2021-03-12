namespace Dcf.Wwp.Api.Core
{
    interface IConnectedServicesConfig
    {
        #region Properties

        string Env             { get; set; }
        string Endpoint        { get; set; }
        string MciUid          { get; set; }
        string MciPwd          { get; set; }
        string MciTo           { get; set; }
        string CwwIndSvcUid    { get; set; }
        string CwwIndSvcPwd    { get; set; }
        string CwwIndSvcTo     { get; set; }
        string CwwKeySecSvcUid { get; set; }
        string CwwKeySecSvcPwd { get; set; }
        
        string CwwKeySecSvcTo  { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
