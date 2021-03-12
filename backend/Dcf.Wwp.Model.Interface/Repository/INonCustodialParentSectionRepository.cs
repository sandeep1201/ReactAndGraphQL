using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialParentSectionRepository
    {
        INonCustodialParentsSection NewNonCustodialParentsSection(int participantId, string user);
    }
}