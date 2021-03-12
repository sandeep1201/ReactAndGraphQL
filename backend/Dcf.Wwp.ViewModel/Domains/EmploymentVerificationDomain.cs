using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class EmploymentVerificationDomain : IEmploymentVerificationDomain
    {
        #region Properties

        private readonly IUnitOfWork                       _unitOfWork;
        private readonly IAuthUser                         _authUser;
        private readonly IMapper                           _mapper;
        private readonly IEmploymentInformationRepository  _employmentInformationRepository;
        private readonly IEmploymentVerificationRepository _employmentVerificationRepository;
        private readonly IParticipantRepository            _participantRepository;
        private readonly WIUIDToFullNameModifiedByResolver _fullNameModifiedByResolver;

        #endregion

        #region Methods

        public EmploymentVerificationDomain(IUnitOfWork                       unitOfWork,
                                            IAuthUser                         authUser,
                                            IMapper                           mapper,
                                            IEmploymentInformationRepository  employmentInformationRepository,
                                            IEmploymentVerificationRepository employmentVerificationRepository,
                                            IParticipantRepository            participantRepository,
                                            WIUIDToFullNameModifiedByResolver fullNameModifiedByResolver)
        {
            _unitOfWork                       = unitOfWork;
            _authUser                         = authUser;
            _mapper                           = mapper;
            _employmentInformationRepository  = employmentInformationRepository;
            _employmentVerificationRepository = employmentVerificationRepository;
            _participantRepository            = participantRepository;
            _fullNameModifiedByResolver       = fullNameModifiedByResolver;
        }

        public List<EmploymentVerificationContract> GetTJTMJEmploymentsForParticipantByJobType(int participantId, int jobTypeId, DateTime enrollmentDate)
        {
            var employmentVerificationContracts = new List<EmploymentVerificationContract>();
            var employments                     = _employmentInformationRepository.GetMany(i => i.ParticipantId == participantId && i.JobTypeId == jobTypeId && i.DeleteReasonId == null)
                                                                                  .Where(i => i.JobBeginDate.GetValueOrDefault().IsSameOrAfter(enrollmentDate))
                                                                                  .ToList();
            var employmentIds           = employments.Select(i => i.Id).ToList();
            var employmentVerifications = _employmentVerificationRepository.GetMany(i => employmentIds.Contains(i.EmploymentInformationId));

            employments.ForEach(i =>
                                {
                                    var employmentVerification         = employmentVerifications.FirstOrDefault(j => j.EmploymentInformationId == i.Id);
                                    var employmentVerificationContract = _mapper.Map<EmploymentVerificationContract>(i);

                                    if (employmentVerification != null)
                                    {
                                        employmentVerificationContract.EmploymentVerificationId   = employmentVerification.Id;
                                        employmentVerificationContract.IsVerified                 = employmentVerification.IsVerified;
                                        employmentVerificationContract.ModifiedDate               = employmentVerification.ModifiedDate;
                                        employmentVerificationContract.ModifiedBy                 = _fullNameModifiedByResolver.Resolve(employmentVerification, null, null, null);
                                        employmentVerificationContract.CreatedDate                = employmentVerification.CreatedDate;
                                        employmentVerificationContract.NumberOfDaysAtVerification = employmentVerification.NumberOfDaysAtVerification;
                                    }

                                    employmentVerificationContracts.Add(employmentVerificationContract);
                                });

            return employmentVerificationContracts;
        }

        public void UpsertEmploymentVerification(string pin, List<EmploymentVerificationContract> contract)
        {
            var modifiedDate                         = _authUser.CDODate ?? DateTime.Now;
            var modifiedBy                           = _authUser.WIUID;
            var employmentVerificationIds            = contract.Select(i => i.EmploymentVerificationId).ToList();
            var decimalPin                           = decimal.Parse(pin);
            var participant                          = _participantRepository.Get(i => i.PinNumber                                          == decimalPin);
            var allEmploymentVerifications           = _employmentVerificationRepository.GetMany(i => i.EmploymentInformation.ParticipantId == participant.Id).ToList();
            var employmentVerificationsNotInContract = allEmploymentVerifications.Where(i => contract.All(j => j.EmploymentVerificationId   != i.Id));
            var employmentVerificationsInContract    = allEmploymentVerifications.Where(i => employmentVerificationIds.Contains(i.Id));
            var employmentVerificationsToUpdate      = contract.Where(i => i.EmploymentVerificationId  > 0).Select(i => i).ToList();
            var employmentVerificationsToAdd         = contract.Where(i => (i.EmploymentVerificationId == null     || i.EmploymentVerificationId     == 0) && i.IsVerified != null).Select(i => i).ToList();
            var is60DaysVerified                     = employmentVerificationsNotInContract.Any(i => i.IsVerified) || contract.Any(i => i.IsVerified == true);

            if (participant.Is60DaysVerified != is60DaysVerified)
            {
                participant.Is60DaysVerified = is60DaysVerified;
                participant.ModifiedBy       = _authUser.Username;
                participant.ModifiedDate     = modifiedDate;
            }

            employmentVerificationsToUpdate.AsNotNull().ForEach(i =>
                                                                {
                                                                    var employmentVerification = employmentVerificationsInContract.First(j => j.Id == i.EmploymentVerificationId);

                                                                    employmentVerification.IsVerified                 = i.IsVerified.GetValueOrDefault();
                                                                    employmentVerification.NumberOfDaysAtVerification = i.IsVerified == true ? i.NumberOfDaysAtVerification : null;
                                                                    employmentVerification.ModifiedDate               = modifiedDate;
                                                                    employmentVerification.ModifiedBy                 = modifiedBy;

                                                                    _employmentVerificationRepository.Update(employmentVerification);
                                                                });

            employmentVerificationsToAdd.AsNotNull().ForEach(i =>
                                                             {
                                                                 var employmentVerification = _employmentVerificationRepository.New();

                                                                 employmentVerification.EmploymentInformationId    = i.Id;
                                                                 employmentVerification.IsVerified                 = i.IsVerified.GetValueOrDefault();
                                                                 employmentVerification.NumberOfDaysAtVerification = i.IsVerified == true ? i.NumberOfDaysAtVerification : null;
                                                                 employmentVerification.ModifiedDate               = modifiedDate;
                                                                 employmentVerification.ModifiedBy                 = modifiedBy;
                                                                 employmentVerification.CreatedDate                = modifiedDate;

                                                                 _employmentVerificationRepository.Add(employmentVerification);
                                                             });

            _unitOfWork.Commit();
        }

        #endregion
    }
}
