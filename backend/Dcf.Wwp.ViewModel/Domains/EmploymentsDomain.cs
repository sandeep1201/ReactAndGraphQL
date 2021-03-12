using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class EpEmploymentsDomain : IEpEmploymentsDomain
    {
        #region Properties

        private readonly IEmployabilityPlanEmploymentInfoBridgeRepository _employabilityPlanEmploymentInfoBridgeRepository;
        private readonly IEmploymentInformationRepository                 _employmentInformationRepository;
        private readonly IEmployabilityPlanRepository                     _employabilityPlanRepository;
        private readonly IEnrolledProgramJobTypeBridgeRepository          _epjtRepo;
        private readonly ILocations                                       _locations;
        private readonly IUnitOfWork                                      _unitOfWork;
        private readonly IAuthUser                                        _authUser;

        #endregion

        public EpEmploymentsDomain(IEmployabilityPlanEmploymentInfoBridgeRepository employabilityPlanEmploymentInfoBridgeRepository,
                                   IEmploymentInformationRepository                 employmentInformationRepository,
                                   IUnitOfWork                                      unitOfWork,
                                   IAuthUser                                        authUser,
                                   IEmployabilityPlanRepository                     employabilityPlanRepository,
                                   ILocations                                       locations,
                                   IEnrolledProgramJobTypeBridgeRepository          epjtRepo)
        {
            _employabilityPlanEmploymentInfoBridgeRepository = employabilityPlanEmploymentInfoBridgeRepository;
            _employmentInformationRepository                 = employmentInformationRepository;
            _unitOfWork                                      = unitOfWork;
            _authUser                                        = authUser;
            _employabilityPlanRepository                     = employabilityPlanRepository;
            _locations                                       = locations;
            _epjtRepo                                        = epjtRepo;
        }

        public List<EpEmploymentContract> GetEmploymentsForEp(string pin, int epId, DateTime epBeginDate, string programCd)
        {
            var decimalPin  = decimal.Parse(pin);
            var contracts   = new List<EpEmploymentContract>();
            var employments = _employmentInformationRepository.GetMany(i => i.Participant.PinNumber == decimalPin && i.DeleteReasonId == null).ToList();
            employments = employments.Where(i => i.JobEndDate == null || i.JobEndDate >= epBeginDate).ToList();

            var epEmployments = _employabilityPlanEmploymentInfoBridgeRepository.GetMany(i => i.EmployabilityPlanId == epId).ToList();
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var jobTypeIds = _epjtRepo.GetMany(i => i.EnrolledProgram.ProgramCode.ToLower() == programCd.ToLower()
                                                    && (i.InActivatedDate == null || i.InActivatedDate >= currentDate) && (i.ActivatedDate <= currentDate))
                                      .Select(i => i.JobTypeId).ToList();
            var progEmployments = employments.Where(i => i.JobTypeId != null && jobTypeIds.Contains((int) i.JobTypeId));

            progEmployments.ForEach(progEmployment =>
                                    {
                                        var id           = epEmployments.FirstOrDefault(i => i.EmploymentInformationId == progEmployment.Id)?.Id ?? 0;
                                        var employmentId = employments.FirstOrDefault(i => i.Id == progEmployment.Id)?.Id;

                                        if (employmentId == null) return;
                                        var contract = new EpEmploymentContract
                                                       {
                                                           Id                      = id,
                                                           EmploymentInformationId = (int) employmentId,
                                                           JobTypeId               = progEmployment.JobTypeId,
                                                           JobTypeName             = progEmployment.JobType?.Name,
                                                           JobBeginDate            = progEmployment.JobBeginDate?.ToString("MM/dd/yyyy"),
                                                           JobEndDate              = progEmployment.JobEndDate?.ToString("MM/dd/yyyy"),
                                                           JobPosition             = progEmployment.JobPosition,
                                                           CompanyName             = progEmployment.CompanyName,
                                                           Location                = _locations.GetLocationInfo(progEmployment, progEmployment.City),
                                                           WageHour                = progEmployment.WageHour?.CurrentAverageWeeklyHours,
                                                           IsSelected              = id != 0
                                                       };
                                        contracts.Add(contract);
                                    });

            return contracts;
        }

        public List<EpEmploymentContract> UpsertEpEmployment(List<EpEmploymentContract> employmentsContract, string pin, int epId)
        {
            if (employmentsContract == null)
            {
                throw new ArgumentNullException(nameof(employmentsContract));
            }

            var updateTime    = DateTime.Now;
            var epEmployments = _employabilityPlanEmploymentInfoBridgeRepository.GetMany(i => i.EmployabilityPlanId == epId).ToList();
            var ep            = _employabilityPlanRepository.Get(i => i.Id == epId);

            // Find the ones that didn't come back from Angular
            var allIds                = epEmployments.Select(i => i.Id).ToList();
            var contractIds           = employmentsContract.Where(i => i.IsSelected).Select(i => i.Id).ToList();
            var idsToDelete           = allIds.Except(contractIds.AsNotNull()).ToList();
            var epEmploymentsToDelete = epEmployments.Where(i => idsToDelete.Contains(i.Id)).Select(i => i).ToList();
            var epEmploymentsToUpdate = employmentsContract.Where(i => i.Id != 0 && i.IsSelected).Select(i => i).ToList();
            var epEmploymentsToAdd    = employmentsContract.Where(i => i.Id == 0 && i.IsSelected).Select(i => i).ToList();

            // Delete them
            epEmploymentsToDelete.ForEach(i => _employabilityPlanEmploymentInfoBridgeRepository.Delete(i));

            // Now process the rest.. (insert or update)
            // First the updates
            foreach (var epeiFromContract in epEmploymentsToUpdate.AsNotNull())
            {
                var epei = epEmployments.FirstOrDefault(i => i.Id == epeiFromContract.Id);

                if (epei != null)
                {
                    epei.EmployabilityPlanId     = epId;
                    epei.EmploymentInformationId = epeiFromContract.EmploymentInformationId;
                    epei.ModifiedBy              = _authUser.WIUID;
                    epei.ModifiedDate            = updateTime;
                    _employabilityPlanEmploymentInfoBridgeRepository.Update(epei);
                }
            }

            // Now the Inserts
            foreach (var epeiFromContract in epEmploymentsToAdd.AsNotNull())
            {
                var epei = _employabilityPlanEmploymentInfoBridgeRepository.New();

                epei.EmployabilityPlanId     = epId;
                epei.EmploymentInformationId = epeiFromContract.EmploymentInformationId;
                epei.ModifiedBy              = _authUser.WIUID;
                epei.ModifiedDate            = updateTime;
                _employabilityPlanEmploymentInfoBridgeRepository.Add(epei);
            }

            _unitOfWork.Commit();

            var epEmp = GetEmploymentsForEp(pin, epId, ep.BeginDate, ep.EnrolledProgram.ProgramCode);

            return epEmp;
        }
    }
}
