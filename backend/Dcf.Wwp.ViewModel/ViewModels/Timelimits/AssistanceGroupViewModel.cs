using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Linq;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class AssistanceGroupViewModel : BaseViewModel
    {
        public AssistanceGroupViewModel(IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
        }

        public AssistanceGroupContract GetParticipantAssistanceGroupByPin(String pin)
        {
            var assistanceGroupContract = new AssistanceGroupContract();

            var assistanceGroupMembers =
                this.Repo.ParticipantAssistanceGroupByPin(pin).ToList();

            foreach (var agm in assistanceGroupMembers)
            {
                var amgContract = AssistanceGroupMemberContract.Create(
                    agm.PinNumber.GetValueOrDefault(),
                    /*!x.IsConfidential*/true
                    , agm.RELATIONSHIP,
                    false);

                if (agm.IsChild())
                {
                    assistanceGroupContract.Children?.Add(amgContract);
                }
                else
                {
                    assistanceGroupContract.Parents?.Add(amgContract);
                }

                //switch (agm.Relationship.ToLower())
                //{
                //    case "friend":
                //    case "husband":
                //    case "wife":
                //    case "not related":
                //    case "non qualified":
                //    case "other qualified":
                //        {

                //            break;
                //        }
                //    case "child":

                //        break;
                //}
            }

            // If we got another parent, add the primary
            if (assistanceGroupContract.Parents.Count > 0)
            {
                var participant = this.Repo.GetRefreshedParticipant(pin);
                AssistanceGroupMemberContract pinPartipant = null;
                if (participant != null)
                {
                    if (assistanceGroupContract.Parents.Count > 0)
                    {
                        pinPartipant = AssistanceGroupMemberContract.Create(participant.PinNumber.GetValueOrDefault(), true, "SELF"/*this.GetOtherRelationship(assistanceGroupContract.Parents.First().Relationship)*/, false);
                    }
                    assistanceGroupContract.Parents.Add(pinPartipant);
                }
            }

            return assistanceGroupContract;
        }

        private String GetOtherRelationship(String relationship)
        {
            switch (relationship.ToLower())
            {
                case "husband":
                    return "Wife";
                case "wife":
                    return "Husband";
                case "friend":
                case "not related":
                case "non qualified":
                case "other qualified":
                default:
                    {
                        return relationship;
                    }
            }
        }
    }
}