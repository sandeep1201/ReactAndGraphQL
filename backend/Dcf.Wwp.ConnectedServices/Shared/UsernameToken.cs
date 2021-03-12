using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public class UsernameToken
    {
        #region Properties

        private readonly string   _uid;
        private readonly string   _pwd;
        private          string   _id;
        private          string   _nonce;
        private          DateTime _created;

        #endregion

        #region Methods

        public UsernameToken(string uid, string pwd)
        {
            _uid = uid;
            _pwd = pwd;
        }

        public XElement GetXml()
        {
            _id = "UsernameToken-" + Guid.NewGuid();

            var _nonceBytes = new byte[16];

            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(_nonceBytes);
                _nonce = Convert.ToBase64String(_nonceBytes);
            }

            var _created        = DateTime.Now.ToUniversalTime();
            var _passwordDigest = ComputePasswordDigest(_nonceBytes.Clone() as byte[], _created, _pwd);

            XNamespace nsWsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace nsWsu  = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            var xeSecNs = new XAttribute[2];
            xeSecNs[0] = new XAttribute(XNamespace.Xmlns + "wsse", nsWsse);
            xeSecNs[1] = new XAttribute(XNamespace.Xmlns + "wsu",  nsWsu);

            var xaType    = new XAttribute("Type",         "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
            var xaEncType = new XAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            var xaTokenId = new XAttribute(nsWsu + "Id",   _id);

            var xe = new XElement(nsWsse + "UsernameToken", xaTokenId,
                                  new XElement(nsWsse + "Username", _uid),
                                  new XElement(nsWsse + "Password", xaType,    Convert.ToBase64String(_passwordDigest)),
                                  new XElement(nsWsse + "Nonce",    xaEncType, _nonce),
                                  new XElement(nsWsu  + "Created",  $"{_created:yyyy-MM-ddTHH:mm:ssZ}"));

            return (xe);
        }

        private static byte[] ComputePasswordDigest(byte[] nonce, DateTime created, string secret)
        {
            if (nonce == null || nonce.Length == 0)
            {
                throw new ArgumentNullException(nameof(nonce));
            }

            if (secret == null)
            {
                throw new ArgumentNullException(nameof(secret));
            }

            var bytes1   = Encoding.UTF8.GetBytes(XmlConvert.ToString(created.ToUniversalTime(), "yyyy-MM-ddTHH:mm:ssZ"));
            var bytes2   = Encoding.UTF8.GetBytes(secret);
            var numArray = new byte[nonce.Length + bytes1.Length + bytes2.Length];
            Array.Copy(nonce,  numArray, nonce.Length);
            Array.Copy(bytes1, 0,        numArray, nonce.Length,                 bytes1.Length);
            Array.Copy(bytes2, 0,        numArray, nonce.Length + bytes1.Length, bytes2.Length);

            return Hash(numArray);
        }

        private static byte[] Hash(byte[] value) => SHA1.Create().ComputeHash(value);

        #endregion
    }
}
