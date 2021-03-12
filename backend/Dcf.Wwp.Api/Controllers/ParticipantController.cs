using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Timelimts.Service;
using Dcf.Wwp.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Dcf.Wwp.Api.Controllers
{
    /// <summary>
    ///     Participant Controller section : notice singular participant
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class ParticipantController : BaseController
    {
        #region Properties

        private readonly IReadOnlyCollection<ICountyAndTribe> _countyAndTribes;
        private readonly IAuthUser                            _authUser;
        private readonly ITimelimitService                    _timelimitService;
        private readonly IDb2TimelimitService                 _db2TimelimitService;
        private readonly IConfidentialityChecker              _confidentialityChecker;
        private readonly IAuthAccessChecker                   _authAccessChecker;
        private readonly ICwwIndService                       _cwwIndSvc;
        private readonly IParticipantDomain                   _participantDomain;
        private readonly IEmployabilityPlanDomain             _epDomain;
        private readonly bool                                 _useWS;
        private readonly IUnitOfWork                          _unitOfWork;
        private readonly ITransactionDomain                   _transactionDomain;

        #endregion

        #region Methods

        public ParticipantController(IRepository repository, IAuthUser authUser, IConfidentialityChecker confidentialityChecker, IReadOnlyCollection<ICountyAndTribe> countyAndTribes, ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService, ICwwIndService cwwIndSvc, IAuthAccessChecker authAccessChecker, IParticipantDomain participantDomain, IEmployabilityPlanDomain epDomain, IDatabaseConfiguration dbConfig, IUnitOfWork unitOfWork, ITransactionDomain transactionDomain) : base(repository)
        {
            _authUser               = authUser;
            _confidentialityChecker = confidentialityChecker;
            _countyAndTribes        = countyAndTribes;
            _timelimitService       = timelimitService;
            _db2TimelimitService    = db2TimelimitService;
            _cwwIndSvc              = cwwIndSvc;
            _authAccessChecker      = authAccessChecker;
            _participantDomain      = participantDomain;
            _epDomain               = epDomain;
            _unitOfWork             = unitOfWork;
            _transactionDomain      = transactionDomain;

            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddEnvironmentVariables()
                     .AddJsonFile("connectedServices.json", true)
                     .Build();

            var wcfSection   = cb.GetSection("wcfSoap");
            var wcfConfigs   = wcfSection.Get<List<WcfSoapConfig>>();
            var wcfEnvConfig = wcfConfigs.FirstOrDefault(i => i.Env == dbConfig.Catalog);
            _useWS = wcfEnvConfig?.UseWS ?? true;
        }

        [HttpGet("{pin}/refresh")]
        public IActionResult GetRefreshedParticipant(string pin, string refresh, [FromQuery] bool usePEPAgency = false)
        {
            var pvm  = new ParticipantViewModel(Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var data = pvm.GetParticipant(pin, true, usePEPAgency, _useWS);

            if (data == null)
            {
                return NotFound(new ParticipantsContract()); // we have to do this because of what the front end expects
            }

            return Ok(data);
        }

        [HttpGet("{pin}")]
        public IActionResult GetParticipant(string pin, string refresh, [FromQuery] bool usePEPAgency = false)
        {
            var pvm  = new ParticipantViewModel(Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var data = pvm.GetParticipant(pin, false, usePEPAgency, _useWS);

            if (data == null)
            {
                return NotFound(new ParticipantsContract()); // we have to do this because of what the front end expects
            }

            return Ok(data);
        }

        [HttpGet("{pin}/details")]
        public IActionResult GetParticipantSummary(string pin, [FromQuery] bool fromSummary = false)
        {
            var pvm  = new ParticipantViewModel(Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var data = pvm.GetParticipantSummary(pin, fromSummary);

            return Ok(data);
        }

        [HttpPut("enroll")]
        public IActionResult EnrollParticipantByProgram([FromBody] EnrolledProgramContract programInfo)
        {
            var pvm = new ParticipantViewModel(Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var res = pvm.EnrollParticipant(programInfo);

            return Ok(res);
        }

        [HttpGet("{pin}/preenroll/{programId}")]
        public IActionResult PreEnrollParticipantByProgram(string pin, int programId)
        {
            var pvm  = new ParticipantViewModel(pin, Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var data = pvm.PreEnrollParticipant(programId);

            return Ok(data);
        }

        [HttpGet("{pin}/predisenroll/{programId}")]
        public IActionResult PreDisenrollParticipantByProgram(string pin, int programId)
        {
            var pvm  = new ParticipantViewModel(pin, Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var data = pvm.PreDisenrollParticipant(programId);

            return Ok(data);
        }

        [HttpPut("disenroll")]
        public IActionResult DisenrollParticipantByProgram([FromBody] EnrolledProgramContract programInfo)
        {
            var pvm = new ParticipantViewModel(Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            pvm.DisenrollParticipant(programInfo);

            return Ok(programInfo);
        }

        [HttpPut("reassign")]
        public IActionResult ReassignWorkerToEnrolledParticipant([FromBody] EnrolledProgramContract programInfo)
        {
            var pvm = new ParticipantViewModel(Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            pvm.ReassignParticipantToWorker(programInfo);

            return Ok(programInfo);
        }

        [HttpPut("{pin}/transfer")]
        public IActionResult Transfer(string pin, [FromBody] EnrolledProgramContract programInfo)
        {
            var pvm = new ParticipantViewModel(pin, Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            pvm.TransferParticipant(programInfo, pin);

            return Ok();
        }

        [HttpPost("{pin}/pretransfer")]
        public IActionResult PreTransferParticipantByProgram(string pin, [FromBody] EnrolledProgramContract programInfo)
        {
            var pvm  = new ParticipantViewModel(pin, Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var data = pvm.PreTransferParticipant(programInfo);

            return Ok(data);
        }

        [HttpGet("{pin}/status/current")]
        public IActionResult GetCurrentStatusesForPin(string pin)
        {
            var statusList = _participantDomain.GetCurrentStatusesForPin(pin);

            return Ok(statusList);
        }

        [HttpGet("{pin}/status/{id}")]
        public IActionResult GetStatus(string pin, int id)
        {
            var statusList = _participantDomain.GetStatus(pin, id);

            return Ok(statusList);
        }

        [HttpGet("{pin}/status/all")]
        public IActionResult GetAllStatusesForPin(string pin)
        {
            var statusList = _participantDomain.GetAllStatusesForPin(pin);

            return Ok(statusList);
        }

        [HttpPost("{pin}/status/add")]
        public IActionResult AddStatus([FromBody] ParticipantStatusContract participantStatusContract)
        {
            var newParticipationStatus = _participantDomain.AddStatus(participantStatusContract);

            return Ok(newParticipationStatus);
        }

        [HttpPost("{pin}/status/update")]
        public IActionResult UpdateStatus([FromBody] ParticipantStatusContract participantStatusContract)
        {
            var newParticipationStatus = _participantDomain.UpdateStatus(participantStatusContract);

            return Ok(newParticipationStatus);
        }

        [HttpPost("{pin}/status/validate")]
        public IActionResult ValidateParticipationStatus(string pin, [FromBody] ParticipantStatusContract participantStatusContract)

        {
            var pvm                    = new ParticipantViewModel(pin, Repo, _authUser, _confidentialityChecker, _timelimitService, _db2TimelimitService, _countyAndTribes, _cwwIndSvc, _authAccessChecker, _epDomain, _unitOfWork, _transactionDomain);
            var activities             = _participantDomain.GetActivitiesForTjOrTmjProgramsForPreCheck(pin);
            var newParticipationStatus = pvm.ValidateAddingParticipationStatus(participantStatusContract, activities);

            return Ok(newParticipationStatus);
        }

        [HttpGet("{pin}/caseNumber/{participationPeriod}/{year}")]
        public IActionResult GetCaseNumber(string pin, string participationPeriod, short year)
        {
            var contract = _participantDomain.GetCaseNumbersBasedOnParticipationPeriod(pin, participationPeriod, year);

            return (Ok(contract));
        }

        [HttpGet("{pin}/paymentDetails/{participationPeriod}/{year}/{caseNumber}")]
        public IActionResult GetDetails(string pin, string participationPeriod, short year, string caseNumber)
        {
            var contract = _participantDomain.GetDetailsBasedOnParticipationPeriod(pin, participationPeriod, year, caseNumber);
            var res      = Ok(contract);

            return (res);
        }

        #endregion
    }
}
