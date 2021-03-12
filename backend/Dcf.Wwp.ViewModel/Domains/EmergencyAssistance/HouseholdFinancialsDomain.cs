using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        private EAHouseHoldFinancialsContract GetEaHouseHoldFinancials(EARequest request, ICollection<EAHouseHoldIncome> houseHoldIncomes, ICollection<EAAssets> assets, ICollection<EAVehicles> vehicles)
        {
            houseHoldIncomes = houseHoldIncomes?.Where(i => !i.IsDeleted).ToList();
            assets           = assets?.Where(i => !i.IsDeleted).ToList();
            vehicles         = vehicles?.Where(i => !i.IsDeleted).ToList();

            var contract = new EAHouseHoldFinancialsContract
                           {
                               RequestId                = request.Id,
                               HasNoIncome              = request.HasNoIncome,
                               HasNoAssets              = request.HasNoAssets,
                               HasNoVehicles            = request.HasNoVehicles,
                               ModifiedBy               = _convertWIUIdToName(request.ModifiedBy),
                               ModifiedDate             = request.ModifiedDate,
                               IsSubmittedViaDriverFlow = request.HasNoIncome != null || request.HasNoAssets != null || request.HasNoVehicles != null
                           };

            if (houseHoldIncomes != null && houseHoldIncomes.Any())
            {
                contract.EaHouseHoldIncomes = houseHoldIncomes.Select(i => new EAHouseHoldIncomeContract
                                                                           {
                                                                               Id                   = i.Id,
                                                                               IncomeType           = i.IncomeType,
                                                                               MonthlyIncome        = i.MonthlyIncome?.ToString("N2"),
                                                                               VerificationTypeId   = i.VerificationTypeId,
                                                                               VerificationTypeName = i.EaVerificationType?.Name,
                                                                               GroupMember          = i.GroupMember
                                                                           }).ToList();
                contract.IsSubmittedViaDriverFlow = true;
            }

            if (assets != null && assets.Any())
            {
                contract.EaAssets = assets.Select(i => new EAAssetsContract
                                                       {
                                                           Id                   = i.Id,
                                                           AssetType            = i.AssetType,
                                                           CurrentValue         = i.CurrentValue?.ToString("N2"),
                                                           VerificationTypeId   = i.VerificationTypeId,
                                                           VerificationTypeName = i.EaVerificationType?.Name,
                                                           AssetOwner           = i.AssetOwner
                                                       }).ToList();
                contract.IsSubmittedViaDriverFlow = true;
            }

            if (vehicles == null || !vehicles.Any()) return contract;
            {
                contract.EaVehicles = vehicles.Select(i => new EAVehiclesContract
                                                           {
                                                               Id                               = i.Id,
                                                               VehicleType                      = i.VehicleType,
                                                               VehicleValue                     = i.VehicleValue?.ToString("N2"),
                                                               AmountOwed                       = i.AmountOwed?.ToString("N2"),
                                                               VehicleEquity                    = i.VehicleEquity?.ToString("N2"),
                                                               OwnerVerificationTypeId          = i.OwnershipVerificationTypeId,
                                                               OwnerVerificationTypeName        = i.EaOwnershipVerificationType?.Name,
                                                               VehicleValueVerificationTypeId   = i.VehicleValueVerificationTypeId,
                                                               VehicleValueVerificationTypeName = i.EaVehicleValueVerificationType?.Name,
                                                               OwedVerificationTypeId           = i.OwedVerificationTypeId,
                                                               OwedVerificationTypeName         = i.EaOwnershipVerificationType?.Name,
                                                               VehicleOwner                     = i.VehicleOwner
                                                           }).ToList();
                contract.IsSubmittedViaDriverFlow = true;
            }

            return contract;
        }

        public EAHouseHoldFinancialsContract UpsertHouseHoldFinancials(EAHouseHoldFinancialsContract contract)
        {
            var modifiedBy       = _authUser.WIUID;
            var modifiedDate     = DateTime.Now;
            var request          = _eaRequestRepository.Get(i => i.Id == contract.RequestId && !i.IsDeleted);
            var houseHoldIncomes = request.EaHouseHoldIncomes?.Where(i => !i.IsDeleted).ToList();
            var assets           = request.EaAssetses?.Where(i => !i.IsDeleted).ToList();
            var vehicles         = request.EaVehicleses?.Where(i => !i.IsDeleted).ToList();

            request.HasNoIncome   = contract.HasNoIncome;
            request.HasNoAssets   = contract.HasNoAssets;
            request.HasNoVehicles = contract.HasNoVehicles;
            request.ModifiedBy    = modifiedBy;
            request.ModifiedDate  = modifiedDate;

            houseHoldIncomes?.ForEach(i => i.IsDeleted = true);

            contract.EaHouseHoldIncomes.ForEach(i =>
                                                {
                                                    var houseHoldIncome = houseHoldIncomes?.FirstOrDefault(j => j.Id == i.Id) ?? _eaHouseHoldIncomeRepository.New();

                                                    houseHoldIncome.EaRequest          = request;
                                                    houseHoldIncome.IncomeType         = i.IncomeType;
                                                    houseHoldIncome.MonthlyIncome      = i.MonthlyIncome.ToDecimal();
                                                    houseHoldIncome.VerificationTypeId = i.VerificationTypeId;
                                                    houseHoldIncome.GroupMember        = i.GroupMember;
                                                    houseHoldIncome.IsDeleted          = false;
                                                    houseHoldIncome.ModifiedBy         = modifiedBy;
                                                    houseHoldIncome.ModifiedDate       = modifiedDate;

                                                    if (i.Id == 0)
                                                        request.EaHouseHoldIncomes?.Add(houseHoldIncome);
                                                });

            assets?.ForEach(i => i.IsDeleted = true);

            contract.EaAssets.ForEach(i =>
                                      {
                                          var asset = assets?.FirstOrDefault(j => j.Id == i.Id) ?? _eaAssetsRepository.New();

                                          asset.EaRequest          = request;
                                          asset.AssetType          = i.AssetType;
                                          asset.CurrentValue       = i.CurrentValue.ToDecimal();
                                          asset.VerificationTypeId = i.VerificationTypeId;
                                          asset.AssetOwner         = i.AssetOwner;
                                          asset.IsDeleted          = false;
                                          asset.ModifiedBy         = modifiedBy;
                                          asset.ModifiedDate       = modifiedDate;

                                          if (i.Id == 0)
                                              request.EaAssetses?.Add(asset);
                                      });

            vehicles?.ForEach(i => i.IsDeleted = true);

            contract.EaVehicles?.ForEach(i =>
                                         {
                                             var vehicle = vehicles?.FirstOrDefault(j => j.Id == i.Id) ?? _eaVehiclesRepository.New();

                                             vehicle.EaRequest                      = request;
                                             vehicle.VehicleType                    = i.VehicleType;
                                             vehicle.VehicleValue                   = i.VehicleValue.ToDecimal();
                                             vehicle.AmountOwed                     = i.AmountOwed.ToDecimal();
                                             vehicle.VehicleEquity                  = i.VehicleEquity.ToDecimal();
                                             vehicle.OwnershipVerificationTypeId    = i.OwnerVerificationTypeId;
                                             vehicle.VehicleValueVerificationTypeId = i.VehicleValueVerificationTypeId;
                                             vehicle.OwedVerificationTypeId         = i.OwedVerificationTypeId;
                                             vehicle.VehicleOwner                   = i.VehicleOwner;
                                             vehicle.IsDeleted                      = false;
                                             vehicle.ModifiedBy                     = modifiedBy;
                                             vehicle.ModifiedDate                   = modifiedDate;

                                             if (i.Id == 0)
                                                 request.EaVehicleses?.Add(vehicle);
                                         });

            request.ModifiedBy   = modifiedBy;
            request.ModifiedDate = modifiedDate;

            _unitOfWork.Commit();

            return GetEaHouseHoldFinancials(request, request.EaHouseHoldIncomes, request.EaAssetses, request.EaVehicleses);
        }

        #endregion
    }
}
