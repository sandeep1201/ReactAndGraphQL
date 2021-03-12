using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using Autofac;
using Autofac.Integration.Wcf;
using Microsoft.Web.Services3.Security.Tokens;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.ConnectedServices.Mci;
using Dcf.Wwp.ConnectedServices.Shared;

namespace Dcf.Wwp.ConnectedServices
{
    public class WebServicesModule : Module
    {
        #region Properties

        public string Endpoint        { get; set; }
        public string MciUid          { get; set; }
        public string MciPwd          { get; set; }
        public string MciTo           { get; set; }
        public string CwwIndSvcUid    { get; set; }
        public string CwwIndSvcPwd    { get; set; }
        public string CwwIndSvcTo     { get; set; }
        public string CwwKeySecSvcUid { get; set; }
        public string CwwKeySecSvcPwd { get; set; }
        public string CwwKeySecSvcTo  { get; set; }

        private readonly Func<string, string, MessageHeader> CreateAndSignToken = (uid, pwd) =>
                                                                                  {
                                                                                      var token  = new UsernameToken(uid, pwd, PasswordOption.SendHashed);
                                                                                      var header = MessageHeader.CreateHeader(
                                                                                                                              "Security",
                                                                                                                              "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd",
                                                                                                                               token.GetXml(new XmlDocument())
                                                                                                                             );
                                                                                      return (header);
                                                                                  };
        #endregion

        #region Methods

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            #region MCI Service

            // MCI only supports SOAP 1.1
            var mciEndpoint = new EndpointAddress(Endpoint);
            var mciBinding  = new BasicHttpBinding
                             {
                                 Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport, Transport = { ClientCredentialType = HttpClientCredentialType.None } }
                             };

            var mciSecHeader = CreateAndSignToken(MciUid, MciPwd);

            builder.Register((c, p) =>
                             {
                                 var cf = new ChannelFactory<IMciService>(mciBinding, mciEndpoint);
                                 cf.Endpoint.Behaviors.Add(new EndpointBehavior(new MessageInspector(mciSecHeader, MciTo)));

                                 return (cf);
                             })
                   .As<ChannelFactory<IMciService>>() // .InstancePerDependency();
                   .SingleInstance();                 // The channel factory only needs one instance

            builder.Register(c => c.Resolve<ChannelFactory<IMciService>>().CreateChannel())
                   .As<IMciService>()
                   .UseWcfSafeRelease();

            #endregion

            #region CWW

            // CWW only supports SOAP 1.2 
            var cwwEndpointUri = new Uri(Endpoint);

            var wsBinding = new WSHttpBinding
                            {
                                Name = "cwwBinding", Security = { Mode = SecurityMode.Transport }, Namespace = "MyGoofyNameSpace"
                            };

            builder.RegisterInstance(wsBinding).As<WSHttpBinding>();

            #region CWW Individual Service

            var cwwIndSvcEndpoint = new EndpointAddress(CwwIndSvcTo);
            var cwwIndSvcSecHeader = CreateAndSignToken(CwwIndSvcUid, CwwIndSvcPwd);

            builder.Register(c =>
                             {
                                 var cf = new ChannelFactory<ICwwIndService>(c.Resolve<WSHttpBinding>(), cwwIndSvcEndpoint);
                                 cf.Endpoint.Behaviors.Add(new EndpointBehavior(new MessageInspector(cwwIndSvcSecHeader)));
                                 cf.Endpoint.Behaviors.Add(new ClientViaBehavior(cwwEndpointUri));

                                 return (cf);
                             })
                   .As<ChannelFactory<ICwwIndService>>()
                   .SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<ICwwIndService>>().CreateChannel())
                   .As<ICwwIndService>()
                   .UseWcfSafeRelease();

            #endregion

            #region CWW KeySecurity Service

            var cwwKeySvcSecHeader = CreateAndSignToken(CwwKeySecSvcUid, CwwKeySecSvcPwd);
            var cwwKeySvcEndpoint  = new EndpointAddress(CwwKeySecSvcTo);

            builder.Register(c =>
                             {
                                 var cf = new ChannelFactory<ICwwKeySecService>(c.Resolve<WSHttpBinding>(), cwwKeySvcEndpoint);
                                 cf.Endpoint.Behaviors.Add(new EndpointBehavior(new MessageInspector(cwwKeySvcSecHeader)));
                                 cf.Endpoint.Behaviors.Add(new ClientViaBehavior(cwwEndpointUri));

                                 return (cf);
                             })
                   .As<ChannelFactory<ICwwKeySecService>>()
                   .SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<ICwwKeySecService>>().CreateChannel())
                   .As<ICwwKeySecService>()
                   .UseWcfSafeRelease();

            #endregion

            #endregion
            
            // done...
        }

        #endregion
    }
}
