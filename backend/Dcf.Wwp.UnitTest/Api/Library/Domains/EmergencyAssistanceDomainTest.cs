using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.Api.Library.Domains.EmergencyAssistance;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Api.AutoMapper;
using Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver;
using Dcf.Wwp.UnitTest.Infrastructure;
using Dcf.Wwp.UnitTest.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class EmergencyAssistanceDomainTest
    {
        private EmergencyAssistanceDomain                _emergencyAssistanceDomain;
        private MockEARequestRepository                  _eaRequestRepository;
        private MockEARequestStatusRepository            _eaRequestStatusRepository;
        private MockEAStatusRepository                   _eaStatusRepository;
        private MockEARequestContactInfoRepository       _eaRequestContactInfoRepository;
        private MockOrganizationRepository               _organizationRepository;
        private MockEARequestParticipantBridgeRepository _eaRequestParticipantBridgeRepository;
        private MockEAIndividualTypeRepository           _eaIndividualTypeRepository;
        private MockEARelationshipTypeRepository         _eaRelationshipTypeRepository;
        private MockEAPaymentAmountRepository            _eaPaymentAmountRepository;
        private MockEligibilityByFPLRepository           _eligibilityByFplRepository;
        private MockEAIPVRepository                      _eaipvRepository;
        private MockWIUIDToFullNameModifiedByResolver    _WIUIDToFullNameModifiedByResolver;
        private MapperConfiguration                      _config;
        private IMapper                                  _mockMapper;
        private AuthUser                                 _authUser;
        private MockUnitOfWork                           _mockUnitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            _eaRequestRepository                  = new MockEARequestRepository();
            _eaRequestStatusRepository            = new MockEARequestStatusRepository();
            _eaStatusRepository                   = new MockEAStatusRepository();
            _eaRequestContactInfoRepository       = new MockEARequestContactInfoRepository();
            _organizationRepository               = new MockOrganizationRepository();
            _eaRequestParticipantBridgeRepository = new MockEARequestParticipantBridgeRepository();
            _eaIndividualTypeRepository           = new MockEAIndividualTypeRepository();
            _eaRelationshipTypeRepository         = new MockEARelationshipTypeRepository();
            _eaPaymentAmountRepository            = new MockEAPaymentAmountRepository();
            _eligibilityByFplRepository           = new MockEligibilityByFPLRepository();
            _eaipvRepository                      = new MockEAIPVRepository();
            _WIUIDToFullNameModifiedByResolver    = new MockWIUIDToFullNameModifiedByResolver();
            _mockUnitOfWork                       = new MockUnitOfWork();
            _authUser                             = new AuthUser { WIUID = "1111", AgencyCode = "Test" };

            _config = new MapperConfiguration(cfg =>
                                              {
                                                  cfg.AddProfile<EmergencyAssistanceProfile>();

                                                  cfg.ConstructServicesUsing(i =>
                                                                             {
                                                                                 if (i.Name.Contains(nameof(WIUIDToFullNameModifiedByResolver)))
                                                                                     return _WIUIDToFullNameModifiedByResolver;

                                                                                 return null;
                                                                             });
                                              });

            _mockMapper = _config.CreateMapper();

            _emergencyAssistanceDomain = new EmergencyAssistanceDomain(_mockUnitOfWork, _authUser, new MockWorkerRepository(), _organizationRepository, _eaRequestRepository, _eaRequestParticipantBridgeRepository, null, null, null, new MockParticipantRepository(), _eaRequestContactInfoRepository, null, null, null, null, null, null, _eaIndividualTypeRepository, _eaRelationshipTypeRepository, null, _eaStatusRepository, _eligibilityByFplRepository, _eaPaymentAmountRepository, null, _eaipvRepository, null, null, null, null, null, null, _eaRequestStatusRepository, null, null, _mockMapper);
        }

        [TestMethod]
        public void UpsertDemographics_WithNewContract_DonotCallGetEAStatusRepository()
        {
            _emergencyAssistanceDomain.UpsertDemographics("1234", new EADemographicsContract { EaDemographicsContact = new EADemographicsContactContract() });
            Assert.IsFalse(_eaRequestStatusRepository.GetHasBeenCalled);
            Assert.IsTrue(_eaRequestRepository.ExecFunctionHasBeenCalled);
        }

        [TestMethod]
        public void UpsertDemographics_WithContractApplicationDateIsSameAsOriginalDate_DonotCallGetEAStatusRepository()
        {
            _emergencyAssistanceDomain.UpsertDemographics("1234", new EADemographicsContract { RequestId = 1, EaDemographicsContact = new EADemographicsContactContract(), ApplicationDate = DateTime.Today });
            Assert.IsFalse(_eaRequestStatusRepository.GetHasBeenCalled);
            Assert.IsFalse(_eaRequestRepository.ExecFunctionHasBeenCalled);
        }

        [TestMethod]
        public void UpsertDemographics_WithContractApplicationDateDifferentAsOriginalDate_CallsGetEAStatusRepository()
        {
            _emergencyAssistanceDomain.UpsertDemographics("1234", new EADemographicsContract { RequestId = 1, EaDemographicsContact = new EADemographicsContactContract(), ApplicationDate = DateTime.Today.AddDays(2) });
            Assert.IsTrue(_eaRequestStatusRepository.GetHasBeenCalled);
            Assert.IsTrue(_eaRequestRepository.ExecFunctionHasBeenCalled);
        }





        private class MockEARequestRepository : MockRepositoryBase<EARequest>, IEARequestRepository
        {
            public new EARequest Get(Expression<Func<EARequest, bool>> clause)
            {
                return new EARequest { Id = 1, ApplicationDate = DateTime.Today };
            }
        }

        private class MockEARequestStatusRepository : MockRepositoryBase<EARequestStatus>, IEARequestStatusRepository
        {
            public bool GetHasBeenCalled;

            public new EARequestStatus Get(Expression<Func<EARequestStatus, bool>> clause)
            {
                GetHasBeenCalled = true;
                return new EARequestStatus();
            }
        }

        private class MockEAStatusRepository : MockRepositoryBase<EAStatus>, IEAStatusRepository
        {
            private readonly EAStatus _eaInprogressStatus = new EAStatus { Id = 1, Code = "IP", Name = "In Progress" };

            public new EAStatus Get(Expression<Func<EAStatus, bool>> clause)
            {
                return new List<EAStatus> { _eaInprogressStatus }.FirstOrDefault(clause.Compile());
            }
        }

        private class MockEARequestContactInfoRepository : MockRepositoryBase<EARequestContactInfo>, IEARequestContactInfoRepository
        {
        }

        private class MockOrganizationRepository : MockRepositoryBase<Organization>, IOrganizationRepository
        {
            public new Organization Get(Expression<Func<Organization, bool>> clause)
            {
                return new Organization { Id = 1 };
            }
        }

        private class MockEARequestParticipantBridgeRepository : MockRepositoryBase<EARequestParticipantBridge>, IEARequestParticipantBridgeRepository
        {
        }

        private class MockEAPaymentAmountRepository : MockRepositoryBase<EAPaymentAmount>, IEAPaymentAmountRepository
        {
        }

        private class MockEAIndividualTypeRepository : MockRepositoryBase<EAIndividualType>, IEAIndividualTypeRepository
        {
        }

        private class MockEARelationshipTypeRepository : MockRepositoryBase<EARelationshipType>, IEARelationshipTypeRepository
        {
        }

        private class MockEligibilityByFPLRepository : MockRepositoryBase<EligibilityByFPL>, IEligibilityByFPLRepository
        {
        }

        private class MockEAIPVRepository : MockRepositoryBase<EAIPV>, IEAIPVRepository
        {
        }

        private class MockWorkerRepository : MockRepositoryBase<Worker>, IWorkerRepository
        {
            public new Worker Get(Expression<Func<Worker, bool>> clause)
            {
                var worker = new Worker();

                return worker;
            }
        }
    }
}
