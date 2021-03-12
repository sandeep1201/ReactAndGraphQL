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

namespace Dcf.Wwp.Api.Library.Domains
{
    public class CareerAssessmentDomain : ICareerAssessmentDomain
    {
        #region Properties

        private readonly IParticipantRepository                   _participantRepository;
        private readonly ICareerAssessmentRepository              _careerAssessmentRepository;
        private readonly ICareerAssessmentElementBridgeRepository _careerAssessmentElementBridgeRepository;
        private readonly IUnitOfWork                              _unitOfWork;
        private readonly IAuthUser                                _authUser;
        private readonly ITransactionDomain                       _transactionDomain;
        private readonly IWorkerRepository                        _workerRepo;
        private readonly Func<string, string>                     _convertWIUIdToName;

        #endregion

        #region Methods

        public CareerAssessmentDomain(IParticipantRepository                   participantRepository,
                                      ICareerAssessmentRepository              careerAssessmentRepository,
                                      ICareerAssessmentElementBridgeRepository careerAssessmentElementBridgeRepository,
                                      IUnitOfWork                              unitOfWork,
                                      IAuthUser                                authUser,
                                      IWorkerRepository                        workerRepo,
                                      ITransactionDomain                       transactionDomain)
        {
            _participantRepository                   = participantRepository;
            _careerAssessmentRepository              = careerAssessmentRepository;
            _careerAssessmentElementBridgeRepository = careerAssessmentElementBridgeRepository;
            _unitOfWork                              = unitOfWork;
            _authUser                                = authUser;
            _transactionDomain                       = transactionDomain;
            _workerRepo                              = workerRepo;

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

        public List<CareerAssessmentContract> GetCareerAssessmentsForPin(decimal pin)
        {
            var contracts         = new List<CareerAssessmentContract>();
            var participantId     = _participantRepository.Get(i => i.PinNumber == pin).Id;
            var careerAssessments = _careerAssessmentRepository.GetMany(i => i.ParticipantId == participantId && i.IsDeleted == false).ToList();

            careerAssessments.OrderByDescending(i => i.CompletionDate)
                             .ThenByDescending(i => i.ModifiedDate)
                             .ForEach(i => contracts.Add(new CareerAssessmentContract
                                                         {
                                                             Id                          = i.Id,
                                                             ParticipantId               = participantId,
                                                             AssessmentProvider          = i.AssessmentProvider,
                                                             AssessmentToolUsed          = i.AssessmentToolUsed,
                                                             AssessmentResultAppliedToEP = i.AssessmentResultAppliedToEP,
                                                             CompletionDate              = i.CompletionDate,
                                                             CreatedDate                 = i.CreatedDate,
                                                             CareerAssessmentContactId   = i.CareerAssessmentContactId,
                                                             AssessmentResults           = i.AssessmentResults,
                                                             ElementIds                  = i.CareerAssessmentElementBridges.Select(a => a.ElementId).ToList(),
                                                             RelatedOccupation           = i.RelatedOccupation,
                                                             IsDeleted                   = i.IsDeleted,
                                                             ModifiedBy                  = _convertWIUIdToName(i.ModifiedBy),
                                                             ModifiedDate                = i.ModifiedDate
                                                         }));


            return contracts;
        }

        public CareerAssessmentContract GetCareerAssessment(int id)
        {
            CareerAssessmentContract contract         = null;
            var                      careerAssessment = _careerAssessmentRepository.Get(i => i.Id == id);

            if (careerAssessment != null)
            {
                contract = new CareerAssessmentContract
                           {
                               Id                          = careerAssessment.Id,
                               ParticipantId               = careerAssessment.ParticipantId,
                               AssessmentProvider          = careerAssessment.AssessmentProvider,
                               AssessmentToolUsed          = careerAssessment.AssessmentToolUsed,
                               AssessmentResultAppliedToEP = careerAssessment.AssessmentResultAppliedToEP,
                               CompletionDate              = careerAssessment.CompletionDate,
                               CreatedDate                 = careerAssessment.CreatedDate,
                               CareerAssessmentContactId   = careerAssessment.CareerAssessmentContactId,
                               ElementIds                  = careerAssessment.CareerAssessmentElementBridges.Select(a => a.ElementId).ToList(),
                               AssessmentResults           = careerAssessment.AssessmentResults,
                               RelatedOccupation           = careerAssessment.RelatedOccupation,
                               IsDeleted                   = careerAssessment.IsDeleted,
                               ModifiedBy                  = _convertWIUIdToName(careerAssessment.ModifiedBy),
                               ModifiedDate                = careerAssessment.ModifiedDate
                           };
            }

            return contract;
        }

        public CareerAssessmentContract UpsertCareerAssessment(CareerAssessmentContract careerAssessmentContract, string pin)
        {
            if (careerAssessmentContract == null)
            {
                throw new ArgumentNullException(nameof(careerAssessmentContract));
            }

            var modifiedBy       = _authUser.WIUID;
            var modifiedDate     = DateTime.Now;
            var decimalPin       = decimal.Parse(pin);
            var participant      = _participantRepository.Get(i => i.PinNumber == decimalPin);
            var careerAssessment = careerAssessmentContract.Id != 0 ? _careerAssessmentRepository.Get(c => c.Id == careerAssessmentContract.Id && c.IsDeleted == false) : _careerAssessmentRepository.New();

            careerAssessment.AssessmentProvider          = careerAssessmentContract.AssessmentProvider;
            careerAssessment.AssessmentResultAppliedToEP = careerAssessmentContract.AssessmentResultAppliedToEP;
            careerAssessment.AssessmentToolUsed          = careerAssessmentContract.AssessmentToolUsed;
            careerAssessment.AssessmentResults           = careerAssessmentContract.AssessmentResults;
            careerAssessment.CareerAssessmentContactId   = careerAssessmentContract.CareerAssessmentContactId;
            careerAssessment.CompletionDate              = careerAssessmentContract.CompletionDate;
            careerAssessment.RelatedOccupation           = careerAssessmentContract.RelatedOccupation;

            if (careerAssessmentContract.ElementIds != null)
            {
                var allIds        = careerAssessment.CareerAssessmentElementBridges?.Select(i => i.ElementId).ToList();
                var contractIds   = careerAssessmentContract.ElementIds;
                var idsToDelete   = allIds?.Except(contractIds.AsNotNull()).ToList();
                var idsToUpdate   = allIds?.Where(i => contractIds.Contains(i)).ToList();
                var idsToAdd      = allIds      != null ? contractIds.Except(allIds).ToList() : contractIds;
                var typesToDelete = idsToDelete != null ? careerAssessment.CareerAssessmentElementBridges?.Where(i => idsToDelete.Contains(i.ElementId)).Select(i => i).ToList() : null;
                var typesToUpdate = idsToUpdate != null ? careerAssessment.CareerAssessmentElementBridges?.Where(i => idsToUpdate.Contains(i.ElementId)).Select(i => i).ToList() : null;


                typesToDelete?.ForEach(type => _careerAssessmentElementBridgeRepository.Delete(type));

                foreach (var type in typesToUpdate.AsNotNull().Select(typeFromContract => careerAssessment.CareerAssessmentElementBridges.FirstOrDefault(i => i.ElementId == typeFromContract.ElementId)).Where(type => type != null))
                {
                    type.ModifiedBy   = modifiedBy;
                    type.ModifiedDate = modifiedDate;
                }

                foreach (var id in idsToAdd.AsNotNull().Select(typeFromContract => new CareerAssessmentElementBridge
                                                                                   {
                                                                                       CareerAssessment = careerAssessment,
                                                                                       ElementId        = typeFromContract,
                                                                                       IsDeleted        = false,
                                                                                       ModifiedBy       = modifiedBy,
                                                                                       ModifiedDate     = modifiedDate
                                                                                   }))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    careerAssessment.CareerAssessmentElementBridges.Add(id);
                }
            }

            careerAssessment.IsDeleted     = false;
            careerAssessment.ParticipantId = participant.Id;
            careerAssessment.ModifiedBy    = modifiedBy;
            careerAssessment.ModifiedDate  = modifiedDate;

            if (careerAssessmentContract.Id == 0)
            {
                careerAssessment.CreatedDate = modifiedDate;
                _careerAssessmentRepository.Add(careerAssessment);
            }
            else
            {
                _careerAssessmentRepository.Update(careerAssessment);
            }

            #region Transaction

            var officeId = participant.ParticipantEnrolledPrograms
                                      .Where(i => _authUser.Authorizations
                                                           .Where(j => j.StartsWith("canAccessProgram_"))
                                                           .Select(j => j.Trim().ToLower().Split('_')[1])
                                                           .Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                                  && i.Office.ContractArea.Organization.EntsecAgencyCode.Trim() == _authUser.AgencyCode.Trim())
                                      .OrderByDescending(i => i.EnrollmentDate)
                                      .First().Office.Id;

            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = participant.Id,
                                          WorkerId            = _workerRepo.Get(x => x.WIUId == _authUser.WIUID).Id,
                                          OfficeId            = officeId,
                                          EffectiveDate       = modifiedDate,
                                          CreatedDate         = modifiedDate,
                                          TransactionTypeCode = TransactionTypes.CareerAssessment,
                                          ModifiedBy          = modifiedBy
                                      };

            _transactionDomain.InsertTransaction(transactionContract);

            #endregion

            var rowCommitted = _unitOfWork.Commit();

            if (rowCommitted > 0)
            {
                careerAssessmentContract.Id = careerAssessment.Id;
            }

            var contract = GetCareerAssessment(careerAssessment.Id);

            return contract;
        }

        #endregion
    }
}
