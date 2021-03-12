using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IAuxiliaryDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<AuxiliaryContract> GetAuxiliaries(int?                         participantId);
        AuxiliaryContract       GetAuxiliary(int                            id);
        AuxiliaryContract       GetDetailsBasedOnParticipationPeriod(string pin, int participantId, string participationPeriod, short year);
        void                    UpsertAuxiliary(AuxiliaryContract           contract);

        #endregion
    }
}
