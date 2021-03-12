using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.Cww;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.Utils
{
    public static class CwwHelper
    {
        public static List<Child> GetCwwChildren(IRepository repo, IParticipant participant)
        {
            var children = repo.CwwCurrentChildren(participant.PinNumber.ToString());

            return children.Select(chld => new Child()
                                           {
                                               Age           = chld.Age,
                                               BirthDate     = chld.BirthDate?.ToShortDateString(),
                                               FirstName     = chld.FirstName,
                                               Gender        = chld.Gender,
                                               LastName      = chld.LastName,
                                               MiddleInitial = chld.Middle,
                                               Relationship  = chld.Relationship
                                           })
                           .ToList();
        }

        public static ChildCareEligibility GetChildCareEligibility(IRepository repo, IParticipant participant)
        {
            ChildCareEligibility cwwEligibility = null;
            var                  ccEligiblity   = repo.CwwChildCareEligibiltyStatus(participant.PinNumber.ToString());

            if (ccEligiblity != null)
            {
                cwwEligibility = new ChildCareEligibility
                                 {
                                     EligibilityStatus = ccEligiblity.EligibilityStatus,
                                     ReasonCode        = ccEligiblity.ReasonCode,
                                     Description       = ccEligiblity.DescriptionText,
                                     ReasonCode1       = ccEligiblity.ReasonCode1,
                                     Description1      = ccEligiblity.DescriptionText1,
                                     ReasonCode2       = ccEligiblity.ReasonCode2,
                                     Description2      = ccEligiblity.DescriptionText2
                                 };
            }

            return cwwEligibility;
        }
    }
}
