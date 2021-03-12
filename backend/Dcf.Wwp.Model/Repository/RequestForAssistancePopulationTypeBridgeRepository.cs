using System;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRequestForAssistancePopulationTypeBridgeRepository
    {
        public IRequestForAssistancePopulationTypeBridge NewRfaPopulationTypeBridge(IRequestForAssistance rfa, int populationTypeId, string user)
        {
            var bridgeRow = new RequestForAssistancePopulationTypeBridge
                            {
                                PopulationTypeId       = populationTypeId,
                                IsDeleted              = false,
                                ModifiedBy             = user,
                                ModifiedDate           = DateTime.Today,
                                RequestForAssistance   = (RequestForAssistance)rfa
                            };
            _db.RequestForAssistancePopulationTypeBridges.Add(bridgeRow);

            return (bridgeRow);
        }

        public void DeleteAllRfaPopulationTypeBridgeRows(int rfaId)
        {
            var oldrows = _db.RequestForAssistancePopulationTypeBridges.Where(i => i.RequestForAssistanceId == rfaId ).ToList();

            if (oldrows.Any())
            {
                _db.RequestForAssistancePopulationTypeBridges.RemoveRange(oldrows);
            }
        }
    }
}
