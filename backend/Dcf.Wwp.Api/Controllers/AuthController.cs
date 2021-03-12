using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Core.EntSec;
using Dcf.Wwp.Api.Library.Auth;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.ViewModels.Account;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService    _authService;
        private readonly IAuthUser                 _authUser;
        private readonly ITokenProvider            _localTokenProvider;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApplicationContext        _appContext;
        private readonly IDatabaseConfiguration    _databaseConfig;

        public AuthController(IRepository    repository,         IAuthenticationService    authService,               IAuthUser          authUser,
                              ITokenProvider localTokenProvider, TokenValidationParameters tokenValidationParameters, ApplicationContext appContext, IDatabaseConfiguration databaseConfig) : base(repository)
        {
            _authService               = authService;
            _authUser                  = authUser;
            _localTokenProvider        = localTokenProvider;
            _tokenValidationParameters = tokenValidationParameters;
            _appContext                = appContext;
            _databaseConfig            = databaseConfig;
        }

        [HttpPost]
        [EnableCors("AllowAll")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] UserContract model)
        {
            var    user = _authService.AuthenticateUser(model.Username, model.Password, Repo.Database);
            string message;

            if (user != null)
            {
                if (user.IsAuthorized)
                {
                    var vm = new LoginViewModel(Repo, _authUser);

                    // We will record the user's login activity and update the table in our DB with
                    // what was just passed to us by EntSec.
                    vm.RecordLogin(user);

                    var data          = vm.GetUserAuthContract(user);
                    var privateClaims = GetUserClaims(data, user);
                    data.Token = _authService.GenerateToken(data.User.Username, privateClaims.ToArray());

                    return Ok(data);
                }
                else
                {
                    message = "Error logging in. " + user.ErrorMessage;
                }
            }
            else
            {
                message = "Username or Password is incorrect";
            }

            var returnData = new { message, token = string.Empty };
            return Json(returnData);
        }

        private List<Claim> GetUserClaims(AuthContract authContract, IUserProfile user)
        {
            var privateClaims = new List<Claim>
                                {
                                    new Claim(JwtRegisteredClaimNames.GivenName,  string.IsNullOrWhiteSpace(authContract.User.MiddleName) ? authContract.User.FirstName : $"{authContract.User.FirstName} {authContract.User.MiddleName}."),
                                    new Claim(JwtRegisteredClaimNames.FamilyName, authContract.User.LastName),
                                    new Claim("entsec_token",                     user.EntSecToken)
                                };

            if (!authContract.User.OfficeName.IsNullOrEmpty())
            {
                privateClaims.Add(new Claim("office_name", authContract.User.OfficeName));
            }

            if (!authContract.User.AgencyCode.IsNullOrEmpty())
            {
                privateClaims.Add(new Claim("agency", authContract.User.AgencyCode));
            }

            //privateClaims.Add(new Claim("canUserRefreshed", authContract.CanUserRefresh.ToString(), ClaimValueTypes.Boolean ));
            //privateClaims.Add(new Claim("lastRefreshedTime", authContract.LastRefreshedTime.ToString(), ClaimValueTypes.Integer64 ));

            if (!user.Wiuid.IsNullOrEmpty())
            {
                privateClaims.Add(new Claim("wiuid", user.Wiuid));
            }

            if (!user.MainFrameId.IsNullOrEmpty())
            {
                privateClaims.Add(new Claim("mainframe_id", user.MainFrameId));
            }

            if (user.RoleNames?.Length > 0)
            {
                privateClaims.Add(new Claim("roles", string.Join(", ", user.RoleNames)));
            }

            if (user.CDODate != null)
            {
                privateClaims.Add(new Claim("cdo_date", user.CDODate.ToString()));
            }

            foreach (var authorization in authContract.User.Authorizations)
            {
                privateClaims.Add(new Claim("authorizations", authorization));
            }

            return privateClaims;
        }

        [HttpPost("simulate")]
        public IActionResult SimulateLogin([FromBody] SimulateUserContract model)
        {
            try
            {
                IAuthUser authUser = GetAuthUserFromToken(model.AuthorizedUserToken);

                if (model.CDODate != null)
                    authUser.CDODate = model.CDODate;

                var vm = new LoginViewModel(Repo, authUser);

                if (_authUser?.IsAuthenticated == true)
                {
                    var localAuthService  = new LocalAuthenticationService(Repo, _localTokenProvider);
                    var adminUser         = localAuthService.AuthenticateUser(authUser.Username, null, null);
                    var adminUserContract = vm.GetUserAuthContract(adminUser);
                    var simulate          = model.CDODate != null ? "canSimulateDateTime" : "canSimulateLogins";
                    var canSimulate       = adminUserContract.User.Authorizations.Contains(simulate);
                    if (!canSimulate)
                    {
                        var error = model.CDODate != null ? $"User {_authUser.Username} is not allowed to simulate this date." : $"User {_authUser.Username} is not allowed to simulate this login.";
                        throw new DCFAuthorizationException(error);
                    }

                    var userId = string.IsNullOrEmpty(model.UserName) ? authUser.Username : model.UserName;

                    var simulatedUser = localAuthService.AuthenticateUser(userId, null, null, model.CDODate);
                    if (simulatedUser?.IsAuthorized == true)
                    {
                        var data = vm.GetUserAuthContract(simulatedUser);
                        if (model.Roles?.Length > 0)
                        {
                            data.User.Authorizations = Repo.AuthorizationsForRoles(model.Roles);
                            simulatedUser.Roles      = model.Roles;
                            simulatedUser.RoleNames  = Repo.AuthorizationRoles(model.Roles).Select(x => x.Name).ToArray();
                        }

                        if (!string.IsNullOrWhiteSpace(model.OrgCode))
                        {
                            var org = Repo.GetOrganizationByCode(model.OrgCode);

                            if (org == null)
                            {
                                throw new DCFApplicationException($"Agency lookup by code failed: {model.OrgCode}");
                            }

                            data.User.AgencyCode = org.EntsecAgencyCode;
                            data.User.OfficeName = org.AgencyName;
                        }

                        var privateClaims = GetUserClaims(data, simulatedUser);

                        data.Token = _authService.GenerateToken(data.User.Username, privateClaims.ToArray());

                        return Ok(data);
                    }
                }

                return Ok(new { message = "Simulated user not authorized" });
            }
            catch (Exception e)
            {
                return Ok(new { message = $"User {_authUser?.Username}  simulation failed for user {model?.UserName}. {e.Message}" });
            }
        }

        private IAuthUser GetAuthUserFromToken(string token)
        {
            IAuthUser user         = new AuthUser();
            var       tokenHandler = new JwtSecurityTokenHandler();
            // ReSharper disable once UnusedVariable
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            if (principal?.Identity?.IsAuthenticated == true)
            {
                // JwtSecurityToken jwtToken = validatedToken as JwtSecurityToken ?? tokenHandler.ReadJwtToken(token);
                user.Username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                user.WIUID    = principal.Claims.Where(c => c.Type == "wiuid").Select(y => y.Value).FirstOrDefault();
            }

            return user;
        }

        [EnableCors("AllowAll")]
        [AllowAnonymous]
        [HttpGet("status")]
        public IActionResult GetServerStatus()
        {
            return Ok(new { Date = _appContext.Date.ToLocalTime(), Program.Version, environment = _databaseConfig.Catalog });
        }

        [EnableCors("AllowAll")]
        [AllowAnonymous]
        [HttpGet("date")]
        public IActionResult GetServerDateTime()
        {
            return Ok(new { Date = _appContext.Date.ToLocalTime() });
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _authService.GetRoles().Select(x => new { x.Code, x.Name });
            return Ok(roles);
        }

        [HttpGet("usernames")]
        public IActionResult GetUsernames()
        {
            var names = _authService.GetAllUserames();
            return Ok(names);
        }

        [HttpGet("offices")]
        public IActionResult GetOffices()
        {
            var offices         = _authService.GetAllOffices().ToList();
            var officeContracts = new List<OfficeContract>();

            foreach (var office in offices)
            {
                if (office.ContractArea?.Organization != null)
                {
                    //var agencyContract = new AgencyContract {AgencyName = office.ContractArea?.Organization?.AgencyName, AgencyCode = office.ContractArea?.Organization?.EntsecAgencyCode, AgencyNumber = office.Agency.AgencyNumber, Id = (int)office.ContractArea?.Organization?.Id};
                    var agencyContract = new AgencyContract { AgencyName = office.ContractArea?.Organization?.AgencyName, AgencyCode = office.ContractArea?.Organization?.EntsecAgencyCode, Id = (int) office.ContractArea?.Organization?.Id };
                    var officeContract = new OfficeContract { Agency     = agencyContract, Id                                        = office.Id, OfficeNumber                                 = office.OfficeNumber };
                    officeContracts.Add(officeContract);
                }
            }

            return Ok(officeContracts);
        }

        [HttpPost("{pin}/elevatedaccess")]
        public IActionResult RequestElevatedAccess(string pin, [FromBody] ElevatedAccessContract model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new LoginViewModel(Repo, _authUser);
                    vm.InitializeFromPin(pin);

                    if (!vm.IsPinValid)
                        return NoContent();

                    vm.ElevatedAccess(model);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(PrepareExceptionForResult(ex));
                }
            }

            return BadRequest(ModelState);
        }
    }
}
