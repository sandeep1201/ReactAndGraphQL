using System;
using System.Linq;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Model.Interface;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ExtensionReasonContract : BaseModelContract
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public ClockTypes ValidClockTypes { get; set; }


        public static ExtensionReasonContract Create(IExtensionReason x)
        {
            var extReasonContract = ExtensionReasonContract.Create(x.Name);
            BaseModelContract.SetBaseProperties(extReasonContract,x);
            extReasonContract.ValidClockTypes = (ClockTypes) x.TimeLimitTypes.Sum(t => t.Id);
            return extReasonContract;

        }
        private static ExtensionReasonContract Create(String name)
        {
            var extensionReasonContract = new ExtensionReasonContract
            {
                Name = name,
            };

            return extensionReasonContract;
        }
    }
}