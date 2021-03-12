using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface.Cww;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ICwwRepository
    {
        List<ICurrentChild> CwwCurrentChildren(String pin);
        ISP_CWWChildCareEligibiltyStatus_Result CwwChildCareEligibiltyStatus(String caseNum);

        List<ILearnfare> CwwLearnfare(String pin);
        List<ISocialSecurityStatus> CwwSocialSecurityStatus(String pin);
    }
}