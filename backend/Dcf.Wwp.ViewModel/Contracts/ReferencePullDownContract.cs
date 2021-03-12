using System;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class ReferencePullDownContract : IFieldData
    {
        #region Properties

        [DataMember(Name = "id")]
        public int Id { get; protected set; }

        [DataMember(Name = "benefitMonth")]
        public int BenefitMonth { get; protected set; }

        [DataMember(Name = "benefitYear")]
        public int BenefitYear { get; protected set; }

        [DataMember(Name = "pullDownDate")]
        public DateTime PullDownDate { get; protected set; }

        #endregion

        #region Methods

        public static ReferencePullDownContract Create(int id, int benefitMonth, int benefitYear, DateTime pullDownDate)
        {
            var r = new ReferencePullDownContract { Id = id, BenefitMonth = benefitMonth, BenefitYear = benefitYear, PullDownDate = pullDownDate };

            return (r);
        }

        #endregion
    }
}
