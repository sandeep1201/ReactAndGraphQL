using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class SupportiveServicesDomain : ISupportiveServicesDomain
    {
        #region Properties

        private readonly ISupportiveServiceRepository _servicesRepo;
        private readonly IUnitOfWork                  _unitOfWork;
        private readonly IAuthUser                    _authUser;

        #endregion

        #region Methods

        public SupportiveServicesDomain(ISupportiveServiceRepository servicesRepo,
                                        IUnitOfWork                  unitOfWork,
                                        IAuthUser                    authUser)
        {
            _servicesRepo = servicesRepo;
            _unitOfWork   = unitOfWork;
            _authUser     = authUser;
        }

        public List<SupportiveServiceContract> GetSupportiveServicesForEP(int epId)
        {
            var services = _servicesRepo.GetMany(i => i.EmployabilityPlanId == epId && i.IsDeleted == false)
                                        .Select(i => new SupportiveServiceContract
                                                     {
                                                         Id                        = i.Id,
                                                         EmployabilityPlanId       = i.EmployabilityPlanId,
                                                         Details                   = i.Details,
                                                         SupportiveServiceTypeId   = i.SupportiveServiceTypeId,
                                                         SupportiveServiceTypeName = i.SupportiveServiceType?.Name
                                                     }).ToList();

            return (services);
        }

        public List<SupportiveServiceContract> Upsert(List<SupportiveServiceContract> supportiveServicesContract, int epId)
        {
            if (supportiveServicesContract == null)
            {
                throw new ArgumentNullException(nameof(supportiveServicesContract));
            }

            // A. wanted to get away from this (Upsert) pattern, but can't change Angular at this point in the game.
            // B. So like Nike we have to just 'do it' - this way.
            // 0. Divide and Conquer strategy Delete, then Updates, then Insert.
            // 1. Get all the supportive services for this EP (we could just grab the epId from the contracts, but...)
            var updateTime = DateTime.Now;
            var services   = _servicesRepo.GetMany(ss => ss.EmployabilityPlanId == epId).ToList();

            // 2. find the ones that didn't come back from Angular
            var allIds           = services.Select(ssc => ssc.Id).ToList();
            var contactIds       = supportiveServicesContract.Select(ssc => ssc.Id).ToList();
            var idsToDelete      = allIds.Except(contactIds.AsNotNull()).ToList();
            var servicesToDelete = services.Where(i => idsToDelete.Contains(i.Id)).Select(i => i).ToList();
            var servicesToUpdate = supportiveServicesContract.Where(i => i.Id != 0).Select(i => i).ToList();
            var servicesToAdd    = supportiveServicesContract.Where(i => i.Id == 0).Select(i => i).ToList();

            // 3. delete them
            servicesToDelete.ForEach(service => _servicesRepo.Delete(service));

            // 4. now process the rest.. (insert or update)
            // 4.1 first the updates

            foreach (var serviceFromContract in servicesToUpdate.AsNotNull())
            {
                var service = services.FirstOrDefault(s => s.Id == serviceFromContract.Id);

                if (service != null)
                {
                    // let AutoMapper do this in tech debt
                    service.SupportiveServiceTypeId = serviceFromContract.SupportiveServiceTypeId;
                    service.Details                 = serviceFromContract.Details;
                    service.ModifiedBy              = _authUser.WIUID;
                    service.ModifiedDate            = updateTime;
                    _servicesRepo.Update(service);
                }
            }

            // now the Inserts
            foreach (var serviceFromContract in servicesToAdd.AsNotNull())
            {
                var service = _servicesRepo.New();

                // let AutoMapper do this in tech debt
                service.EmployabilityPlanId     = epId;
                service.SupportiveServiceTypeId = serviceFromContract.SupportiveServiceTypeId;
                service.Details                 = serviceFromContract.Details;
                service.ModifiedBy              = _authUser.WIUID;
                service.ModifiedDate            = updateTime;
                _servicesRepo.Add(service);
            }

            _unitOfWork.Commit();   // The new 'Save()' - Commits all changes on the context

            // We could just pass the contract back minus the deleted ones and avoid a database call - tech debt?
            // follow the pattern ;(
            var supportiveServicesForEp = GetSupportiveServicesForEP(epId);

            return (supportiveServicesForEp);
        }

        #endregion
    }
}

// Inserts (using Nav props)
//if (servicesToAdd != null && servicesToAdd.Any())
//{
//    var ep = _epRepo.Get(i => i.Id == epId);

//    if (ep != null)
//    {
//        ep.SupportiveServices = ep.SupportiveServices ?? new List<SupportiveService>();
//        foreach (var serviceFromContract in servicesToAdd)
//        {
//            var service = _servicesRepo.New();

//            // let AutoMapper do this in tech debt
//            service.EmployabilityPlanId     = epId;
//            service.SupportiveServiceTypeId = serviceFromContract.SupportiveServiceTypeId;
//            service.Details                 = serviceFromContract.Details;
//            service.ModifiedBy              = _authUser.WIUID;
//            service.ModifiedDate            = updateTime;
//            ep.SupportiveServices.Add(service);
//            _epRepo.Update(ep);
//        }
//    }
//}
