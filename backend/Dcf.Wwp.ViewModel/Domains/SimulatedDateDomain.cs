using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class SimulatedDateDomain : ISimulatedDateDomain
    {

        #region Properties

        private readonly ISimulatedDateRepository _simulatedDateRepository;
        private readonly IAuthUser _authUser;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Methods

        public SimulatedDateDomain(ISimulatedDateRepository simulatedDateRepository,
            IAuthUser authUser,
            IWorkerRepository workerRepo,
            IUnitOfWork unitOfWork)
        {
            _simulatedDateRepository = simulatedDateRepository;
            _authUser = authUser;
            _unitOfWork = unitOfWork;
        }

        public SimulatedDateContract GetSimulatedDate(int id)
        {
            SimulatedDateContract contract = null;

            var simulatedDate = _simulatedDateRepository.Get(sd => sd.Id == id);

            if (simulatedDate != null)
            {
                contract = new SimulatedDateContract
                {
                    Id = simulatedDate.Id,
                    WUID = simulatedDate.WUID,
                    StartTimeStamp = simulatedDate.StartTimeStamp,
                    CDODate = simulatedDate.CDODate,
                    IsDeleted = simulatedDate.IsDeleted,
                    ModifiedBy = simulatedDate.WUID,
                    ModifiedDate = simulatedDate.ModifiedDate

                };
            }

            return (contract);
        }

        public SimulatedDateContract UpsertSimulateDate(SimulatedDateContract simulatedDateContract)
        {
            SimulatedDateContract contract = null;

            if (simulatedDateContract == null)
            {
                throw new ArgumentNullException(nameof(simulatedDateContract));
            }

            var updateTime = DateTime.Now;

            var simulatedDate = simulatedDateContract.Id != 0 ? _simulatedDateRepository.Get(sd => sd.Id == simulatedDateContract.Id && sd.IsDeleted == false) : _simulatedDateRepository.New();

            if (simulatedDateContract.Id != 0)
            {
                if (simulatedDate != null)
                {
                    // We only update SimulatedDate records to add an EndTimeStamp and update the last modified info
                    simulatedDate.EndTimeStamp = updateTime;
                    simulatedDate.ModifiedBy = simulatedDate.WUID;
                    simulatedDate.ModifiedDate = updateTime;

                    _simulatedDateRepository.Update(simulatedDate);
                    _unitOfWork.Commit();
                    contract = GetSimulatedDate(simulatedDate.Id);
                }
            } else
            {
                simulatedDate.WUID = _authUser.WIUID;
                simulatedDate.StartTimeStamp = updateTime;
                simulatedDate.CDODate = simulatedDateContract.CDODate;
                simulatedDate.ModifiedBy = simulatedDate.WUID;
                simulatedDate.ModifiedDate = updateTime;

                _simulatedDateRepository.Add(simulatedDate);
                _unitOfWork.Commit();
                contract = GetSimulatedDate(simulatedDate.Id);
            }

            return contract;
        }

        #endregion
    }
}
