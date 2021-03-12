using AutoMapper;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.UnitTest.Api.AutoMapper;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class PlanDomainTest
    {
        #region Properties

        private MockPlanRepository  _planRepository;
        private PlanDomain          _planDomain;
        private MockUnitOfWork      _mockUnitOfWork;
        private IMapper             _mockMapper;
        private MapperConfiguration _config;
        private MapperBaseTest      _mapperBaseTest;
        private IAuthUser           _authUser;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _planRepository = new MockPlanRepository();
            _mockUnitOfWork = new MockUnitOfWork();
            _authUser       = new AuthUser { WIUID = "1111" };
            _mapperBaseTest = new MapperBaseTest();
            _config         = new MapperConfiguration(cfg => cfg.AddProfile<PlanProfile>());
            _mockMapper     = _config.CreateMapper();
            _planDomain     = new PlanDomain(_planRepository, _authUser, _mockUnitOfWork, _mockMapper);
        }

        [TestMethod]
        public void PlanProfileTestMappingEmptyEntityToContractForValueEquality()
        {
            var entity   = new PlanContract();
            var contract = _mockMapper.Map<PlanContract>(entity);

            _mapperBaseTest.AssertAllPropertiesMapped(entity, contract);
        }

        [TestMethod]
        public void GetW2PlansByParticipantId_ValidParticipantId_GetsPlansFromRepositoryAndReturnsValidContract()
        {
            var contract = _planDomain.GetW2PlansByParticipantId(123);
            Assert.IsTrue(_planRepository.GetManyHasBeenCalled);
            Assert.IsNotNull(contract);
        }

        #endregion

        #region Mocks

        private class MockPlanRepository : MockRepositoryBase<Plan>, IPlanRepository
        {
        }

        #endregion
    }
}
