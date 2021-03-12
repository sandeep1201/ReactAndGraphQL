using System;
using Microsoft.Extensions.Configuration;
using DCF.Common.Configuration;

namespace Dcf.Wwp.Api.Core
{
    public class ConnectedServicesConfig : IConnectedServicesConfig
    {
        #region Properties

        public string Env             { get; set; }
        public string Endpoint        { get; set; }
        public string MciTo           { get; set; }
        public string MciUid          { get; set; }
        public string MciPwd          { get; set; }
        public string CwwIndSvcTo     { get; set; }
        public string CwwIndSvcUid    { get; set; }
        public string CwwIndSvcPwd    { get; set; }
        public string CwwKeySecSvcTo  { get; set; }
        public string CwwKeySecSvcUid { get; set; }
        public string CwwKeySecSvcPwd { get; set; }

        private IConfiguration _config;

        #endregion

        #region Methods

        public ConnectedServicesConfig ()
        {
            var idx = 0;

            var cb = new ConfigurationBuilder()
                     .AddEnvironmentVariables("WWP_")
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("connectedServices.json");

            var config = cb.Build();

            var dbc = new DatabaseConfiguration();

            switch (dbc.Catalog)
            {
                case "WWPDEV":
                case "WWPSYS":
                    idx = 0;
                    break;

                case "WWPACC":
                    idx = 1;
                    break;

                case "WWPTRN":
                    idx = 2;
                    break;

                case "WWP":
                    idx = 3;
                    break;

                default:
                    break;
            }

            Env             = dbc.Catalog;
            Endpoint        = config[$"wcfSoap:{idx}:endpoint"];
            MciTo           = config[$"wcfSoap:{idx}:services:0:to"];
            MciUid          = config[$"wcfSoap:{idx}:services:0:uid"];
            MciPwd          = (Env == "WWPTRN" ? config["MCI_CRED2"] : config["MCI_CRED"]);
            
            CwwIndSvcTo     = config[$"wcfSoap:{idx}:services:1:to"];
            CwwIndSvcUid    = config[$"wcfSoap:{idx}:services:1:uid"];
            CwwIndSvcPwd    = (Env == "WWPTRN" ? config["CWW_INDV_CRED2"] : config["CWW_INDV_CRED"]);
            
            CwwKeySecSvcTo  = config[$"wcfSoap:{idx}:services:2:to"];
            CwwKeySecSvcUid = config[$"wcfSoap:{idx}:services:2:uid"];
            CwwKeySecSvcPwd = (Env == "WWPTRN" ? config["CWW_KEYSEC_CRED2"] : config["CWW_KEYSEC_CRED"]);
        }

        #endregion
    }
}
