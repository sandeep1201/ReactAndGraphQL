using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dcf.Wwp.Api.Controllers
{
    #region Participants Controller

    /// <summary>
    ///     Participants Controller Section : notice multiple participant
    ///     These Participants are the
    /// </summary>
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ParticipantsController : BaseController
    {
        private readonly IAuthUser                            _authUser;
        private readonly IConfidentialityChecker              _confidentialityChecker;
        private readonly IAuthAccessChecker                   _authAccessChecker;
        private readonly IReadOnlyCollection<ICountyAndTribe> _countyAndTribesList;
        private readonly bool                                 _useWS;

        public ParticipantsController(IRepository repository, IAuthUser authUser, IConfidentialityChecker confidentialityChecker, IReadOnlyCollection<ICountyAndTribe> countyAndTribesList, IAuthAccessChecker authAccessChecker, IDatabaseConfiguration dbConfig) : base(repository)
        {
            _authUser               = authUser;
            _confidentialityChecker = confidentialityChecker;
            _countyAndTribesList    = countyAndTribesList;
            _authAccessChecker      = authAccessChecker;

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

        [HttpGet("featureToggles/{fieldName}")]
        [EnableCors("AllowAll")]
        public IActionResult GetFeatureToggleValues(string fieldName)
        {
            var recentparticipants = new ParticipantsViewModel(Repo, _authUser, _confidentialityChecker, _countyAndTribesList, _authAccessChecker);
            var data               = recentparticipants.GetFeatureToggleValues(fieldName);

            return Ok(data);
        }

        [HttpGet("{user}/recent")]
        [EnableCors("AllowAll")]
        public IActionResult GetRecentParticipantsByUser(string user)
        {
            var recentparticipants = new ParticipantsViewModel(Repo, _authUser, _confidentialityChecker, _countyAndTribesList, _authAccessChecker);
            var data               = recentparticipants.GetRecentParticipants(user, _useWS);

            return Ok(data);
        }

        [HttpGet("{user?}/{agency}/{program?}")]
        [EnableCors("AllowAll")]
        public IActionResult GetParticipantsForWorker(string user, string agency, string program)
        {
            var participants = new ParticipantsViewModel(Repo, _authUser, _confidentialityChecker, _countyAndTribesList, _authAccessChecker);
            var data         = participants.GetParticipantsForWorker(user, agency, program, _useWS);

            if (data == null)
            {
                throw new EntityNotFoundException("Something unexpected happened.");
            }

            return Ok(data);
        }

        [HttpGet("{user}/referrals/{refresh?}")]
        [EnableCors("AllowAll")]
        public IActionResult GetReferralsByUser(string user, string refresh)
        {
            var partcipantsVm = new ParticipantsViewModel(Repo, _authUser, _confidentialityChecker, _countyAndTribesList, _authAccessChecker);
            var data          = partcipantsVm.GetReferralsAndTransfersResults(user, refresh.IsNullOrWhiteSpace(), _useWS);

            if (data == null)
            {
                throw new EntityNotFoundException("Data couldn't be retrieved");
            }

            return Ok(data);
        }

        [HttpGet("search")]
        [EnableCors("AllowAll")]
        public IActionResult GetParticipantsBySearch([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] string middleName, [FromQuery] string gender, [FromQuery] DateTime? dob)
        {
            var participants = new ParticipantsViewModel(Repo, _authUser, _confidentialityChecker, _countyAndTribesList, _authAccessChecker);
            var data = participants.GetParticipantsBySearch(firstName, lastName, middleName, gender, dob, _useWS);

            if (data == null)
            {
                throw new EntityNotFoundException("Something unexpected happened.");
            }

            return Ok(data);
        }
    }

    #endregion
}
