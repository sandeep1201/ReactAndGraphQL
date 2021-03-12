using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Api.AutoMapper;
using Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class EmploymentVerificationDomainTest
    {
        #region Properties

        private       EmploymentVerificationDomain         _employmentVerificationDomain;
        private       IAuthUser                            _authUser;
        private       IMapper                              _mockMapper;
        private       MapperConfiguration                  _config;
        private       MapperBaseTest                       _mapperBaseTest;
        private       MockEmploymentInformationRepository  _employmentInformationRepository;
        private       MockEmploymentVerificationRepository _employmentVerificationRepository;
        private       MockParticipantRepository            _participantRepository;
        private       MockUnitOfWork                       _mockUnitOfWork;
        private const string                               Pin = "1234567890";
        private       List<EmploymentVerificationContract> _employmentVerificationContracts;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _employmentInformationRepository  = new MockEmploymentInformationRepository();
            _employmentVerificationRepository = new MockEmploymentVerificationRepository();
            _participantRepository            = new MockParticipantRepository();
            _mockUnitOfWork                   = new MockUnitOfWork();
            _authUser                         = new AuthUser { WIUID = "1111", AgencyCode = "FSC", Username = "WWP" };
            _mapperBaseTest                   = new MapperBaseTest();
            _config                           = new MapperConfiguration(cfg => cfg.AddProfile<EmploymentVerificationProfile>());
            _mockMapper                       = _config.CreateMapper();
            _employmentVerificationContracts  = new List<EmploymentVerificationContract>
                                                {
                                                    new EmploymentVerificationContract
                                                    {
                                                        Id            = 0,
                                                        ParticipantId = 123
                                                    }
                                                };
            _employmentVerificationDomain = new EmploymentVerificationDomain(_mockUnitOfWork, _authUser, _mockMapper, _employmentInformationRepository, _employmentVerificationRepository, _participantRepository, new MockWIUIDToFullNameModifiedByResolver());
        }

        [TestMethod]
        public void EmploymentVerificationProfileTestMappingEmptyEntityToContractForValueEquality()
        {
            var entity   = new EmploymentVerificationContract();
            var contract = _mockMapper.Map<EmploymentVerificationContract>(entity);

            _mapperBaseTest.AssertAllPropertiesMapped(entity, contract);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_EnrollmentInLaterDate_willNotReturnsEmployments()
        {
            Assert.AreEqual(0, _employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 7, DateTime.Now.AddDays(-5)).Count);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_ValidParticipantIdAndJobType_ReturnsEmployments()
        {
            Assert.IsNotNull(_employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 9, DateTime.Now.AddDays(-100)));
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_ValidParticipantIdAndJobType_GetsEmploymentInformation()
        {
            _employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 9, DateTime.Now.AddDays(-100));

            Assert.IsTrue(_employmentInformationRepository.GetManyHasBeenCalled);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_ValidParticipantIdAndJobType_GetsEmploymentVerifications()
        {
            _employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 9, DateTime.Now.AddDays(-100));

            Assert.IsTrue(_employmentVerificationRepository.GetManyHasBeenCalled);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_ValidParticipantIdAndJobType_ReturnsValidEmployments()
        {
            var employments = _employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 6, DateTime.Now.AddDays(-100));

            Assert.AreEqual(employments[0].Id,                                   _employmentInformationRepository.EmploymentInformation[1].Id);
            Assert.AreEqual(employments[0].JobTypeId,                            _employmentInformationRepository.EmploymentInformation[1].JobTypeId);
            Assert.AreEqual(employments[0].JobTypeName,                          _employmentInformationRepository.EmploymentInformation[1].JobType.Name);
            Assert.AreEqual(employments[0].JobBeginDate?.ToString("MM/dd/yyyy"), _employmentInformationRepository.EmploymentInformation[1].JobBeginDate?.ToString("MM/dd/yyyy"));
            Assert.AreEqual(employments[0].JobEndDate,                           _employmentInformationRepository.EmploymentInformation[1].JobEndDate?.ToString("MM/dd/yyyy"));
            Assert.AreEqual(employments[0].AvgWeeklyHours,                       _employmentInformationRepository.EmploymentInformation[1].WageHour.CurrentAverageWeeklyHours);
            Assert.AreEqual(employments[0].CompanyName,                          _employmentInformationRepository.EmploymentInformation[1].CompanyName);
            Assert.AreEqual(employments[0].JobPosition,                          _employmentInformationRepository.EmploymentInformation[1].JobPosition);
            Assert.AreEqual(employments[0].Location.FullAddress,                 _employmentInformationRepository.EmploymentInformation[1].StreetAddress);
            Assert.AreEqual(employments[0].Location.City,                        _employmentInformationRepository.EmploymentInformation[1].City?.Name);
            Assert.AreEqual(employments[0].Location.State,                       _employmentInformationRepository.EmploymentInformation[1].City?.State.Name);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_NoEmploymentVerification_ReturnsEmploymentWithNoEmploymentVerification()
        {
            var employments = _employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 7, DateTime.Now.AddDays(-100));

            Assert.AreEqual(employments[0].Id, _employmentInformationRepository.EmploymentInformation[2].Id);
            Assert.IsNull(employments[0].IsVerified);
            Assert.IsNull(employments[0].ModifiedBy);
            Assert.IsNull(employments[0].ModifiedDate);
            Assert.IsNull(employments[0].EmploymentVerificationId);
            Assert.IsNull(employments[0].NumberOfDaysAtVerification);
        }

        [TestMethod]
        public void GetTJTMJEmploymentsForParticipantByJobType_EmploymentVerification_ReturnsEmploymentWithEmploymentVerification()
        {
            var employments = _employmentVerificationDomain.GetTJTMJEmploymentsForParticipantByJobType(11196, 5, DateTime.Now.AddDays(-100));

            Assert.AreEqual(employments[0].Id,                         _employmentInformationRepository.EmploymentInformation[0].Id);
            Assert.AreEqual(employments[0].IsVerified,                 _employmentVerificationRepository.EmploymentVerifications[0].IsVerified);
            Assert.AreEqual(employments[0].ModifiedDate,               _employmentVerificationRepository.EmploymentVerifications[0].ModifiedDate);
            Assert.AreEqual(employments[0].EmploymentVerificationId,   _employmentVerificationRepository.EmploymentVerifications[0].Id);
            Assert.AreEqual(employments[0].NumberOfDaysAtVerification, _employmentVerificationRepository.EmploymentVerifications[0].NumberOfDaysAtVerification);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_ValidEmploymentContract_CallsCommit()
        {
            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);
            Assert.AreEqual(1, _mockUnitOfWork.CommitCalled);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_ValidEmploymentContract_GetsExistingEmploymentsVerifications()
        {
            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);
            Assert.IsTrue(_employmentVerificationRepository.GetManyHasBeenCalled);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_InvalidEmploymentContract_DoNotAddEmploymentVerification()
        {
            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);
            Assert.IsFalse(_employmentVerificationRepository.AddHasBeenCalled);

            // EmploymentVerificationId null and IsVerified null
            var employmentVerifications = new List<EmploymentVerificationContract>
                                          {
                                              new EmploymentVerificationContract
                                              {
                                                  Id = 1000
                                              },
                                              new EmploymentVerificationContract
                                              {
                                                  Id = 1001
                                              }
                                          };
            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);
            Assert.IsFalse(_employmentVerificationRepository.AddHasBeenCalled);

            // EmploymentVerificationId is 0 but IsVerified null
            employmentVerifications[0].EmploymentVerificationId = 0;
            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);
            Assert.IsFalse(_employmentVerificationRepository.AddHasBeenCalled);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_ValidEmploymentContract_AddsNewEmploymentVerification()
        {
            var employmentVerifications = new List<EmploymentVerificationContract>
                                          {
                                              new EmploymentVerificationContract
                                              {
                                                  EmploymentVerificationId = 0,
                                                  IsVerified               = true
                                              },
                                              new EmploymentVerificationContract
                                              {
                                                  EmploymentVerificationId = null,
                                                  IsVerified               = false
                                              }
                                          };

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);
            Assert.AreEqual(2, _employmentVerificationRepository.AddCount);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_ValidEmploymentContract_AddsValidEmploymentVerification()
        {
            var employmentVerifications = new List<EmploymentVerificationContract>
                                          {
                                              new EmploymentVerificationContract
                                              {
                                                  EmploymentVerificationId = 0,
                                                  IsVerified               = true
                                              }
                                          };

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);
            Assert.AreEqual(employmentVerifications[0].IsVerified,                 _employmentVerificationRepository.AddedValues.IsVerified);
            Assert.AreEqual(employmentVerifications[0].NumberOfDaysAtVerification, _employmentVerificationRepository.AddedValues.NumberOfDaysAtVerification);
            Assert.AreEqual(DateTime.Now.ToStringMonthDayYear(),                   _employmentVerificationRepository.AddedValues.ModifiedDate.ToStringMonthDayYear());
            Assert.AreEqual("1111",                                                _employmentVerificationRepository.AddedValues.ModifiedBy);
            Assert.IsTrue(_employmentVerificationRepository.AddHasBeenCalled);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_VerifiedEmployment_AddsNumberOfDaysAtVerification()
        {
            var employmentVerifications = new List<EmploymentVerificationContract>
                                          {
                                              new EmploymentVerificationContract
                                              {
                                                  EmploymentVerificationId   = 0,
                                                  IsVerified                 = true,
                                                  NumberOfDaysAtVerification = 10
                                              }
                                          };

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);
            Assert.AreEqual(employmentVerifications[0].NumberOfDaysAtVerification, _employmentVerificationRepository.AddedValues.NumberOfDaysAtVerification);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_VerifiedEmployment_AddsNumberOfDaysAtVerificationAsNull()
        {
            var employmentVerifications = new List<EmploymentVerificationContract>
                                          {
                                              new EmploymentVerificationContract
                                              {
                                                  EmploymentVerificationId = 0,
                                                  IsVerified               = false
                                              }
                                          };

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);
            Assert.IsNull(employmentVerifications[0].NumberOfDaysAtVerification);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_CallsParticipantRepositoryGet()
        {
            var employmentVerifications = new List<EmploymentVerificationContract>
                                          {
                                              new EmploymentVerificationContract
                                              {
                                                  EmploymentVerificationId = 0,
                                                  IsVerified               = true
                                              }
                                          };

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, employmentVerifications);

            Assert.IsTrue(_participantRepository.GetHasBeenCalled);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void UpsertEmploymentVerification_MarksParticipantIs60DayVerifiedSameAsContract(bool isVerified)
        {
            _employmentVerificationContracts[0].IsVerified = isVerified;
            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);

            Assert.IsTrue(_participantRepository.Participant.Is60DaysVerified == isVerified);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_WithVerifiedFalseAndTrueContract_MarksParticipantIs60DayVerifiedTrue()
        {
            _employmentVerificationContracts[0].IsVerified = false;
            _employmentVerificationContracts.Add(new EmploymentVerificationContract
                                                 {
                                                     EmploymentVerificationId = 0,
                                                     IsVerified               = true
                                                 });

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);

            Assert.IsTrue(_participantRepository.Participant.Is60DaysVerified == true);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_WhenVerifiedMatchesParticipantVerified_DoesNotUpdateParticipant()
        {
            _employmentVerificationContracts[0].IsVerified = false;

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);

            Assert.AreNotEqual(_participantRepository.Participant.ModifiedBy, _authUser.Username);
        }

        [TestMethod]
        public void UpsertEmploymentVerification_WhenVerifiedDoNotMatchParticipantVerified_UpdateParticipant()
        {
            _employmentVerificationContracts[0].IsVerified = true;

            _employmentVerificationDomain.UpsertEmploymentVerification(Pin, _employmentVerificationContracts);

            Assert.AreEqual(_participantRepository.Participant.ModifiedBy, _authUser.Username);
        }

        #endregion

        private class MockEmploymentInformationRepository : MockRepositoryBase<EmploymentInformation>, IEmploymentInformationRepository
        {
            public bool GetManyHasBeenCalled;

            public readonly List<EmploymentInformation> EmploymentInformation = new List<EmploymentInformation>
                                                                                {
                                                                                    new EmploymentInformation
                                                                                    {
                                                                                        Id            = 10,
                                                                                        ParticipantId = 11196,
                                                                                        JobTypeId     = 5,
                                                                                        JobBeginDate  = DateTime.Today.AddDays(-5),
                                                                                        JobType       = new JobType(),
                                                                                        WageHour      = new WageHour()
                                                                                    },
                                                                                    new EmploymentInformation
                                                                                    {
                                                                                        Id            = 13,
                                                                                        ParticipantId = 11196,
                                                                                        JobTypeId     = 6,
                                                                                        JobBeginDate  = DateTime.Today.AddDays(-10),
                                                                                        JobType       = new JobType(),
                                                                                        WageHour      = new WageHour()
                                                                                    },
                                                                                    new EmploymentInformation
                                                                                    {
                                                                                        Id            = 20,
                                                                                        ParticipantId = 11196,
                                                                                        JobTypeId     = 7,
                                                                                        JobBeginDate  = DateTime.Today.AddDays(-15),
                                                                                        JobType       = new JobType(),
                                                                                        WageHour      = new WageHour()
                                                                                    }
                                                                                };

            public new IEnumerable<EmploymentInformation> GetMany(Expression<Func<EmploymentInformation, bool>> clause)
            {
                GetManyHasBeenCalled = true;
                return EmploymentInformation.Where(clause.Compile()).Select(x => x).ToList();
            }
        }

        private class MockEmploymentVerificationRepository : MockRepositoryBase<EmploymentVerification>, IEmploymentVerificationRepository
        {
            public bool GetManyHasBeenCalled;

            public readonly List<EmploymentVerification> EmploymentVerifications = new List<EmploymentVerification>
                                                                                   {
                                                                                       new EmploymentVerification
                                                                                       {
                                                                                           Id                      = 1000,
                                                                                           EmploymentInformationId = 10,
                                                                                           EmploymentInformation   = new EmploymentInformation
                                                                                                                     {
                                                                                                                         Id            = 1,
                                                                                                                         ParticipantId = 123
                                                                                                                     }
                                                                                       },
                                                                                       new EmploymentVerification
                                                                                       {
                                                                                           Id                      = 1001,
                                                                                           EmploymentInformationId = 13,
                                                                                           EmploymentInformation   = new EmploymentInformation
                                                                                                                     {
                                                                                                                         Id            = 2,
                                                                                                                         ParticipantId = 123
                                                                                                                     }
                                                                                       },
                                                                                       new EmploymentVerification
                                                                                       {
                                                                                           Id                      = 1002,
                                                                                           EmploymentInformationId = 14,
                                                                                           EmploymentInformation   = new EmploymentInformation
                                                                                                                     {
                                                                                                                         Id            = 3,
                                                                                                                         ParticipantId = 123
                                                                                                                     }
                                                                                       },
                                                                                       new EmploymentVerification
                                                                                       {
                                                                                           Id                      = 1003,
                                                                                           EmploymentInformationId = 15,
                                                                                           EmploymentInformation   = new EmploymentInformation
                                                                                                                     {
                                                                                                                         Id            = 4,
                                                                                                                         ParticipantId = 123
                                                                                                                     }
                                                                                       },
                                                                                       new EmploymentVerification
                                                                                       {
                                                                                           Id                      = 1004,
                                                                                           EmploymentInformationId = 16,
                                                                                           EmploymentInformation   = new EmploymentInformation
                                                                                                                     {
                                                                                                                         Id            = 5,
                                                                                                                         ParticipantId = 123
                                                                                                                     }
                                                                                       }
                                                                                   };

            public new IEnumerable<EmploymentVerification> GetMany(Expression<Func<EmploymentVerification, bool>> clause)
            {
                GetManyHasBeenCalled = true;
                return EmploymentVerifications.Where(clause.Compile()).Select(i => i);
            }
        }
    }
}
