using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class GoalDomainTest
    {
        #region Properties

        private GoalDomain                                _goalDomain;
        private MockGoalRepository                        _mockGoalRepository;
        private MockEmployabilityPlanGoalBridgeRepository _mockEmployabilityPlanGoalBridgeRepository;
        private MockWorkerRepository                      _mockWorkerRepository;
        private MockUnitOfWork                            _mockUnitOfWork;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _mockGoalRepository                        = new MockGoalRepository();
            _mockEmployabilityPlanGoalBridgeRepository = new MockEmployabilityPlanGoalBridgeRepository();
            _mockWorkerRepository                      = new MockWorkerRepository();
            _mockUnitOfWork                            = new MockUnitOfWork();

            _goalDomain = new GoalDomain(_mockGoalRepository, null, _mockEmployabilityPlanGoalBridgeRepository, _mockWorkerRepository, _mockUnitOfWork, null);
        }

        [TestMethod]
        [DynamicData(nameof(GetContract), DynamicDataSourceType.Method)]
        public void DeleteGoal_BasedOnCanCommitFlag_CallsCommit(CommitInput input)
        {
            _goalDomain.DeleteGoal(input.GoalId, input.EpId, input.CanCommit);

            Assert.AreEqual(input.CanCommit ? 1 : 0, _mockUnitOfWork.CommitCalled);
        }

        [TestMethod]
        public void DeleteGoal_WhenGoalBridgeCountIsOne_GoalRepositoryDeleteIsCalled()
        {
            _goalDomain.DeleteGoal(1, 1, false);

            Assert.IsTrue(_mockGoalRepository.DeleteHasBeenCalled);
            Assert.IsFalse(_mockEmployabilityPlanGoalBridgeRepository.DeleteHasBeenCalled);
        }

        [TestMethod]
        public void DeleteGoal_WhenGoalBridgeCountIsMoreThanOne_EPGoalBridgeRepositoryDeleteIsCalled()
        {
            _mockEmployabilityPlanGoalBridgeRepository.ReturnItems = 2;
            _goalDomain.DeleteGoal(1, 1, false);

            Assert.IsTrue(_mockEmployabilityPlanGoalBridgeRepository.DeleteHasBeenCalled);
            Assert.IsFalse(_mockGoalRepository.DeleteHasBeenCalled);
        }

        private static IEnumerable<CommitInput[]> GetContract()
        {
            return new[]
                   {
                       new CommitInput { GoalId = 1, EpId = 1, CanCommit = false },
                       new CommitInput { GoalId = 1, EpId = 1, CanCommit = true }
                   }.Select(i => new[] { i });
        }

        #endregion

        #region Mocks

        private class MockGoalRepository : MockRepositoryBase<Goal>, IGoalRepository
        {
            public new void Delete(Expression<Func<Goal, bool>> clause)
            {
                DeleteHasBeenCalled = true;
            }
        }

        private class MockEmployabilityPlanGoalBridgeRepository : MockRepositoryBase<EmployabilityPlanGoalBridge>, IEmployabilityPlanGoalBridgeRepository
        {
            public int ReturnItems = 1;

            public new IEnumerable<EmployabilityPlanGoalBridge> GetMany(Expression<Func<EmployabilityPlanGoalBridge, bool>> clause)
            {
                var epabs = new List<EmployabilityPlanGoalBridge> { new EmployabilityPlanGoalBridge { Id = 1 } };

                if (ReturnItems > 1)
                    epabs.Add(new EmployabilityPlanGoalBridge
                              {
                                  Id = 2
                              });


                return epabs;
            }

            public new void Delete(Expression<Func<EmployabilityPlanGoalBridge, bool>> clause)
            {
                DeleteHasBeenCalled = true;
            }
        }

        #endregion

        #region Inputs

        public class CommitInput
        {
            public int  GoalId    { get; set; }
            public int  EpId      { get; set; }
            public bool CanCommit { get; set; }
        }

        #endregion
    }
}
