using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IAssistanceGroupRepository
    {
        IEnumerable<AssistanceGroupMember> ParticipantAssistanceGroupByPin(String pin);
    }
}
