using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Constants;
using SmartFormat;
using ITransactionRepository = Dcf.Wwp.DataAccess.Interfaces.ITransactionRepository;
using Phase1Transaction = Dcf.Wwp.Data.Sql.Model.Transaction;
using Transaction = Dcf.Wwp.DataAccess.Models.Transaction;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class TransactionDomain : ITransactionDomain
    {
        #region Properties

        private readonly IMapper                      _mapper;
        private readonly ITransactionRepository       _transactionRepository;
        private readonly ITransactionTypeRepository   _transactionTypeRepository;
        private readonly ISpecialInitiativeRepository _featureToggleRepository;

        #endregion

        #region Methods

        public TransactionDomain(IMapper                      mapper,
                                 ITransactionRepository       transactionRepository,
                                 ITransactionTypeRepository   transactionTypeRepository,
                                 ISpecialInitiativeRepository featureToggleRepository)
        {
            _mapper                    = mapper;
            _transactionRepository     = transactionRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _featureToggleRepository   = featureToggleRepository;
        }

        public List<TransactionContract> GetTransactions(int participantId)
        {
            return _mapper.Map<List<TransactionContract>>(_transactionRepository.GetMany(i => i.ParticipantId == participantId)?.OrderByDescending(j => j.CreatedDate.Date).ThenByDescending(i => i.Id));
        }

        public dynamic InsertTransaction(TransactionContract transactionContract, bool returnInterface = false)
        {
            if (_featureToggleRepository.Get(i => i.ParameterName == TransactionTypes.Transactions).ParameterValue.ToDateMonthDayYear().IsSameOrBefore(DateTime.Today))
            {
                var transactionType   = _transactionTypeRepository.Get(i => i.Code == transactionContract.TransactionTypeCode);
                var transactionTypeId = transactionType.Id;
                var description       = Smart.Format(transactionType.Description, transactionContract.StatusCode);

                if (returnInterface)
                {
                    var phase1Transaction = _mapper.Map<Phase1Transaction>(transactionContract);

                    phase1Transaction.TransactionTypeId = transactionTypeId;
                    phase1Transaction.Description       = description;
                    phase1Transaction.ModifiedBy        = phase1Transaction.ModifiedBy ?? transactionContract.ModifiedBy;

                    return phase1Transaction;
                }

                var transaction = _mapper.Map<Transaction>(transactionContract);

                transaction.TransactionTypeId = transactionTypeId;
                transaction.Description       = description;
                transaction.ModifiedBy        = transaction.ModifiedBy ?? transactionContract.ModifiedBy;

                _transactionRepository.Add(transaction);
            }

            return null;
        }

        #endregion
    }
}
