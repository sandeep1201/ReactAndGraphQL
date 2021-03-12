using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRequestForAssistancePopulationTypeBridgeRepository
    {
        IRequestForAssistancePopulationTypeBridge NewRfaPopulationTypeBridge(IRequestForAssistance rfa, int populationTypeId, string user);
        void DeleteAllRfaPopulationTypeBridgeRows(int rfaId);
    }
}


