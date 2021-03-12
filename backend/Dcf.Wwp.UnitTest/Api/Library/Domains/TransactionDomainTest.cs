using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver;
using Dcf.Wwp.UnitTest.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class TransactionDomainTest
    {
        #region Properties

        private       MockTransactionRepository             _mockTransactionRepository;
        private       IMapper                               _mockMapper;
        private       MockTransactionTypeRepository         _mockTransactionTypeRepository;
        private       MockSpecialInitiativeRepository       _mockSpecialInitiativeRepository;
        private const int                                   ParticipantIdReturnsNull = 12354796;
        private const int                                   ValidId                  = 123;
        private       MockSetModifiedDetailsResolver        _mockSetModifiedDetailsResolver;
        private       MockSetWorkerNameFromWorkerIdResolver _mockSetWorkerNameFromWorkerIdResolver;
        private       MockWIUIDToFullNameModifiedByResolver _mockWIUIDToFullNameModifiedByResolver;
        private       TransactionDomain                     _transactionDomain;
        private       TransactionContract                   _mockTransactionContract;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _mockTransactionRepository             = new MockTransactionRepository();
            _mockTransactionTypeRepository         = new MockTransactionTypeRepository();
            _mockSetModifiedDetailsResolver        = new MockSetModifiedDetailsResolver();
            _mockSpecialInitiativeRepository       = new MockSpecialInitiativeRepository();
            _mockSetWorkerNameFromWorkerIdResolver = new MockSetWorkerNameFromWorkerIdResolver();
            _mockWIUIDToFullNameModifiedByResolver = new MockWIUIDToFullNameModifiedByResolver();

            _mockTransactionContract = new TransactionContract
                                       {
                                           ParticipantId       = 123,
                                           WorkerId            = 1,
                                           OfficeId            = 2,
                                           Description         = "Test",
                                           EffectiveDate       = DateTime.Today,
                                           TransactionTypeCode = _mockTransactionTypeRepository.FakeValidTransaction.Code
                                       };

            var config = new MapperConfiguration(cfg =>
                                                 {
                                                     cfg.AddProfile<TransactionProfile>();

                                                     cfg.ConstructServicesUsing(i =>
                                                                                {
                                                                                    if (i.Name.Contains(nameof(SetModifiedDetailsResolver)))
                                                                                        return _mockSetModifiedDetailsResolver;

                                                                                    if (i.Name.Contains(nameof(SetWorkerNameFromWorkerIdResolver)))
                                                                                        return _mockSetWorkerNameFromWorkerIdResolver;

                                                                                    if (i.Name.Contains(nameof(WIUIDToFullNameModifiedByResolver)))
                                                                                        return _mockWIUIDToFullNameModifiedByResolver;

                                                                                    return null;
                                                                                });
                                                 });

            _mockMapper = config.CreateMapper();

            _transactionDomain = new TransactionDomain(_mockMapper, _mockTransactionRepository, _mockTransactionTypeRepository, _mockSpecialInitiativeRepository);
        }

        [TestMethod]
        public void GetTransactionsByParticipant_ValidId_ReturnsTransactions()
        {
            Assert.IsNotNull(_transactionDomain.GetTransactions(ValidId));
        }

        [TestMethod]
        public void GetTransactionsByParticipant_TransactionRepositoryGetManyReturnsNull_ReturnsEmptyList()
        {
            Assert.IsNotNull(_transactionDomain.GetTransactions(ParticipantIdReturnsNull));
        }

        [TestMethod]
        public void GetTransactionsByParticipant_CallsGetMany_Works()
        {
            _transactionDomain.GetTransactions(1254684);
            Assert.IsTrue(_mockTransactionRepository.GetManyHasBeenCalled);
        }

        [TestMethod]
        public void InsertTransaction_CallsRepoGetSpecialInitiative()
        {
            var result = _transactionDomain.InsertTransaction(_mockTransactionContract);

            Assert.IsTrue(_mockSpecialInitiativeRepository.GetHasBeenCalled);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void InsertTransaction_CallsRepoInsert()
        {
            _transactionDomain.InsertTransaction(_mockTransactionContract);
            Assert.IsTrue(_mockTransactionRepository.AddHasBeenCalled);
        }

        [TestMethod]
        public void InsertTransaction_CreatesTransactionFromTransactionContract_CallsRepoInsertWithTransaction()
        {
            _transactionDomain.InsertTransaction(_mockTransactionContract);
            Assert.IsTrue(_mockTransactionRepository.AddHasBeenCalled);
            Assert.AreEqual(_mockTransactionContract.ParticipantId,                 _mockTransactionRepository.PassedInToRepository.ParticipantId);
            Assert.AreEqual(_mockTransactionContract.WorkerId,                      _mockTransactionRepository.PassedInToRepository.WorkerId);
            Assert.AreEqual(_mockTransactionContract.OfficeId,                      _mockTransactionRepository.PassedInToRepository.OfficeId);
            Assert.AreEqual(_mockTransactionContract.Description,                   _mockTransactionRepository.PassedInToRepository.Description);
            Assert.AreEqual(_mockTransactionTypeRepository.FakeValidTransaction.Id, _mockTransactionRepository.PassedInToRepository.TransactionTypeId);
            Assert.AreEqual(_mockTransactionContract.EffectiveDate,                 _mockTransactionRepository.PassedInToRepository.EffectiveDate);
        }

        [TestMethod]
        public void InsertTransaction_CreatesTransactionFromTransactionContract_ReturnsInterfaceForPhase1()
        {
            var result = _transactionDomain.InsertTransaction(_mockTransactionContract, true);

            Assert.IsInstanceOfType(result, typeof(ITransaction));
            Assert.AreEqual(_mockTransactionContract.ParticipantId,                 result.ParticipantId);
            Assert.AreEqual(_mockTransactionContract.WorkerId,                      result.WorkerId);
            Assert.AreEqual(_mockTransactionContract.OfficeId,                      result.OfficeId);
            Assert.AreEqual(_mockTransactionTypeRepository.FakeValidTransaction.Id, result.TransactionTypeId);
            Assert.AreEqual(_mockTransactionContract.Description,                   result.Description);
            Assert.AreEqual(_mockTransactionContract.EffectiveDate,                 result.EffectiveDate);
        }

        [TestMethod]
        public void InsertTransaction_ReturnInterfaceIsTrue_ReturnsInterfaceForPhase1()
        {
            Assert.IsInstanceOfType(_transactionDomain.InsertTransaction(_mockTransactionContract, true), typeof(ITransaction));
        }

        [TestMethod]
        public void InsertTransaction_ReturnInterfaceIsFalse_ReturnsNull()
        {
            Assert.IsNull(_transactionDomain.InsertTransaction(_mockTransactionContract));
        }

        [TestMethod]
        public void InsertTransaction_TransactionContractHasNoDescription_TransactionTypeRepositoryWillRetrieveDescription()
        {
            _mockTransactionContract.Description = null;
            Assert.AreEqual(_mockTransactionTypeRepository.FakeValidTransaction.Description, _transactionDomain.InsertTransaction(_mockTransactionContract, true).Description);
        }

        #endregion

        private class MockTransactionRepository : MockRepositoryBase<Transaction>, ITransactionRepository
        {
            public bool        GetManyHasBeenCalled;
            public Transaction PassedInToRepository;

            private readonly Transaction _knownTransaction = new Transaction
                                                             {
                                                                 ParticipantId     = 123,
                                                                 WorkerId          = 1,
                                                                 OfficeId          = 3,
                                                                 TransactionTypeId = 4,
                                                                 Description       = "test test",
                                                                 EffectiveDate     = DateTime.Today,
                                                                 CreatedDate       = DateTime.Today,
                                                                 IsDeleted         = false,
                                                                 ModifiedBy        = "test",
                                                                 ModifiedDate      = DateTime.Today,
                                                                 Worker = new Worker
                                                                          {
                                                                              FirstName = "Test",
                                                                              LastName  = "Test"
                                                                          },
                                                                 TransactionType = new TransactionType
                                                                                   {
                                                                                       Name        = "Test",
                                                                                       Code        = "TEST",
                                                                                       Description = "Test"
                                                                                   },
                                                                 Office = new Office
                                                                          {
                                                                              ContractArea = new ContractArea
                                                                                             {
                                                                                                 Organization = new Organization
                                                                                                                {
                                                                                                                    AgencyName = "FSC"
                                                                                                                }
                                                                                             },
                                                                              CountyAndTribe = new CountyAndTribe
                                                                                               {
                                                                                                   CountyName = "Dane"
                                                                                               }
                                                                          }
                                                             };

            public new IEnumerable<Transaction> GetMany(Expression<Func<Transaction, bool>> clause)
            {
                GetManyHasBeenCalled = true;
                var transactions = new List<Transaction> { _knownTransaction };
                var results      = transactions.Where(clause.Compile()).ToList();
                return results.Any() ? results : null;
            }

            public new void Add(Transaction entity)
            {
                PassedInToRepository = entity;
                base.Add(entity);
            }
        }

        private class MockTransactionTypeRepository : MockRepositoryBase<TransactionType>, ITransactionTypeRepository
        {
            public readonly TransactionType FakeValidTransaction = new TransactionType
                                                                   {
                                                                       Id          = 19,
                                                                       Code        = "TEST",
                                                                       Description = "Test"
                                                                   };

            public new TransactionType Get(Expression<Func<TransactionType, bool>> clause)
            {
                var fakeInValidTransaction = new TransactionType
                                             {
                                                 Id   = 2,
                                                 Code = "TEST2"
                                             };
                var fakeData = new List<TransactionType> { fakeInValidTransaction, FakeValidTransaction };
                return fakeData.FirstOrDefault(clause.Compile());
            }
        }

        private class MockSpecialInitiativeRepository : MockRepositoryBase<SpecialInitiative>, ISpecialInitiativeRepository
        {
            public bool GetHasBeenCalled;

            public readonly SpecialInitiative FakeValidFeatureDate = new SpecialInitiative
                                                                     {
                                                                         Id             = 19,
                                                                         ParameterName  = "Transactions",
                                                                         ParameterValue = "10-10-2020"
                                                                     };

            public new SpecialInitiative Get(Expression<Func<SpecialInitiative, bool>> clause)
            {
                GetHasBeenCalled = true;
                var fakeInValidFeatureDate = new SpecialInitiative
                                             {
                                                 Id             = 40,
                                                 ParameterName  = "TEST",
                                                 ParameterValue = "10-10-2040"
                                             };
                var fakeData = new List<SpecialInitiative> { FakeValidFeatureDate, fakeInValidFeatureDate };
                return fakeData.FirstOrDefault(clause.Compile());
            }
        }
    }
}
