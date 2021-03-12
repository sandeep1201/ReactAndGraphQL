using System.Xml.Linq;
using log4net.Appender;

namespace Dcf.Wwp.ConnectedServices
{
    public class WcfAdoNetAppenderParameter : AdoNetAppenderParameter
    {
        #region Properties

        protected static readonly XNamespace NsSoap11Env = "http://schemas.xmlsoap.org/soap/envelope/";
        protected static readonly XNamespace NsSoap12Env = "http://www.w3.org/2003/05/soap-envelope";
        protected static readonly XNamespace NsWsse      = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        protected static readonly XNamespace NsWsu       = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        protected static readonly XNamespace NsWsa       = "http://schemas.xmlsoap.org/ws/2004/08/addressing";

        #endregion

        #region Methods
        
        #endregion
    }
}
