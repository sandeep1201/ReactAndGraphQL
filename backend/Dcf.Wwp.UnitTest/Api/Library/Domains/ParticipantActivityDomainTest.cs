using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using ParticipationStatus = Dcf.Wwp.DataAccess.Models.ParticipationStatus;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class ParticipantActivityDomainTest
    {
        #region Properties

        private       MockParticipantRepository                     _participantRepository;
        private       MockEmployabilityPlanRepository               _employabilityPlanRepository;
        private       MockEmployabilityPlanActivityBridgeRepository _epActivityBridgeRepository;
        private       MockParticipationStatusLocalRepository        _participationStatusRepository;
        private       ParticipantActivityDomain                     _participantActivityDomain;
        private const string                                        ValidParticipantPin         = "1234567890";
        private const string                                        InvalidParticipantPin       = "123456789";
        private const string                                        GetFrequencyNameForSchedule = "GetFrequencyNameForSchedule";

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _participantRepository         = new MockParticipantRepository();
            _employabilityPlanRepository   = new MockEmployabilityPlanRepository();
            _epActivityBridgeRepository    = new MockEmployabilityPlanActivityBridgeRepository();
            _participationStatusRepository = new MockParticipationStatusLocalRepository();
            _participantActivityDomain     = new ParticipantActivityDomain(_participantRepository, _employabilityPlanRepository, _epActivityBridgeRepository, _participationStatusRepository);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPins_ReturnActivityContract()
        {
            Assert.IsNotNull(_participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin));
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidTwoPins_CallsGetParticipantsTwice()
        {
            _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin + "," + ValidParticipantPin);
            Assert.AreEqual(2, _participantRepository.GetCount);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidInPin_ReturnsDataNotFound()
        {
            var contract = _participantActivityDomain.GetParticipantActivitiesByPins(InvalidParticipantPin);
            Assert.IsFalse(contract.Participants[0].IsDateFound);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPin_CallGetEpRepository()
        {
            _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin);
            Assert.IsTrue(_employabilityPlanRepository.GetHasBeenCalled);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_SubmittedEP_CallGetAsQueryableEpActivityRepository()
        {
            _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin);
            Assert.IsTrue(_epActivityBridgeRepository.GetAsQueryableHasBeenCalled);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_CallsGetManyParticipationStatusRepository()
        {
            _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin);
            Assert.IsTrue(_participationStatusRepository.GetManyHasBeenCalled);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPin_ReturnsPinWithValidValues()
        {
            var contract = _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin);

            Assert.AreEqual(ValidParticipantPin, contract.Participants[0].PinNumber.ToString());
            Assert.IsTrue(contract.Participants[0].Programs.Any());
            Assert.AreEqual(EnrolledProgram.W2ProgramCode,  contract.Participants[0].Programs[0].ProgramCode);
            Assert.AreEqual(EnrolledProgram.TjProgramCode,  contract.Participants[0].Programs[1].ProgramCode);
            Assert.AreEqual(EnrolledProgram.TmjProgramCode, contract.Participants[0].Programs[2].ProgramCode);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPin_ReturnsValidActivityValues()
        {
            var contract = _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin);

            Assert.AreEqual(_epActivityBridgeRepository.Activity.Activity.Description,                     contract.Participants[0].Programs[0].Activities[0].Description);
            Assert.AreEqual(_epActivityBridgeRepository.Activity.Activity.StartDate,                       contract.Participants[0].Programs[0].Activities[0].StartDate);
            Assert.AreEqual(_epActivityBridgeRepository.Activity.Activity.EndDate,                         contract.Participants[0].Programs[0].Activities[0].EndDate);
            Assert.AreEqual(MockEmployabilityPlanActivityBridgeRepository.ActivitySchedule.BeginTime,      contract.Participants[0].Programs[0].Activities[0].ActivitySchedules[0].BeginTime);
            Assert.AreEqual(MockEmployabilityPlanActivityBridgeRepository.ActivitySchedule.EndTime,        contract.Participants[0].Programs[0].Activities[0].ActivitySchedules[0].EndTime);
            Assert.AreEqual(MockEmployabilityPlanActivityBridgeRepository.ActivitySchedule.HoursPerDay,    contract.Participants[0].Programs[0].Activities[0].ActivitySchedules[0].HoursPerDay);
            Assert.AreEqual(MockEmployabilityPlanActivityBridgeRepository.ActivitySchedule.PlannedEndDate, contract.Participants[0].Programs[0].Activities[0].ActivitySchedules[0].PlannedEndDate);
        }

        [TestMethod]
        public void GetParticipantActivitiesByPins_ValidPin_ReturnsValidParticipationStatusValues()
        {
            var contract = _participantActivityDomain.GetParticipantActivitiesByPins(ValidParticipantPin);

            Assert.AreEqual(MockParticipationStatusLocalRepository.ParticipationStatus.BeginDate, contract.Participants[0].Programs[0].ParticipationStatuses[0].BeginDate);
            Assert.AreEqual(MockParticipationStatusLocalRepository.ParticipationStatus.EndDate,   contract.Participants[0].Programs[0].ParticipationStatuses[0].EndDate);
            Assert.AreEqual(MockParticipationStatusLocalRepository.ParticipationStatus.Details,   contract.Participants[0].Programs[0].ParticipationStatuses[0].Details);
        }

        [TestMethod]
        public void GetFrequencyNameForSchedule_NonRecurringSchedule_ReturnsEmptyString()
        {
            var schedule = new ActivitySchedule { IsRecurring = false };

            var privateObject     = new PrivateObject(new ParticipantActivityDomain(null, null, null, null));
            var frequencyTypeName = privateObject.Invoke(GetFrequencyNameForSchedule, schedule);

            Assert.AreEqual("", frequencyTypeName);
        }

        [TestMethod]
        public void GetFrequencyNameForSchedule_RecurringDailySchedule_ReturnsEmptyString()
        {
            var schedule = new ActivitySchedule { IsRecurring = true, FrequencyTypeId = 1, FrequencyType = new FrequencyType { Name = "TestFrequency" } };

            var privateObject     = new PrivateObject(new ParticipantActivityDomain(null, null, null, null));
            var frequencyTypeName = privateObject.Invoke(GetFrequencyNameForSchedule, schedule);

            Assert.AreEqual(schedule.FrequencyType.Name, frequencyTypeName);
        }

        [TestMethod]
        public void GetFrequencyNameForSchedule_RecurringMonthlySchedule_ReturnsExpectedString()
        {
            var frequencyBridge = new ActivityScheduleFrequencyBridge
                                  {
                                      MRFrequency = new Frequency
                                                    {
                                                        Name = "FirstMonth"
                                                    },
                                      WKFrequency = new Frequency
                                                    {
                                                        Name = "FirstWeek"
                                                    }
                                  };
            var schedule = new ActivitySchedule
                           {
                               IsRecurring                      = true,
                               FrequencyTypeId                  = 4,
                               FrequencyType                    = new FrequencyType { Name = ActivityScheduleFrequencyName.Monthly },
                               ActivityScheduleFrequencyBridges = new List<ActivityScheduleFrequencyBridge> { frequencyBridge }
                           };

            var privateObject     = new PrivateObject(new ParticipantActivityDomain(null, null, null, null));
            var frequencyTypeName = privateObject.Invoke(GetFrequencyNameForSchedule, schedule);

            Assert.AreEqual($"{schedule.FrequencyType.Name} - {frequencyBridge.MRFrequency.Name} - {frequencyBridge.WKFrequency.Name}", frequencyTypeName);
        }

        [TestMethod]
        public void GetFrequencyNameForSchedule_RecurringWeeklySchedule_ReturnsEmptyString()
        {
            var frequencyBridge  = new ActivityScheduleFrequencyBridge { WKFrequency = new Frequency { Name = "FirstWeek" } };
            var frequencyBridge2 = new ActivityScheduleFrequencyBridge { WKFrequency = new Frequency { Name = "SecondWeek" } };
            var schedule = new ActivitySchedule
                           {
                               IsRecurring                      = true,
                               FrequencyTypeId                  = 3,
                               FrequencyType                    = new FrequencyType { Name = ActivityScheduleFrequencyName.Biweekly },
                               ActivityScheduleFrequencyBridges = new List<ActivityScheduleFrequencyBridge> { frequencyBridge, frequencyBridge2 }
                           };

            var privateObject     = new PrivateObject(new ParticipantActivityDomain(null, null, null, null));
            var frequencyTypeName = privateObject.Invoke(GetFrequencyNameForSchedule, schedule);

            Assert.AreEqual($"{schedule.FrequencyType.Name} - {frequencyBridge.WKFrequency.Name}, {frequencyBridge2.WKFrequency.Name}", frequencyTypeName);
        }

        #endregion

        #region Mocks

        private class MockEmployabilityPlanActivityBridgeRepository : MockRepositoryBase<EmployabilityPlanActivityBridge>, IEmployabilityPlanActivityBridgeRepository
        {
            public static readonly ActivitySchedule ActivitySchedule = new ActivitySchedule
                                                                       {
                                                                           BeginTime      = new TimeSpan(),
                                                                           EndTime        = new TimeSpan(),
                                                                           HoursPerDay    = 10,
                                                                           PlannedEndDate = DateTime.Today
                                                                       };

            public readonly EmployabilityPlanActivityBridge Activity = new EmployabilityPlanActivityBridge
                                                                       {
                                                                           EmployabilityPlanId = 3,
                                                                           Activity = new Activity
                                                                                      {
                                                                                          Description = "TestDescription",
                                                                                          StartDate   = DateTime.Today,
                                                                                          EndDate     = DateTime.Today,
                                                                                          ActivityType = new ActivityType
                                                                                                         {
                                                                                                             Code = "TestCode",
                                                                                                             Name = ""
                                                                                                         },
                                                                                          ActivitySchedules = new List<ActivitySchedule> { ActivitySchedule }
                                                                                      }
                                                                       };

            public new IQueryable<EmployabilityPlanActivityBridge> GetAsQueryable(bool withTracking = true)
            {
                GetAsQueryableHasBeenCalled = true;
                return new List<EmployabilityPlanActivityBridge> { Activity }.AsQueryable();
            }
        }

        private class MockParticipationStatusLocalRepository : MockRepositoryBase<ParticipationStatus>, IParticipationStatusRepository
        {
            public static readonly ParticipationStatus ParticipationStatus = new ParticipationStatus { BeginDate = DateTime.Today, EndDate = DateTime.Today, Details = "TestDetails", Status = new ParticipationStatusType() };

            public new IEnumerable<ParticipationStatus> GetMany(Expression<Func<ParticipationStatus, bool>> clause)
            {
                GetManyHasBeenCalled = true;
                return new List<ParticipationStatus> { ParticipationStatus };
            }
        }

        #endregion
    }
}
