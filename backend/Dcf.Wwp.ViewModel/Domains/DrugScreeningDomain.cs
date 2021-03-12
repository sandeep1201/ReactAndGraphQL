using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;
using IWorkerRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerRepository;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class DrugScreeningDomain : IDrugScreeningDomain
    {
        private readonly IParticipantRepository         _participantRepository;
        private readonly IDrugScreeningRepository       _drugScreeningRepository;
        private readonly IDrugScreeningStatusRepository _drugScreeningStatusRepository;
        private readonly IUnitOfWork                    _unitOfWork;
        private readonly IAuthUser                      _authUser;
        private readonly Func<string, string>           _convertWIUIdToName;


        public DrugScreeningDomain(IParticipantRepository         participantRepository,
                                   IDrugScreeningRepository       drugScreeningRepository,
                                   IDrugScreeningStatusRepository drugScreeningStatusRepository,
                                   IUnitOfWork                    unitOfWork,
                                   IAuthUser                      authUser,
                                   IWorkerRepository              workerRepo
        )
        {
            _participantRepository         = participantRepository;
            _drugScreeningRepository       = drugScreeningRepository;
            _drugScreeningStatusRepository = drugScreeningStatusRepository;
            _unitOfWork                    = unitOfWork;
            _authUser                      = authUser;
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

        public DrugScreeningContract GetDrugScreeningForPin(decimal pin)
        {
            var                   drugScreening         = _drugScreeningRepository.Get(i => i.Participant.PinNumber == pin && !i.IsDeleted);
            DrugScreeningContract drugScreeningContract = null;

            if (drugScreening != null)
            {
                drugScreeningContract = new DrugScreeningContract
                                        {
                                            Id                        = drugScreening.Id,
                                            ParticipantId             = drugScreening.ParticipantId,
                                            UsedNonRequiredDrugs      = drugScreening.UsedNonRequiredDrugs,
                                            AbusedMoreDrugs           = drugScreening.AbusedMoreDrugs,
                                            CannotStopAbusingDrugs    = drugScreening.CannotStopAbusingDrugs,
                                            FeelGuiltyAboutDrugs      = drugScreening.FeelGuiltyAboutDrugs,
                                            HadBlackoutsFromDrugs     = drugScreening.HadBlackoutsFromDrugs,
                                            IllegalActivitiesForDrugs = drugScreening.IllegalActivitiesForDrugs,
                                            MedicalProblemsFromDrugs  = drugScreening.MedicalProblemsFromDrugs,
                                            NeglectedFamilyForDrugs   = drugScreening.NeglectedFamilyForDrugs,
                                            SickFromStoppingDrugs     = drugScreening.SickFromStoppingDrugs,
                                            SpouseComplaintOnDrugs    = drugScreening.SpouseComplaintOnDrugs,
                                            IsDeleted                 = drugScreening.IsDeleted,
                                            ModifiedBy                = _convertWIUIdToName(drugScreening.ModifiedBy),
                                            ModifiedDate              = drugScreening.ModifiedDate,
                                            DrugScreeningStatuses = drugScreening.DrugScreeningStatuses?.OrderByDescending(i => i.Id)
                                                                                 .Select(i => new DrugScreeningStatusContract
                                                                                              {
                                                                                                  Id                             = i.Id,
                                                                                                  DrugScreeningStatusTypeId      = i.DrugScreeningStatusTypeId,
                                                                                                  DrugScreeningStatusName        = i.DrugScreeningStatusType.Name,
                                                                                                  DrugScreeningStatusDisplayName = i.DrugScreeningStatusType.DisplayName,
                                                                                                  DrugScreeningStatusDate        = i.DrugScreeningStatusDate,
                                                                                                  Details                        = i.Details,
                                                                                                  ModifiedBy                     = _convertWIUIdToName(i.ModifiedBy),
                                                                                                  ModifiedDate                   = i.ModifiedDate
                                                                                              }).ToList()
                                        };
            }

            return drugScreeningContract;
        }

        public DrugScreeningContract UpsertDrugScreening(DrugScreeningContract drugScreeningContract, string pin)
        {
            if (drugScreeningContract == null)
            {
                throw new ArgumentNullException(nameof(drugScreeningContract));
            }

            var decimalPin   = decimal.Parse(pin);
            var participant  = _participantRepository.Get(i => i.PinNumber == decimalPin);
            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;

            var drugScreening = drugScreeningContract.Id != 0
                                    ? _drugScreeningRepository.Get(i => i.Id == drugScreeningContract.Id && !i.IsDeleted)
                                    : _drugScreeningRepository.New();
            var drugScreeningStatus = _drugScreeningStatusRepository.New();

            drugScreening.Participant               = participant;
            drugScreening.AbusedMoreDrugs           = drugScreeningContract.AbusedMoreDrugs;
            drugScreening.CannotStopAbusingDrugs    = drugScreeningContract.CannotStopAbusingDrugs;
            drugScreening.FeelGuiltyAboutDrugs      = drugScreeningContract.FeelGuiltyAboutDrugs;
            drugScreening.HadBlackoutsFromDrugs     = drugScreeningContract.HadBlackoutsFromDrugs;
            drugScreening.IllegalActivitiesForDrugs = drugScreeningContract.IllegalActivitiesForDrugs;
            drugScreening.MedicalProblemsFromDrugs  = drugScreeningContract.MedicalProblemsFromDrugs;
            drugScreening.NeglectedFamilyForDrugs   = drugScreeningContract.NeglectedFamilyForDrugs;
            drugScreening.SpouseComplaintOnDrugs    = drugScreeningContract.SpouseComplaintOnDrugs;
            drugScreening.SickFromStoppingDrugs     = drugScreeningContract.SickFromStoppingDrugs;
            drugScreening.UsedNonRequiredDrugs      = drugScreeningContract.UsedNonRequiredDrugs;
            drugScreening.IsDeleted                 = false;
            drugScreening.ModifiedBy                = modifiedBy;
            drugScreening.ModifiedDate              = modifiedDate;

            drugScreeningStatus.DrugScreening             = drugScreening;
            drugScreeningStatus.DrugScreeningStatusDate   = DateTime.Today;
            drugScreeningStatus.Details                   = !string.IsNullOrWhiteSpace(drugScreeningContract.Details) ? drugScreeningContract.Details : null;
            drugScreeningStatus.ModifiedBy                = modifiedBy;
            drugScreeningStatus.ModifiedDate              = modifiedDate;

            if (drugScreeningContract.DrugScreeningStatusTypeId != null)
            {
                drugScreeningStatus.DrugScreeningStatusTypeId = (int) drugScreeningContract.DrugScreeningStatusTypeId;
            }

            drugScreening.DrugScreeningStatuses.Add(drugScreeningStatus);

            if (drugScreeningContract.Id == 0)
                _drugScreeningRepository.Add(drugScreening);

            _unitOfWork.Commit();

            drugScreeningContract.Id                        = drugScreening.Id;
            drugScreeningContract.IsDeleted                 = drugScreening.IsDeleted;
            drugScreeningContract.ModifiedBy                = drugScreening.ModifiedBy;
            drugScreeningContract.ModifiedDate              = drugScreening.ModifiedDate;
            drugScreeningContract.DrugScreeningStatusTypeId = null;
            drugScreeningContract.Details                   = null;

            if (drugScreeningContract.DrugScreeningStatuses == null)
                drugScreeningContract.DrugScreeningStatuses = new List<DrugScreeningStatusContract>();

            drugScreeningContract.DrugScreeningStatuses.Add(new DrugScreeningStatusContract
                                                            {
                                                                Id                             = drugScreeningStatus.Id,
                                                                DrugScreeningStatusTypeId      = drugScreeningStatus.DrugScreeningStatusTypeId,
                                                                DrugScreeningStatusName        = drugScreeningStatus.DrugScreeningStatusType.Name,
                                                                DrugScreeningStatusDisplayName = drugScreeningStatus.DrugScreeningStatusType.DisplayName,
                                                                DrugScreeningStatusDate        = drugScreeningStatus.DrugScreeningStatusDate,
                                                                Details                        = drugScreeningStatus.Details,
                                                                ModifiedBy                     = _convertWIUIdToName(drugScreeningStatus.ModifiedBy),
                                                                ModifiedDate                   = drugScreeningStatus.ModifiedDate
                                                            });

            drugScreeningContract.DrugScreeningStatuses.Sort((i, j) => j.Id.CompareTo(i.Id));

            return drugScreeningContract;
        }
    }
}
