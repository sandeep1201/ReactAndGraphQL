using System;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class WorkerDomain : IWorkerContactInfoDomain
    {
        #region Properties

        private readonly IWorkerContactInfoRepository _workerContactInfoRepository;
        private readonly IWorkerRepository            _workerRepository;
        private readonly IAuthUser                    _authUser;
        private readonly IUnitOfWork                  _unitOfWork;

        #endregion

        #region Methods

        public WorkerDomain(IWorkerContactInfoRepository workerContactInfoRepository, IWorkerRepository workerRepository, IAuthUser authUser, IUnitOfWork unitOfWork)
        {
            _workerContactInfoRepository = workerContactInfoRepository;
            _workerRepository            = workerRepository;
            _authUser                    = authUser;
            _unitOfWork                  = unitOfWork;
        }

        /// <summary>
        /// Get the Worker contact info based on wiUid
        /// </summary>
        /// <param name="wiUId"></param>
        /// <returns></returns>
        public WorkerInfoContract GetContactInfo()
        {
            var workerInfoContract = new WorkerInfoContract();
            var worker             = _workerRepository.Get(i => i.WIUId == _authUser.WIUID);
            var contactInfo        = _workerContactInfoRepository.Get(i => i.WorkerId == worker.Id && i.IsDeleted == false);

            workerInfoContract.WorkerId = worker.Id;

            if (contactInfo == null) return workerInfoContract;
            workerInfoContract.Id          = contactInfo.Id;
            workerInfoContract.Email       = contactInfo.Email;
            workerInfoContract.PhoneNumber = contactInfo.PhoneNumber;

            return workerInfoContract;
        }

        /// <summary>
        /// Save WorkerInfo to DB
        /// </summary>
        /// <param name="workerInfoContract"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorkerInfoContract UpsertIContactInformation(WorkerInfoContract workerInfoContract)
        {
            if (workerInfoContract == null)
            {
                throw new ArgumentNullException(nameof(workerInfoContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var contactInfo = workerInfoContract.Id != 0
                                  ? _workerContactInfoRepository.Get(g => g.Id == workerInfoContract.Id && g.IsDeleted == false)
                                  : _workerContactInfoRepository.New();

            contactInfo.PhoneNumber  = workerInfoContract.PhoneNumber;
            contactInfo.Email        = workerInfoContract.Email;
            contactInfo.ModifiedBy   = modifiedBy;
            contactInfo.WorkerId     = workerInfoContract.WorkerId;
            contactInfo.ModifiedDate = modifiedDate;

            if (workerInfoContract.Id != 0)
            {
                _workerContactInfoRepository.Update(contactInfo);
            }
            else
            {
                _workerContactInfoRepository.Add(contactInfo);
            }

            _unitOfWork.Commit();

            workerInfoContract.Id = contactInfo.Id;
            return (workerInfoContract);
        }

        #endregion
    }
}
