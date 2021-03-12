using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using IWorkerRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerRepository;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class OrganizationInformationDomain : IOrganizationInformationDomain
    {
        #region Properties

        private readonly IOrganizationInformationRepository _organizationInformationRepository;
        private readonly ICityDomain                        _cityDomain;
        private readonly IUnitOfWork                        _unitOfWork;
        private readonly IAuthUser                          _authUser;
        private readonly Func<string, string>               _convertWIUIdToName;

        #endregion

        #region Methods

        public OrganizationInformationDomain(IOrganizationInformationRepository organizationInformationRepository,
                                             ICityDomain                        cityDomain,
                                             IUnitOfWork                        unitOfWork,
                                             IAuthUser                          authUser,
                                             IWorkerRepository                  workerRepo)
        {
            _organizationInformationRepository = organizationInformationRepository;
            _cityDomain                        = cityDomain;
            _unitOfWork                        = unitOfWork;
            _authUser                          = authUser;

            _convertWIUIdToName = (wiuId) =>
                                  {
                                      var wo = workerRepo.GetAsQueryable()
                                                         .Where(i => i.WIUId == wiuId)
                                                         .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                         .FirstOrDefault();

                                      var wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");

                                      return (wn);
                                  };
        }


        public OrganizationInformationContract GetIOrganizationInformation(int progId, int orgId)
        {
            OrganizationInformationContract contract = null;
            var                             orgInfo  = _organizationInformationRepository.Get(i => i.EnrolledProgramId == progId && i.OrganizationId == orgId);

            if (orgInfo != null)
            {
                contract = new OrganizationInformationContract
                           {
                               Id               = orgInfo.Id,
                               ProgramId        = orgInfo.EnrolledProgramId,
                               ProgramName      = orgInfo.EnrolledProgram.Name,
                               OrganizationId   = orgInfo.OrganizationId,
                               OrganizationName = orgInfo.Organization.AgencyName,
                               Locations        = new List<FinalistLocationContract>(),
                               ModifiedBy       = _convertWIUIdToName(orgInfo.ModifiedBy),
                               ModifiedDate     = orgInfo.ModifiedDate
                           };

                var locations = orgInfo.OrganizationLocations?.Where(i => !i.IsDeleted).ToList();
                locations?.OrderByDescending(x => x.EndDate ?? DateTime.MaxValue).ForEach(l =>
                                                                                          {
                                                                                              var lookupId = l.AddressVerificationTypeLookupId;
                                                                                              contract.Locations.Add(new FinalistLocationContract
                                                                                                                     {
                                                                                                                         Id = l.Id,
                                                                                                                         FinalistAddress = new FinalistAddressContract
                                                                                                                                           {
                                                                                                                                               AddressLine1        = l.AddressLine1,
                                                                                                                                               City                = l.City?.Name,
                                                                                                                                               State               = l.City?.State?.Code,
                                                                                                                                               Zip                 = l.ZipCode,
                                                                                                                                               UseSuggestedAddress = lookupId == AddressVerificationType.FinalistVerifiedId,
                                                                                                                                               UseEnteredAddress   = lookupId == AddressVerificationType.WorkerOverrideId || lookupId == AddressVerificationType.FinalistUnverified
                                                                                                                                           },
                                                                                                                         EffectiveDate = l.EffectiveDate.ToString("MM/dd/yyyy"),
                                                                                                                         EndDate       = (l.EndDate != null ? l.EndDate.GetValueOrDefault().ToString("MM/dd/yyyy") : "")
                                                                                                                     });
                                                                                          });
            }

            return contract;
        }

        public OrganizationInformationContract UpsertIOrganizationInformation(OrganizationInformationContract organizationInformationContractContract, int id)
        {
            OrganizationInformationContract contract = null;

            if (organizationInformationContractContract == null)
            {
                throw new ArgumentNullException(nameof(organizationInformationContractContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;

            var orgInfo = organizationInformationContractContract.Id != 0
                              ? _organizationInformationRepository.Get(g => g.Id == organizationInformationContractContract.Id && g.IsDeleted == false)
                              : _organizationInformationRepository.New();

            if (organizationInformationContractContract.Id != 0)
            {
                if (orgInfo != null)
                {
                    orgInfo.EnrolledProgramId = organizationInformationContractContract.ProgramId;
                    orgInfo.OrganizationId    = organizationInformationContractContract.OrganizationId;
                    orgInfo.ModifiedBy        = modifiedBy;
                    orgInfo.ModifiedDate      = modifiedDate;

                    if (orgInfo.OrganizationLocations != null)
                    {
                        var locationsToUpdate = organizationInformationContractContract.Locations?.Where(i => i.Id != 0).Select(i => i).ToList();
                        var locationsToAdd    = organizationInformationContractContract.Locations?.Where(i => i.Id == 0).Select(i => i).ToList();

                        foreach (var locationFromContract in locationsToUpdate.AsNotNull())
                        {
                            var location = orgInfo.OrganizationLocations.FirstOrDefault(l => l.Id == locationFromContract.Id);

                            if (location != null)
                            {
                                location.City = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: locationFromContract.FinalistAddress, isClientReg: true);

                                if (location.City != null) location.City.IsDeleted = false;
                                location.AddressLine1                    = locationFromContract.FinalistAddress.AddressLine1;
                                location.ZipCode                         = locationFromContract.FinalistAddress.Zip;
                                location.AddressVerificationTypeLookupId = locationFromContract.FinalistAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
                                location.EffectiveDate                   = Convert.ToDateTime(locationFromContract.EffectiveDate);
                                location.EndDate                         = locationFromContract.EndDate.ToDateTimeMonthDayYear();
                                location.ModifiedBy                      = modifiedBy;
                                location.ModifiedDate                    = modifiedDate;
                            }
                        }

                        foreach (var locationFromContract in locationsToAdd.AsNotNull())
                        {
                            var city = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: locationFromContract.FinalistAddress, isClientReg: true);

                            var location = new OrganizationLocation
                                           {
                                               OrganizationInformation         = orgInfo,
                                               AddressLine1                    = locationFromContract.FinalistAddress.AddressLine1,
                                               City                            = city,
                                               ZipCode                         = locationFromContract.FinalistAddress.Zip,
                                               AddressVerificationTypeLookupId = locationFromContract.FinalistAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId,
                                               EffectiveDate                   = Convert.ToDateTime(locationFromContract.EffectiveDate),
                                               EndDate                         = locationFromContract.EndDate.ToDateTimeMonthDayYear(),
                                               ModifiedBy                      = modifiedBy,
                                               ModifiedDate                    = modifiedDate
                                           };

                            orgInfo.OrganizationLocations.Add(location);
                        }
                    }

                    _organizationInformationRepository.Update(orgInfo);
                    _unitOfWork.Commit();
                    contract = GetIOrganizationInformation(orgInfo.EnrolledProgramId, orgInfo.OrganizationId);
                }
            }
            else
            {
                orgInfo.EnrolledProgramId = organizationInformationContractContract.ProgramId;
                orgInfo.OrganizationId    = organizationInformationContractContract.OrganizationId;
                orgInfo.ModifiedBy        = modifiedBy;
                orgInfo.ModifiedDate      = modifiedDate;

                organizationInformationContractContract.Locations.AsNotNull().ForEach(i =>
                                                                                      {
                                                                                          var city = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: i.FinalistAddress, isClientReg: true);

                                                                                          var l = new OrganizationLocation
                                                                                                  {
                                                                                                      OrganizationInformation         = orgInfo,
                                                                                                      AddressLine1                    = i.FinalistAddress.AddressLine1,
                                                                                                      City                            = city,
                                                                                                      ZipCode                         = i.FinalistAddress.Zip,
                                                                                                      AddressVerificationTypeLookupId = i.FinalistAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId,
                                                                                                      EffectiveDate                   = Convert.ToDateTime(i.EffectiveDate),
                                                                                                      EndDate                         = i.EndDate.ToDateTimeMonthDayYear(),
                                                                                                      ModifiedBy                      = modifiedBy,
                                                                                                      ModifiedDate                    = modifiedDate
                                                                                                  };
                                                                                          orgInfo.OrganizationLocations.Add(l);
                                                                                      });

                _organizationInformationRepository.Add(orgInfo);
                _unitOfWork.Commit();
                contract = GetIOrganizationInformation(orgInfo.EnrolledProgramId, orgInfo.OrganizationId);
            }

            return (contract);
        }

        #endregion
    }
}
