using System;
using System.IO;
using System.Net;
using System.Text;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.ConnectedServices.Tableau
{
    public class TableauApi : ITableauApi
    {
        #region Properties

        private readonly ITableauConfig _config;

        #endregion

        #region Methods

        public TableauApi (ITableauConfig config)
        {
            _config = config;
        }

        public string GetTrustedTicket()
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(_config.Endpoint);

                dynamic encoding = new ASCIIEncoding();
                var     postData = "username=" + _config.UserName + "&target_site=" + _config.SiteId;
                byte[]  data     = encoding.GetBytes(postData);

                request.Method        = "POST";
                request.ContentType   = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                var outStream = request.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();
                var response  = (HttpWebResponse) request.GetResponse();
                var inStream  = new StreamReader(response.GetResponseStream(), encoding);
                var resString = inStream.ReadToEnd();
                inStream.Close();

                return (resString);
            }
            catch (Exception ex)
            {
                throw new Exception("HttpWebRequest could not be created with Tableau API.");
            }
        }

        #endregion
    }
}
