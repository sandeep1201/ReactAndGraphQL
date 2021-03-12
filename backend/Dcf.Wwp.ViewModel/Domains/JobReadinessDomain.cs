using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using ActionNeededPage = Dcf.Wwp.Model.Interface.Constants.ActionNeededPage;
using IParticipantRepository = Dcf.Wwp.DataAccess.Interfaces.IParticipantRepository;
using IWorkerRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerRepository;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class JobReadinessDomain : IJobReadinessDomain
    {
        #region Properties

        private readonly IParticipantRepository                 _participantRepository;
        private readonly IJobReadinessRepository                _jobReadinessRepository;
        private readonly IJRApplicationInfoRepository           _jrApplicationInfoRepository;
        private readonly IJRContactInfoRepository               _jrContactInfoRepository;
        private readonly IJRHistoryInfoRepository               _jrHistoryInfoRepository;
        private readonly IJRInterviewInfoRepository             _jrInterviewInfoRepository;
        private readonly IJRWorkPreferencesRepository           _jrWorkPreferencesRepository;
        private readonly IJRWorkPreferenceShiftBridgeRepository _jrWorkPreferenceShiftBridgeRepository;
        private readonly IUnitOfWork                            _unitOfWork;
        private readonly IAuthUser                              _authUser;
        private readonly IWorkerRepository                      _workerRepository;
        private readonly IActionNeededDomain                    _actionNeededDomain;
        private readonly ITransactionDomain                     _transactionDomain;
        private readonly Func<string, string>                   _convertWIUIdToName;

        #endregion

        #region Methods

        public JobReadinessDomain(IParticipantRepository                 participantRepository,
                                  IJobReadinessRepository                jobReadinessRepository,
                                  IJRApplicationInfoRepository           jrApplicationInfoRepository,
                                  IJRContactInfoRepository               jrContactInfoRepository,
                                  IJRHistoryInfoRepository               jrHistoryInfoRepository,
                                  IJRInterviewInfoRepository             jrInterviewInfoRepository,
                                  IJRWorkPreferencesRepository           jrWorkPreferencesRepository,
                                  IJRWorkPreferenceShiftBridgeRepository jrWorkPreferenceShiftBridgeRepository,
                                  IUnitOfWork                            unitOfWork,
                                  IAuthUser                              authUser,
                                  IWorkerRepository                      workerRepo,
                                  IActionNeededDomain                    actionNeededDomain,
                                  ITransactionDomain                     transactionDomain)
        {
            _participantRepository                 = participantRepository;
            _jobReadinessRepository                = jobReadinessRepository;
            _jrApplicationInfoRepository           = jrApplicationInfoRepository;
            _jrContactInfoRepository               = jrContactInfoRepository;
            _jrHistoryInfoRepository               = jrHistoryInfoRepository;
            _jrInterviewInfoRepository             = jrInterviewInfoRepository;
            _jrWorkPreferencesRepository           = jrWorkPreferencesRepository;
            _jrWorkPreferenceShiftBridgeRepository = jrWorkPreferenceShiftBridgeRepository;
            _unitOfWork                            = unitOfWork;
            _authUser                              = authUser;
            _workerRepository                      = workerRepo;
            _actionNeededDomain                    = actionNeededDomain;
            _transactionDomain                     = transactionDomain;

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


        public JobReadinessContract GetJobReadinessForPin(decimal pin)
        {
            var jobReadinessContract = new JobReadinessContract
                                       {
                                           JrApplicationInfo = new JRApplicationInfoContract(),
                                           JrContactInfo     = new JRContactInfoContract(),
                                           JrHistoryInfo     = new JRHistoryInfoContract(),
                                           JrInterviewInfo   = new JRInterviewInfoContract(),
                                           JrWorkPreferences = new JRWorkPreferencesContract()
                                       };
            var participant  = _participantRepository.Get(i => i.PinNumber == pin);
            var jobReadiness = _jobReadinessRepository.Get(i => i.ParticipantId == participant.Id && i.IsDeleted == false);

            if (jobReadiness != null)
            {
                jobReadinessContract = new JobReadinessContract
                                       {
                                           Id = jobReadiness.Id,
                                           JrApplicationInfo = jobReadiness.JrApplicationInfos.Select(j => new JRApplicationInfoContract
                                                                                                           {
                                                                                                               Id                               = j.Id,
                                                                                                               CanSubmitOnline                  = j.CanSubmitOnline,
                                                                                                               CanSubmitOnlineDetails           = j.CanSubmitOnlineDetails,
                                                                                                               HaveCurrentResume                = j.HaveCurrentResume,
                                                                                                               HaveCurrentResumeDetails         = j.HaveCurrentResumeDetails,
                                                                                                               HaveProfessionalReference        = j.HaveProfessionalReference,
                                                                                                               HaveProfessionalReferenceDetails = j.HaveProfessionalReferenceDetails,
                                                                                                               NeedDocumentLookupId             = j.NeedDocumentLookupId,
                                                                                                               //NeedDocumentLookupName           = j.NeedDocumentLookupId != null ? j.NeedDocumentLookup.Name : null,
                                                                                                               NeedDocumentDetail = j.NeedDocumentDetail
                                                                                                           }).FirstOrDefault(),
                                           JrContactInfo = jobReadiness.JrContactInfos.Select(j => new JRContactInfoContract
                                                                                                   {
                                                                                                       Id                                  = j.Id,
                                                                                                       CanYourPhoneNumberUsed              = j.CanYourPhoneNumberUsed,
                                                                                                       PhoneNumberDetails                  = j.PhoneNumberDetails,
                                                                                                       HaveAccessToVoiceMailOrTextMessages = j.HaveAccessToVoiceMailOrTextMessages,
                                                                                                       VoiceOrTextMessageDetails           = j.VoiceOrTextMessageDetails,
                                                                                                       HaveEmailAddress                    = j.HaveEmailAddress,
                                                                                                       EmailAddressDetails                 = j.EmailAddressDetails,
                                                                                                       HaveAccessDailyToEmail              = j.HaveAccessDailyToEmail,
                                                                                                       AccessEmailDailyDetails             = j.AccessEmailDailyDetails
                                                                                                   }).FirstOrDefault(),
                                           JrHistoryInfo = jobReadiness.JrHistoryInfos.Select(j => new JRHistoryInfoContract
                                                                                                   {
                                                                                                       Id                    = j.Id,
                                                                                                       LastJobDetails        = j.LastJobDetails,
                                                                                                       AccomplishmentDetails = j.AccomplishmentDetails,
                                                                                                       StrengthDetails       = j.StrengthDetails,
                                                                                                       AreasNeedImprove      = j.AreasNeedImprove
                                                                                                   }).FirstOrDefault(),
                                           JrInterviewInfo = jobReadiness.JrInterviewInfos.Select(j => new JRInterviewInfoContract
                                                                                                       {
                                                                                                           Id                          = j.Id,
                                                                                                           LastInterviewDetails        = j.LastInterviewDetails,
                                                                                                           CanLookAtSocialMedia        = j.CanLookAtSocialMedia,
                                                                                                           CanLookAtSocialMediaDetails = j.CanLookAtSocialMediaDetails,
                                                                                                           HaveOutfit                  = j.HaveOutfit,
                                                                                                           HaveOutfitDetails           = j.HaveOutfitDetails
                                                                                                       }).FirstOrDefault(),
                                           JrWorkPreferences = jobReadiness.JrWorkPreferenceses.Select(j => new JRWorkPreferencesContract
                                                                                                            {
                                                                                                                Id                                 = j.Id,
                                                                                                                KindOfJobDetails                   = j.KindOfJobDetails,
                                                                                                                JobInterestDetails                 = j.JobInterestDetails,
                                                                                                                TrainingNeededForJobDetails        = j.TrainingNeededForJobDetails,
                                                                                                                SomeOtherPlacesJobAvailableDetails = j.SomeOtherPlacesJobAvailableDetails,
                                                                                                                SomeOtherPlacesJobAvailableUnknown = j.SomeOtherPlacesJobAvailableUnknown,
                                                                                                                SituationsToAvoidDetails           = j.SituationsToAvoidDetails,
                                                                                                                BeginHour                          = GetHour(j.WorkScheduleBeginTime),
                                                                                                                BeginMinute                        = GetMinute(j.WorkScheduleBeginTime),
                                                                                                                BeginAmPm                          = GetAmPm(j.WorkScheduleBeginTime),
                                                                                                                EndHour                            = GetHour(j.WorkScheduleEndTime),
                                                                                                                EndMinute                          = GetMinute(j.WorkScheduleEndTime),
                                                                                                                EndAmPm                            = GetAmPm(j.WorkScheduleEndTime),
                                                                                                                WorkScheduleDetails                = j.WorkScheduleDetails,
                                                                                                                TravelTimeToWork                   = j.TravelTimeToWork,
                                                                                                                DistanceHomeToWork                 = j.DistanceHomeToWork,
                                                                                                                WorkShiftIds                       = j.JrWorkPreferenceShiftBridges.Select(k => k.WorkShiftId).ToList(),
                                                                                                                WorkShiftNames                     = j.JrWorkPreferenceShiftBridges.Select(k => k.JrWorkShift?.Name).ToList()
                                                                                                            }).FirstOrDefault(),
                                           CreatedDate  = jobReadiness.CreatedDate,
                                           ModifiedBy   = _convertWIUIdToName(jobReadiness.ModifiedBy),
                                           ModifiedDate = jobReadiness.ModifiedDate
                                       };
            }

            jobReadinessContract.ActionNeeded = _actionNeededDomain.GetActionNeededContract(participant, ActionNeededPage.JobReadiness);

            return jobReadinessContract;
        }

        public JobReadinessContract UpsertJobReadiness(JobReadinessContract jobReadinessContract, string pin, int id, bool hasSaveErrors)
        {
            if (jobReadinessContract == null)
            {
                throw new ArgumentNullException(nameof(jobReadinessContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var decimalPin   = decimal.Parse(pin);
            var participant  = _participantRepository.Get(i => i.PinNumber == decimalPin);
            var jobReadiness = jobReadinessContract.Id != 0 ? _jobReadinessRepository.Get(i => i.Id == id && i.IsDeleted == false) : _jobReadinessRepository.New();

            if (jobReadinessContract.Id == 0) jobReadiness.CreatedDate = DateTime.Now;

            jobReadiness.IsDeleted    = false;
            jobReadiness.ModifiedBy   = modifiedBy;
            jobReadiness.ModifiedDate = modifiedDate;

            var jrApplicationInfo = jobReadinessContract.Id != 0 ? jobReadiness.JrApplicationInfos.FirstOrDefault() : _jrApplicationInfoRepository.New();
            var jrContactInfo     = jobReadinessContract.Id != 0 ? jobReadiness.JrContactInfos.FirstOrDefault() : _jrContactInfoRepository.New();
            var jrHistoryInfo     = jobReadinessContract.Id != 0 ? jobReadiness.JrHistoryInfos.FirstOrDefault() : _jrHistoryInfoRepository.New();
            var jrInterviewInfo   = jobReadinessContract.Id != 0 ? jobReadiness.JrInterviewInfos.FirstOrDefault() : _jrInterviewInfoRepository.New();
            var jrWorkPreferences = jobReadinessContract.Id != 0 ? jobReadiness.JrWorkPreferenceses.FirstOrDefault() : _jrWorkPreferencesRepository.New();

            jobReadiness.Participant = participant;

            if (jrApplicationInfo != null)
            {
                jrApplicationInfo.JobReadiness                     = jobReadiness;
                jrApplicationInfo.CanSubmitOnline                  = jobReadinessContract.JrApplicationInfo.CanSubmitOnline;
                jrApplicationInfo.CanSubmitOnlineDetails           = jobReadinessContract.JrApplicationInfo.CanSubmitOnlineDetails;
                jrApplicationInfo.HaveCurrentResume                = jobReadinessContract.JrApplicationInfo.HaveCurrentResume;
                jrApplicationInfo.HaveCurrentResumeDetails         = jobReadinessContract.JrApplicationInfo.HaveCurrentResumeDetails;
                jrApplicationInfo.HaveProfessionalReference        = jobReadinessContract.JrApplicationInfo.HaveProfessionalReference;
                jrApplicationInfo.HaveProfessionalReferenceDetails = jobReadinessContract.JrApplicationInfo.HaveProfessionalReferenceDetails;
                jrApplicationInfo.NeedDocumentLookupId             = jobReadinessContract.JrApplicationInfo.NeedDocumentLookupId;
                jrApplicationInfo.NeedDocumentDetail               = jobReadinessContract.JrApplicationInfo.NeedDocumentDetail;
                jrApplicationInfo.ModifiedBy                       = modifiedBy;
                jrApplicationInfo.ModifiedDate                     = modifiedDate;
            }

            if (jrContactInfo != null)
            {
                jrContactInfo.JobReadiness                        = jobReadiness;
                jrContactInfo.CanYourPhoneNumberUsed              = jobReadinessContract.JrContactInfo.CanYourPhoneNumberUsed;
                jrContactInfo.PhoneNumberDetails                  = jobReadinessContract.JrContactInfo.PhoneNumberDetails;
                jrContactInfo.HaveAccessToVoiceMailOrTextMessages = jobReadinessContract.JrContactInfo.HaveAccessToVoiceMailOrTextMessages;
                jrContactInfo.VoiceOrTextMessageDetails           = jobReadinessContract.JrContactInfo.VoiceOrTextMessageDetails;
                jrContactInfo.HaveEmailAddress                    = jobReadinessContract.JrContactInfo.HaveEmailAddress;
                jrContactInfo.EmailAddressDetails                 = jobReadinessContract.JrContactInfo.EmailAddressDetails;
                jrContactInfo.HaveAccessDailyToEmail              = jobReadinessContract.JrContactInfo.HaveAccessDailyToEmail;
                jrContactInfo.AccessEmailDailyDetails             = jobReadinessContract.JrContactInfo.AccessEmailDailyDetails;
                jrContactInfo.ModifiedBy                          = modifiedBy;
                jrContactInfo.ModifiedDate                        = modifiedDate;
            }

            if (jrHistoryInfo != null)
            {
                jrHistoryInfo.JobReadiness          = jobReadiness;
                jrHistoryInfo.LastJobDetails        = jobReadinessContract.JrHistoryInfo.LastJobDetails;
                jrHistoryInfo.AccomplishmentDetails = jobReadinessContract.JrHistoryInfo.AccomplishmentDetails;
                jrHistoryInfo.StrengthDetails       = jobReadinessContract.JrHistoryInfo.StrengthDetails;
                jrHistoryInfo.AreasNeedImprove      = jobReadinessContract.JrHistoryInfo.AreasNeedImprove;
                jrHistoryInfo.ModifiedBy            = modifiedBy;
                jrHistoryInfo.ModifiedDate          = modifiedDate;
            }

            if (jrInterviewInfo != null)
            {
                jrInterviewInfo.JobReadiness                = jobReadiness;
                jrInterviewInfo.LastInterviewDetails        = jobReadinessContract.JrInterviewInfo.LastInterviewDetails;
                jrInterviewInfo.CanLookAtSocialMedia        = jobReadinessContract.JrInterviewInfo.CanLookAtSocialMedia;
                jrInterviewInfo.CanLookAtSocialMediaDetails = jobReadinessContract.JrInterviewInfo.CanLookAtSocialMediaDetails;
                jrInterviewInfo.HaveOutfit                  = jobReadinessContract.JrInterviewInfo.HaveOutfit;
                jrInterviewInfo.HaveOutfitDetails           = jobReadinessContract.JrInterviewInfo.HaveOutfitDetails;
                jrInterviewInfo.ModifiedBy                  = modifiedBy;
                jrInterviewInfo.ModifiedDate                = modifiedDate;
            }

            if (jrWorkPreferences != null)
            {
                TimeSpan? beginTime = null;
                if (jobReadinessContract.JrWorkPreferences.BeginHour != null)
                {
                    var beginAmPm   = jobReadinessContract.JrWorkPreferences.BeginAmPm   != null ? jobReadinessContract.JrWorkPreferences.BeginAmPm == 1 ? "AM" : "PM" : "AM";
                    var beginMinute = jobReadinessContract.JrWorkPreferences.BeginMinute != null ? jobReadinessContract.JrWorkPreferences.BeginMinute.ToString() : "00";

                    beginTime = GetHourMinute(jobReadinessContract.JrWorkPreferences.BeginHour.ToString(), beginMinute, beginAmPm);
                }

                TimeSpan? endTime = null;
                if (jobReadinessContract.JrWorkPreferences.EndHour != null)
                {
                    var endAmPm   = jobReadinessContract.JrWorkPreferences.EndAmPm   != null ? jobReadinessContract.JrWorkPreferences.EndAmPm == 1 ? "AM" : "PM" : "AM";
                    var endMinute = jobReadinessContract.JrWorkPreferences.EndMinute != null ? jobReadinessContract.JrWorkPreferences.EndMinute.ToString() : "00";

                    endTime = GetHourMinute(jobReadinessContract.JrWorkPreferences.EndHour.ToString(), endMinute, endAmPm);
                }

                jrWorkPreferences.JobReadiness                       = jobReadiness;
                jrWorkPreferences.KindOfJobDetails                   = jobReadinessContract.JrWorkPreferences.KindOfJobDetails;
                jrWorkPreferences.JobInterestDetails                 = jobReadinessContract.JrWorkPreferences.JobInterestDetails;
                jrWorkPreferences.TrainingNeededForJobDetails        = jobReadinessContract.JrWorkPreferences.TrainingNeededForJobDetails;
                jrWorkPreferences.SomeOtherPlacesJobAvailableDetails = jobReadinessContract.JrWorkPreferences.SomeOtherPlacesJobAvailableDetails;
                jrWorkPreferences.SomeOtherPlacesJobAvailableUnknown = jobReadinessContract.JrWorkPreferences.SomeOtherPlacesJobAvailableUnknown;
                jrWorkPreferences.SituationsToAvoidDetails           = jobReadinessContract.JrWorkPreferences.SituationsToAvoidDetails;
                jrWorkPreferences.WorkScheduleBeginTime              = beginTime;
                jrWorkPreferences.WorkScheduleEndTime                = endTime;
                jrWorkPreferences.WorkScheduleDetails                = jobReadinessContract.JrWorkPreferences.WorkScheduleDetails;
                jrWorkPreferences.TravelTimeToWork                   = jobReadinessContract.JrWorkPreferences.TravelTimeToWork;
                jrWorkPreferences.DistanceHomeToWork                 = jobReadinessContract.JrWorkPreferences.DistanceHomeToWork;
                jrWorkPreferences.ModifiedBy                         = modifiedBy;
                jrWorkPreferences.ModifiedDate                       = modifiedDate;

                var allIds        = jrWorkPreferences.JrWorkPreferenceShiftBridges?.Select(i => i.WorkShiftId).ToList();
                var contractIds   = jobReadinessContract.JrWorkPreferences.WorkShiftIds;
                var idsToDelete   = allIds?.Except(contractIds.AsNotNull()).ToList();
                var idsToUpdate   = allIds?.Where(i => contractIds.Contains(i)).ToList();
                var idsToAdd      = allIds      != null ? contractIds.Except(allIds).ToList() : contractIds;
                var typesToDelete = idsToDelete != null ? jrWorkPreferences.JrWorkPreferenceShiftBridges?.Where(i => idsToDelete.Contains(i.WorkShiftId)).Select(i => i).ToList() : null;
                var typesToUpdate = idsToUpdate != null ? jrWorkPreferences.JrWorkPreferenceShiftBridges?.Where(i => idsToUpdate.Contains(i.WorkShiftId)).Select(i => i).ToList() : null;

                typesToDelete?.ForEach(type => _jrWorkPreferenceShiftBridgeRepository.Delete(type));

                typesToUpdate.AsNotNull()
                             .Select(typeFromContract => jrWorkPreferences.JrWorkPreferenceShiftBridges
                                                                          .FirstOrDefault(i => i.WorkShiftId == typeFromContract.WorkShiftId))
                             .Where(type => type != null)
                             .ForEach(type =>
                                      {
                                          type.ModifiedBy   = modifiedBy;
                                          type.ModifiedDate = modifiedDate;
                                      });

                idsToAdd.AsNotNull().Select(typeFromContract => new JRWorkPreferenceShiftBridge
                                                                {
                                                                    JrWorkPreferences = jrWorkPreferences,
                                                                    WorkShiftId       = typeFromContract,
                                                                    IsDeleted         = false,
                                                                    ModifiedBy        = modifiedBy,
                                                                    ModifiedDate      = modifiedDate
                                                                }).ForEach(type => jrWorkPreferences.JrWorkPreferenceShiftBridges.Add(type));
            }

            if (jobReadinessContract.Id != 0)
                _jobReadinessRepository.Update(jobReadiness);
            else
            {
                _jobReadinessRepository.Add(jobReadiness);
                _jrApplicationInfoRepository.Add(jrApplicationInfo);
                _jrContactInfoRepository.Add(jrContactInfo);
                _jrHistoryInfoRepository.Add(jrHistoryInfo);
                _jrInterviewInfoRepository.Add(jrInterviewInfo);
                _jrWorkPreferencesRepository.Add(jrWorkPreferences);
            }

            #region Transaction

            if (!hasSaveErrors)
            {
                var transactionContract = new TransactionContract
                                          {
                                              ParticipantId       = participant.Id,
                                              WorkerId            = _workerRepository.Get(x => x.WIUId == _authUser.WIUID).Id,
                                              OfficeId            = ParticipantHelper.GetMostRecentEnrolledProgram(participant, _authUser).Office.Id,
                                              EffectiveDate       = modifiedDate,
                                              CreatedDate         = modifiedDate,
                                              TransactionTypeCode = TransactionTypes.JobReadiness,
                                              ModifiedBy          = modifiedBy
                                          };

                _transactionDomain.InsertTransaction(transactionContract);
            }

            #endregion

            _unitOfWork.Commit();
            var contract = GetJobReadinessForPin(decimal.Parse(pin));

            return contract;
        }

        private int? GetHour(TimeSpan? time)
        {
            int? hr = null;

            if (time.HasValue)
            {
                hr = int.Parse(DateTime.Today.Add((TimeSpan) time).ToString("hh"));
            }


            return hr;
        }

        private int? GetMinute(TimeSpan? time)
        {
            int? mm = null;

            if (time.HasValue)
                mm = int.Parse(DateTime.Today.Add((TimeSpan) time).ToString("mm"));

            return mm;
        }

        private int? GetAmPm(TimeSpan? time)
        {
            string ampm = null;

            if (time.HasValue)
                ampm = DateTime.Today.Add((TimeSpan) time).ToString("tt");

            if (ampm != null)
                return ampm.ToLower() == "am" ? 1 : 2;
            return null;
        }

        private TimeSpan GetHourMinute(string hr, string mm, string tt)
        {
            var dateTime = DateTime.Parse(hr + ":" + mm + " " + tt);
            var time     = TimeSpan.Parse(dateTime.ToString("HH:mm"));

            return time;
        }

        #endregion
    }
}
